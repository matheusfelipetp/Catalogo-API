using APICatalogo.Models;
using APICatalogo.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProdutosController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("categorias/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
    {
        var produtos = _unitOfWork.ProdutoRepository.GetProdutosPorCategoria(id);
       return Ok(produtos);
    }

   [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _unitOfWork.ProdutoRepository.GetAll();
        return Ok(produtos);
    }

    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound("Produto não encontrado");
        }

        return Ok(produto);
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
       if (produto is null)
        {
            return BadRequest();
        }

        var produtoCriado = _unitOfWork.ProdutoRepository.Create(produto);
        _unitOfWork.Commit();

        return new CreatedAtRouteResult("ObterProduto",
            new { id = produtoCriado.ProdutoId }, produtoCriado);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            return BadRequest();
        }

        var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
        _unitOfWork.Commit();

        return Ok(produtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound("Produto não encontrado");
        }

        var produtoDeletado = _unitOfWork.ProdutoRepository.Delete(produto);
        _unitOfWork.Commit();

        return Ok(produtoDeletado);
    }
}