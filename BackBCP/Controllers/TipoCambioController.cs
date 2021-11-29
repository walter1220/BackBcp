using BackBCP.Business;
using BackBCP.Data;
using BackBCP.Models;
using BackBCP.Models.Request;
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
    public class TipoCambioController : ControllerBase
    {
        private readonly DbBcpContext _dbBcpContext;
        private IBCalculo _bCalculo;
        public TipoCambioController(DbBcpContext dbBcpContext, IBCalculo bCalculo)
        {
            _dbBcpContext = dbBcpContext;
            _bCalculo = bCalculo;
        }

        [HttpPost]
        public async Task<ActionResult> Post(TipoCambioRequest oTipoCambioRequest)
        {
            Respuesta oRespuesta = new Respuesta();
            CalculoAplicadoDTO oResponseCalculoAplicado = new CalculoAplicadoDTO();
            TipoCambio oTipoCambio = new TipoCambio();

            try
            {
                if (oTipoCambioRequest.Monto > 0 && oTipoCambioRequest.MonedaOrigen != "" && oTipoCambioRequest.MonedaDestino != "")
                {
                    if (oTipoCambioRequest.MonedaOrigen != oTipoCambioRequest.MonedaDestino)
                    {
                        var objCalculado = _bCalculo.CalculoAplicadoCambio(oTipoCambioRequest);

                        oResponseCalculoAplicado.Monto = objCalculado.Monto;
                        oResponseCalculoAplicado.TipoDeCambio = objCalculado.TipoDeCambioResultado;
                        oResponseCalculoAplicado.MontoConTipoCambio = objCalculado.MontoConTipoCambio;
                        oResponseCalculoAplicado.MonedaOrigen = oTipoCambioRequest.MonedaOrigen;
                        oResponseCalculoAplicado.MonedaDestino = oTipoCambioRequest.MonedaDestino;
                        oRespuesta.Exito = 1;
                        oRespuesta.Data = oResponseCalculoAplicado;

                        oTipoCambio.Monto = objCalculado.Monto;
                        oTipoCambio.TipoDeCambio = objCalculado.TipoDeCambioResultado;
                        oTipoCambio.MontoConTipoCambio = objCalculado.MontoConTipoCambio;
                        oTipoCambio.MonedaOrigen = oTipoCambioRequest.MonedaOrigen;
                        oTipoCambio.MonedaDestino = oTipoCambioRequest.MonedaDestino;
                        oTipoCambio.SimboloOrigen = objCalculado.SimboloOrigen;
                        oTipoCambio.SimboloDestino = objCalculado.SimboloDestino;

                        _dbBcpContext.tipoCambio.Add(oTipoCambio);
                        await _dbBcpContext.SaveChangesAsync();
                    }
                    else
                    {
                        oRespuesta.Mensaje = "Error: La Moneda Destino debe ser distinta a la Moneda Origen.";
                        return Ok(oRespuesta);
                    }
                }
                else
                {
                    oRespuesta.Mensaje = "Error: Los 3 campos son obligatorios, vuelva a intentarlo.";
                    return Ok(oRespuesta);
                }

             
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            Respuesta oRespuesta = new Respuesta();

            try
            {
                List<CalculoAplicadoDTO> lstMostrar = new List<CalculoAplicadoDTO>();
                CalculoAplicadoDTO oCalculoAplicado = new CalculoAplicadoDTO();
                var lista = await _dbBcpContext.tipoCambio.ToListAsync();
                if (lista.Count > 0)
                {
                    foreach (var item in lista)
                    {
                        lstMostrar.Add(new CalculoAplicadoDTO()
                        {
                            Monto = item.Monto,
                            MonedaOrigen = item.MonedaOrigen,
                            MonedaDestino = item.MonedaDestino,
                            TipoDeCambio = item.TipoDeCambio,
                            MontoConTipoCambio = item.MontoConTipoCambio
                        });
                    }               
                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lstMostrar;
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

        
    }
}
