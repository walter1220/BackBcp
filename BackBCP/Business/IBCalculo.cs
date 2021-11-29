using BackBCP.Models;
using BackBCP.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackBCP.Business
{
    public interface IBCalculo
    {
       TipoCambio CalculoAplicadoCambio(TipoCambioRequest oTipoCambioRequest);
    }
}
