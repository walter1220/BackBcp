using BackBCP.Data;
using BackBCP.Models;
using BackBCP.Models.Request;
using System;
using System.Linq;

namespace BackBCP.Business
{
    public class BCalculo: IBCalculo
    {
        private readonly DbBcpContext _dbBcpContext;

        public BCalculo(DbBcpContext dbBcpContext)
        {
            _dbBcpContext = dbBcpContext;
        }

        public TipoCambio CalculoAplicadoCambio(TipoCambioRequest oTipoCambioRequest)
        {
            TipoCambio tipoCambioTotal = new TipoCambio();

            var Origen = ValorMonedaOrigen(oTipoCambioRequest);
            decimal ValorOrigen = Convert.ToDecimal(Origen.TipoDeCambio);
            string simboloInicial = Origen.SimboloOrigen;

            var Destino = ValorMonedaDestino(oTipoCambioRequest);
            decimal ValorDestino = Convert.ToDecimal(Destino.TipoDeCambio);
            string simbolofinal = Destino.SimboloDestino;

            decimal tipoCambioCalculado = ValorOrigen / ValorDestino;
            tipoCambioCalculado = decimal.Round(tipoCambioCalculado, 4);

            decimal montoCalculado = oTipoCambioRequest.Monto * (ValorOrigen / ValorDestino);
            montoCalculado = decimal.Round(montoCalculado, 2);

            tipoCambioTotal.Monto = oTipoCambioRequest.Monto;
            tipoCambioTotal.TipoDeCambioResultado = simbolofinal + ' ' + tipoCambioCalculado;
            tipoCambioTotal.MontoConTipoCambio = simbolofinal + ' ' + montoCalculado;

            tipoCambioTotal.SimboloOrigen = simboloInicial;
            tipoCambioTotal.SimboloDestino = simbolofinal;

            return tipoCambioTotal;
        }

        public TipoCambio ValorMonedaOrigen(TipoCambioRequest oTipoCambioRequest)
        {
            TipoCambio tipoCambio1 = new TipoCambio();

            try
            {
                tipoCambio1 = (from tm in _dbBcpContext.tipoMoneda
                               where tm.Moneda == oTipoCambioRequest.MonedaOrigen
                               select new TipoCambio
                               {
                                   TipoDeCambio = tm.Valor,
                                   SimboloOrigen = tm.Simbolo
                               }).First();
            }
            catch (Exception ex)
            {

                throw new Exception("Error al digitar la Moneda Origen", ex);
            }

            return tipoCambio1;
        }

        public TipoCambio ValorMonedaDestino(TipoCambioRequest oTipoCambioRequest)
        {
            TipoCambio tipoCambio2 = new TipoCambio();

            try
            {
                tipoCambio2 = (from tm in _dbBcpContext.tipoMoneda
                               where tm.Moneda == oTipoCambioRequest.MonedaDestino
                               select new TipoCambio
                               {
                                   TipoDeCambio = tm.Valor,
                                   SimboloDestino = tm.Simbolo
                               }).First();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al digitar la Moneda Destino", ex);
            }
            return tipoCambio2;
        }
    }
}
