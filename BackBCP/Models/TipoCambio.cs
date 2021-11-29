using System.ComponentModel.DataAnnotations;

namespace BackBCP.Models
{
    public class TipoCambio
    {
        [Key]
        public int Id { get; set; }
        public int Monto { get; set; }
        public string MonedaOrigen { get; set; }
        public string MonedaDestino { get; set; }
        public string MontoConTipoCambio { get; set; }
        public string TipoDeCambio { get; set; }

        public string TipoDeCambioResultado { get; set; }

        public string SimboloOrigen { get; set; }
        public string SimboloDestino { get; set; }
    }
}
