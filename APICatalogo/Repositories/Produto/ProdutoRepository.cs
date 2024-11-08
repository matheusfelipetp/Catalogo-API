using APICatalogo.Context;

namespace APICatalogo.Repositories.Produto
{
    public class ProdutoRepository : Repository<Models.Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Models.Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id);
        }
    }
}
