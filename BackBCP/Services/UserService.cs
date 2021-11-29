using BackBCP.Data;
using BackBCP.Models;
using BackBCP.Models.Common;
using BackBCP.Models.Request;
using BackBCP.Models.Response;
using BackBCP.Tools;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BackBCP.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly DbBcpContext _dbBcpContext;

        public UserService(IOptions<AppSettings> appSettings, DbBcpContext dbBcpContext)
        {
            _appSettings = appSettings.Value;
            _dbBcpContext = dbBcpContext;
        }
        public UserDTO Auth(AuthRequest model)
        {
            UserDTO userDTO = new UserDTO();

            string spassword = Encrypt.GetSHA256(model.Password);
            var usuario = _dbBcpContext.usuario.Where(d => d.Email == model.Email && d.Password == spassword).FirstOrDefault();
            if (usuario == null) return null;
            userDTO.Email = usuario.Email;
            userDTO.Token = GetToken(usuario);
            return userDTO;
        }

        private string GetToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //claim es la informacion que vamos a tener en el token
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                        new Claim(ClaimTypes.Email, usuario.Email)
                    }),
               
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
