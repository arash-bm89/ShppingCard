using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ShoppingCard.Service.Models;

namespace ShoppingCard.Service.IServices;

public interface IJwtService
{
    string CreateJwt(Guid id, string name, string key);

    bool ValidateJwt(string token, string key);

    JwtObject GetJwtObjectFromHttpContext(HttpContext context);
}