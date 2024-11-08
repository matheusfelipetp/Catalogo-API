namespace APICatalogo.Repositories.Categoria
{
    public interface ICategoriaRepository
    {
        IEnumerable<Models.Categoria> GetCategorias();
        Models.Categoria GetCategoria(int id);
        Models.Categoria Create(Models.Categoria categoria);
        Models.Categoria Update(Models.Categoria categoria);
        void Delete(int id);
    }
}
