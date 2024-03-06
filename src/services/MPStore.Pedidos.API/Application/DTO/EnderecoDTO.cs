namespace MPStore.Pedidos.API.Application.DTO
{
    public class EnderecoDTO
    {
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string EnderecoSecundario { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}
