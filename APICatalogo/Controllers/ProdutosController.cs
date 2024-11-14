﻿using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProdutosController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet("categorias/{id}")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
    {
        var produtos = _unitOfWork.ProdutoRepository.GetProdutosPorCategoria(id);
        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

       return Ok(produtosDto);
    }

   [HttpGet]
    public ActionResult<IEnumerable<ProdutoDTO>> Get()
    {
        var produtos = _unitOfWork.ProdutoRepository.GetAll();
        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);
    }

    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> Get(int id)
    {
        var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound("Produto não encontrado");
        }

        var produtoDto = _mapper.Map<ProdutoDTO>(produto);

        return Ok(produtoDto);
    }

    [HttpPost]
    public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
    {
       if (produtoDto is null)
        {
            return BadRequest();
        }

       var produto = _mapper.Map<Produto>(produtoDto);  

        var produtoCriado = _unitOfWork.ProdutoRepository.Create(produto);
        _unitOfWork.Commit();

        var novoProdutoDto = _mapper.Map<ProdutoDTO>(produtoCriado);

        return new CreatedAtRouteResult("ObterProduto",
            new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
    {
        if (id != produtoDto.ProdutoId)
            return BadRequest();

        var produto = _mapper.Map<Produto>(produtoDto);

        var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
        _unitOfWork.Commit();

        var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);

        return Ok(produtoAtualizadoDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> Delete(int id)
    {
        var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound("Produto não encontrado");
        }

        var produtoDeletado = _unitOfWork.ProdutoRepository.Delete(produto);
        _unitOfWork.Commit();

        var produtoDto = _mapper.Map<ProdutoDTO>(produtoDeletado);

        return Ok(produtoDto);
    }
}