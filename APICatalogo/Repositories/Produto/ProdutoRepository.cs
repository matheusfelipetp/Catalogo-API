using APICatalogo.Context;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories.Produto
{
    public class ProdutoRepository : Repository<Models.Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public PagedList<Models.Produto> GetProdutos(ProdutosParameters produtosParams)
        {
            var produtos = GetAll().OrderBy(p => p.Nome).AsQueryable();
            var produtosOrdenados = PagedList<Models.Produto>.ToPagedList(produtos, produtosParams.PageNumber, produtosParams.PageSize);

            return produtosOrdenados;
        }

        public PagedList<Models.Produto>? GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroParams)
        {
            var produtos = GetAll().AsQueryable();

            if (produtosFiltroParams.Preco != null && !string.IsNullOrEmpty(produtosFiltroParams.PrecoCriterio))
            {
                if (produtosFiltroParams.PrecoCriterio.Equals("maior"))
                {
                    produtos = produtos.Where(p => p.Preco > (decimal)produtosFiltroParams.Preco).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroParams.PrecoCriterio.Equals("menor"))
                {
                    produtos = produtos.Where(p => p.Preco < (decimal)produtosFiltroParams.Preco).OrderBy(p => p.Preco);
                }   
                else if (produtosFiltroParams.PrecoCriterio.Equals("igual"))
                {
                    produtos = produtos.Where(p => p.Preco == (decimal)produtosFiltroParams.Preco).OrderBy(p => p.Preco);
                }

                var produtosFiltrados = PagedList<Models.Produto>.ToPagedList(produtos, produtosFiltroParams.PageNumber, produtosFiltroParams.PageSize);

                return produtosFiltrados;
            } 

            return null;
        }

        public IEnumerable<Models.Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id);
        }
    }
}
