namespace MPStore.Catalogo.API.Models
{
    public class ResultadoPaginado<T> where T : class
    {
        public IEnumerable<T> Lista {  get; set; }
        public int ResultadoTotal { get; set; }
        public int IndexPagina { get; set; }
        public int TamanhoTotalPagina { get; set; }
        public string? Query {  get; set; }
    }
}
