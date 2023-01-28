using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace Athena.AspDotnetHelper;

public static class AthenaJwtHelper
{
    public static string Base64Encode(byte[] input)
    {
        var output = Convert.ToBase64String(input);
        return output.Trim('=');
    }

    public static byte[] Base64Decode(string input)
    {
        switch (input.Length % 4)
        {
            case 0: break;
            case 2:
                input += "==";
                break;
            case 3:
                input += "=";
                break;
            default: throw new Exception("Illegal base64url string!");
        }

        var output = Convert.FromBase64String(input);
        return output;
    }


    public static string CreateJwt(object payload, string key)
    {
        return CreateJwt(payload, Encoding.UTF8.GetBytes(key));
    }

    public static bool ValidateJwt(string payload, string key)
    {
        return ValidateJwt(payload, Encoding.UTF8.GetBytes(key));
    }


    private static string CreateJwt(object payload, byte[] keyBytes)
    {
        var segments = new List<string>();
        var header = new { alg = "HS256", typ = "JWT" };

        var headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Formatting.None));
        var payloadBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload, Formatting.None));

        // segments should be encoded before signing
        segments.Add(Base64Encode(headerBytes));
        segments.Add(Base64Encode(payloadBytes));

        var stringToSign = string.Join(".", segments);

        var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

        var signature = new HMACSHA256(keyBytes).ComputeHash(bytesToSign);

        segments.Add(Base64Encode(signature));

        return string.Join(".", segments);
    }

    private static bool ValidateJwt(string token, byte[] keyBytes)
    {
        // todo: this has some bugs in validating token
        try
        {
            var parts = token.Split('.');
            if (parts.Length != 3) return false;

            var header = parts[0];
            var payload = parts[1];
            var crypto = Base64Decode(parts[2]);

            var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
            var signature = new HMACSHA256(keyBytes).ComputeHash(bytesToSign);

            var decodedCrypto = Convert.ToBase64String(crypto);
            var decodedSignature = Convert.ToBase64String(signature);

            return string.Equals(decodedSignature, decodedCrypto);
        }
        catch
        {
            return false;
        }
    }
}