
using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories.Produto
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Models.Produto> GetProdutos()
        {
            return _context.Produtos.AsNoTracking().ToList();
        }

        public Models.Produto GetProduto(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId  == id);

            if (produto is null)
            {
                throw new ArgumentNullException("Produto não encontrado");
            }

            return produto;
        }

        public Models.Produto Create(Models.Produto produto)
        {
            if (produto is null)
            {
                throw new InvalidOperationException("Dados inválidos");
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return produto;
        }

        public Models.Produto Update(Models.Produto produto)
        {
            if (produto is null)
                throw new ArgumentNullException(nameof(produto));

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return produto;
        }

        public void Delete(int id)
        {
            var produto = _context.Produtos.Find(id);

            if (produto is null)
                throw new ArgumentNullException(nameof(produto));

            _context.Produtos.Remove(produto);
            _context.SaveChanges();
        }
    }
}
