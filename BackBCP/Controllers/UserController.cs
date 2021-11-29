using BackBCP.Data;
using BackBCP.Models;
using BackBCP.Models.Request;
using BackBCP.Models.Response;
using BackBCP.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackBCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DbBcpContext _dbBcpContext;
        private IUserService _userService;
        public UserController(IUserService userService, DbBcpContext dbBcpContext)
        {
            _dbBcpContext = dbBcpContext;
            _userService = userService;

            if (_dbBcpContext.usuario.Count() == 0)
            {
                List<Usuario> ListaUsuario = new List<Usuario>();
                ListaUsuario.Add(new Usuario { Email = "walterbc12@gmail.com", Password = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92", Nombre = "Walter" });
                
                _dbBcpContext.usuario.AddRange(ListaUsuario);
                _dbBcpContext.SaveChanges();
            }
        }


        [HttpPost("login")]
        public IActionResult Autentificar([FromBody] AuthRequest model)
        {
            Respuesta respuesta = new Respuesta();
            var userresponse = _userService.Auth(model);

            if (userresponse == null)
            {
                respuesta.Mensaje = "Usuario o Contraseña Incorrecta";
                return BadRequest(respuesta);
            }
            respuesta.Exito = 1;
            respuesta.Data = userresponse;

            return Ok(respuesta);
        }
    }
}
