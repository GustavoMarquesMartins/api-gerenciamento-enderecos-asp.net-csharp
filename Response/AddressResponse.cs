namespace GerenciamentoDeEndereco.Response
{
    public class AddressResponse
    {
        public long Id { get; set; }
        public string ZipCode { get; set; } 
        public string Street { get; set; } 
        public string AdditionalInfo { get; set; } 
        public string Neighborhood { get; set; } 
        public string City { get; set; }
        public string State { get; set; } 
        public int Number { get; set; }
    }
}


