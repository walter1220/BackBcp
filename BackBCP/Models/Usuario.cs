using System.ComponentModel.DataAnnotations;

namespace BackBCP.Models
{
    public class Usuario
    {
        [Key]
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
    }
}
