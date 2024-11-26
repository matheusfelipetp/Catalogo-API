using APICatalogo.Pagination;

namespace APICatalogo.Repositories.Produto
{
    public interface IProdutoRepository : IRepository<Models.Produto>
    {
        PagedList<Models.Produto> GetProdutos(ProdutosParameters produtosParams);
        PagedList<Models.Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroParams);
        IEnumerable<Models.Produto> GetProdutosPorCategoria(int id);
    }
}
