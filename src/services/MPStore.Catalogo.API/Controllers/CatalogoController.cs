using Microsoft.AspNetCore.Mvc;
using MPStore.Catalogo.API.Models;
using MPStore.WebAPI.Core.Controllers;

namespace MPStore.Catalogo.API.Controllers
{
    [Route("catalogo")]
    public class CatalogoController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;

        public CatalogoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpGet("produtos/{id}")]
        public async Task<Produto> Details(Guid id)
        {
            return await _produtoRepository.BuscarPorId(id);
        }

        [HttpGet("produtos")]
        public async Task<ResultadoPaginado<Produto>> Index([FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            return await _produtoRepository.BuscarTodos(ps, page, q);
        }

        [HttpGet("produtos/lista/{ids}")]
        public async Task<IEnumerable<Produto>> GetManyById(string ids)
        {
            return await _produtoRepository.BuscarProdutosPorId(ids);
        }
    }
}
