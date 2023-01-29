using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athena.AspDotnetHelper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ShoppingCard.Service.IServices;
using ShoppingCard.Service.Models;

namespace ShoppingCard.Service.Services;

public class JwtService : IJwtService
{
    public string CreateJwt(Guid id, string key)
    {
        var jwtObject = new JwtObject()
        {
            Id = id,
        };

        return AthenaJwtHelper.CreateJwt(jwtObject, key);
    }

    public bool ValidateJwt(string token, string key)
    {
        return AthenaJwtHelper.ValidateJwt(token, key);
    }

    public JwtObject GetJwtObjectFromHttpContext(HttpContext context)
    {
        var noPayload = !context.Request.Headers.TryGetValue("token-payload", out var payload);

        if (noPayload) throw new Exception("token-payload not found from request.headers.");

        var decodedPayload = AthenaJwtHelper.Base64Decode(payload);
        var jwtObject = JsonConvert.DeserializeObject<JwtObject>(Encoding.UTF8.GetString(decodedPayload));
        return jwtObject;
    }
}