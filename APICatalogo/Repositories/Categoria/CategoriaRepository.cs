
using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories.Categoria
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public Models.Categoria GetCategoria(int id)
        {
           return _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
        }

        public IEnumerable<Models.Categoria> GetCategorias()
        {
            return _context.Categorias.AsNoTracking().ToList();
        }

        public Models.Categoria Create(Models.Categoria categoria)
        {
            if (categoria is null)
                throw new ArgumentNullException(nameof(categoria));

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return categoria;
        }

        public Models.Categoria Update(Models.Categoria categoria)
        {
            if (categoria is null)
                throw new ArgumentNullException(nameof(categoria));
            
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return categoria;
        }

        public void Delete(int id)
        {
            var categoria = _context.Categorias.Find(id);

            if (categoria is null)
                throw new ArgumentNullException(nameof(categoria));

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
        }
    }
}
