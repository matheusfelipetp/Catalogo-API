using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoriasController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
        var categorias = _unitOfWork.CategoriaRepository.GetAll();
        return Ok(categorias);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> Get(int id)
    {
        var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound("Categoria não encotrada");
        }

        return Ok(categoria);
    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        if(categoria is null)
        {
            return BadRequest();
        }

        var categoriaCriada = _unitOfWork.CategoriaRepository.Create(categoria);
        _unitOfWork.Commit();

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = categoriaCriada.CategoriaId }, categoriaCriada);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
        {
            return BadRequest("Dados inválidos");
        }

        _unitOfWork.CategoriaRepository.Update(categoria);
        _unitOfWork.Commit();

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound("Categoria não encontrada");
        }

        var categoriaExcluida = _unitOfWork.CategoriaRepository.Delete(categoria);
        _unitOfWork.Commit();

        return Ok(categoriaExcluida);
    }
}