using APICatalogo.Pagination;

namespace APICatalogo.Repositories.Categoria
{
    public interface ICategoriaRepository : IRepository<Models.Categoria>
    {
        PagedList<Models.Categoria> GetCategorias(CategoriasParameters categoriasParams);
        PagedList<Models.Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasFiltroParams);
    }
}
