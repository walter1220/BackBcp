using System.ComponentModel.DataAnnotations;

namespace BackBCP.Models.Response
{
    public class CalculoAplicadoDTO
    {
        [Key]
        public int Monto { get; set; }
        public string MonedaOrigen { get; set; }
        public string MonedaDestino { get; set; }
        public string TipoDeCambio { get; set; }
        public string MontoConTipoCambio { get; set; }
    }
}
