using System.ComponentModel.DataAnnotations;

namespace BackBCP.Models
{
    public class TipoMoneda
    {
        [Key]
        public int Id { get; set; }
        public string Moneda { get; set; }
        public string Simbolo { get; set; }
        public string Valor { get; set; } 
    }
}
