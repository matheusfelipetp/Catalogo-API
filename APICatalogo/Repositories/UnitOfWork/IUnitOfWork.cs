using APICatalogo.Repositories.Categoria;
using APICatalogo.Repositories.Produto;

namespace APICatalogo.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }
        void Commit();
    }
}
