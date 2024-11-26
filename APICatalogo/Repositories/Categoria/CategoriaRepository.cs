using APICatalogo.Context;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories.Categoria
{
    public class CategoriaRepository : Repository<Models.Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context){ }

        public PagedList<Models.Categoria> GetCategorias(CategoriasParameters categoriasParams)
        {
            var categorias = GetAll().OrderBy(c => c.Nome).AsQueryable();
            var categoriasOrdenadas = PagedList<Models.Categoria>.ToPagedList(categorias, categoriasParams.PageNumber, categoriasParams.PageSize);

            return categoriasOrdenadas;
        }

        public PagedList<Models.Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasFiltroParams)
        {
            var categorias = GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(categoriasFiltroParams.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasFiltroParams.Nome));
            }

            var categoriasFiltradas = PagedList<Models.Categoria>.ToPagedList(categorias, categoriasFiltroParams.PageNumber, categoriasFiltroParams.PageSize);

            return categoriasFiltradas;
        }
    }
}
