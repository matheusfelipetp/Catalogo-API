﻿using APICatalogo.Models;
using APICatalogo.Repositories.Produto;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutosController(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
    {
       var produtos = _produtoRepository.GetProdutosPorCategoria(id);

       if (produtos is null)
        {
            return NotFound("Produtos não encontrados para essa categoria");
        }

       return Ok(produtos);
    }

   [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _produtoRepository.GetAll();
        return Ok(produtos);
    }

    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _produtoRepository.Get(p => p.ProdutoId == id);

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

        var produtoCriado = _produtoRepository.Create(produto);

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

        var produtoAtualizado = _produtoRepository.Update(produto);
        return Ok(produtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _produtoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound("Produto não encontrado");
        }

        var produtoDeletado = _produtoRepository.Delete(produto);
        return Ok(produtoDeletado);
    }
}