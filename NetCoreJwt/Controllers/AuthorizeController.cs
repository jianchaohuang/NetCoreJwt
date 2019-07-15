using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCoreJwt.Models;

namespace NetCoreJwt.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthorizeController : Controller
    {
         private JwtSettings setting;
         public AuthorizeController(IOptions<JwtSettings> options)
         {
             setting = options.Value;
         }
 
         [HttpPost]
         public IActionResult Login(LoginViewModel login)
         {
             if (ModelState.IsValid)
             {
                 if (login.UserName == "wangshibang" && login.Password == "123456")
                 {
                     var claims = new Claim[] {
                         new Claim(ClaimTypes.Name, login.UserName),
                         new Claim(ClaimTypes.Role, "admin")
                     };
                     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.SecretKey));
                     var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                     var token = new JwtSecurityToken(
                         setting.Issuer,
                         setting.Audience,
                         claims,
                         DateTime.Now,
                         DateTime.Now.AddMinutes(30),
                         creds);
                     return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
                 }
             }
             return BadRequest();
         }
 
         [HttpGet]
         public IActionResult NoValidate()
         {
             return Ok();
         }
     
    }
}