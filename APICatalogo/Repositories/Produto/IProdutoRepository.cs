namespace APICatalogo.Repositories.Produto
{
    public interface IProdutoRepository : IRepository<Models.Produto>
    {
        IEnumerable<Models.Produto> GetProdutosPorCategoria(int id);
    }
}
