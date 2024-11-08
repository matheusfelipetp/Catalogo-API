using APICatalogo.Context;

namespace APICatalogo.Repositories.Categoria
{
    public class CategoriaRepository : Repository<Models.Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context){ }
    }
}
