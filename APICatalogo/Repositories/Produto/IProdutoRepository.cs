namespace APICatalogo.Repositories.Produto
{
    public interface IProdutoRepository
    {
        IEnumerable<Models.Produto> GetProdutos();
        Models.Produto GetProduto(int id);
        Models.Produto Create(Models.Produto produto);
        Models.Produto Update(Models.Produto produto);
        void Delete(int id);
    }
}
