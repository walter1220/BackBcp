namespace BackBCP.Models.Request
{
    public class TipoCambioRequest
    {
      
        public int Monto { get; set; }
       
        public string MonedaOrigen { get; set; }
        
        public string MonedaDestino { get; set; }
    }
}
