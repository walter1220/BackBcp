using BackBCP.Data;
using BackBCP.Models;
using BackBCP.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackBCP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TipoMonedaController : ControllerBase
    {
     
        private readonly DbBcpContext _dbBcpContext;

        public TipoMonedaController(DbBcpContext dbBcpContext)
        {
            _dbBcpContext = dbBcpContext;

            if (_dbBcpContext.tipoMoneda.Count() == 0)
            {
                List<TipoMoneda> ListaTipoMoneda = new List<TipoMoneda>();

                ListaTipoMoneda.Add(new TipoMoneda { Moneda = "PEN", Simbolo = "S/", Valor = "1" });
                ListaTipoMoneda.Add(new TipoMoneda { Moneda = "USD", Simbolo = "$", Valor = "4,02" });

                _dbBcpContext.tipoMoneda.AddRange(ListaTipoMoneda);
                _dbBcpContext.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            Respuesta oRespuesta = new Respuesta();

            try
            {
                var lista = await _dbBcpContext.tipoMoneda.ToListAsync();
                if (lista.Count > 0)
                {
                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lista;
                }
                else
                {
                    oRespuesta.Mensaje = "No hay data en la Base de Datos";
                    return Ok(oRespuesta);
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            Respuesta oRespuesta = new Respuesta();
            try
            {
                var registro = await _dbBcpContext.tipoMoneda.FirstOrDefaultAsync(s => s.Id == id);
                if (registro == null)
                {
                    oRespuesta.Mensaje = "Id No existe en la Base de Datos";
                    return Ok(oRespuesta);
                }
                oRespuesta.Exito = 1;
                oRespuesta.Data = registro;
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        
        [HttpPost]
        public async Task<ActionResult> Post(TipoMoneda tipoMoneda)
        {
            Respuesta oRespuesta = new Respuesta();

            try
            {
                _dbBcpContext.tipoMoneda.Add(tipoMoneda);
                await _dbBcpContext.SaveChangesAsync();

                CreatedAtAction(nameof(GetById), new { id = tipoMoneda.Id }, tipoMoneda);
                oRespuesta.Exito = 1;
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpPut]
        public async Task<ActionResult> Put(TipoMoneda tipoMoneda)
        {
            Respuesta oRespuesta = new Respuesta();
            try
            {
                TipoMoneda otipoMoneda = await _dbBcpContext.tipoMoneda.FindAsync(tipoMoneda.Id);
                if (otipoMoneda != null)
                {
                    otipoMoneda.Moneda = tipoMoneda.Moneda;
                    otipoMoneda.Simbolo = tipoMoneda.Simbolo;
                    otipoMoneda.Valor = tipoMoneda.Valor;
                    _dbBcpContext.Entry(otipoMoneda).State = EntityState.Modified;
                    await _dbBcpContext.SaveChangesAsync();
                    oRespuesta.Exito = 1;
                }
                else
                {
                    oRespuesta.Mensaje = "Id No existe en la Base de Datos";
                    return Ok(oRespuesta);
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Respuesta oRespuesta = new Respuesta();
            var registro = await _dbBcpContext.tipoMoneda.FindAsync(id);

            try
            {
                if (registro == null)
                {
                    oRespuesta.Mensaje = "Id No existe en la Base de Datos, Elija un Id Válido.";
                    return Ok(oRespuesta);
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            _dbBcpContext.tipoMoneda.Remove(registro);
            await _dbBcpContext.SaveChangesAsync();
            oRespuesta.Exito = 1;
            return Ok(oRespuesta);            
        }
    }
}
