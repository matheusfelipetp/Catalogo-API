using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

    private ActionResult<IEnumerable<ProdutoDTO>> OberProdutos(PagedList<Produto> produtos)
    {
        var metadata = new
        {
            produtos.TotalCount,
            produtos.PageSize,
            produtos.CurrentPage,
            produtos.TotalPages,
            produtos.HasNext,
            produtos.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDto);
    }

    [HttpGet("categorias/{id}")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
    {
        var produtos = _unitOfWork.ProdutoRepository.GetProdutosPorCategoria(id);
        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

       return Ok(produtosDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPagination([FromQuery] ProdutosParameters produtosParameters)
    {
        var produtos = _unitOfWork.ProdutoRepository.GetProdutos(produtosParameters);
        return OberProdutos(produtos);
    }

    [HttpGet("filter/preco/pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosFiltroPreco([FromQuery] ProdutosFiltroPreco produtosFiltroPreco)
    {
        var produtos = _unitOfWork.ProdutoRepository.GetProdutosFiltroPreco(produtosFiltroPreco);
        return OberProdutos(produtos);
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

    [HttpPatch("{id}")]
    public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDto)
    {
        if(patchProdutoDto is null || id <= 0)
            return BadRequest();

        var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

        if(produto is null)
            return NotFound("Produto não encontrado");

        var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

        patchProdutoDto.ApplyTo(produtoUpdateRequest, ModelState);

        if(!TryValidateModel(produtoUpdateRequest) || !ModelState.IsValid)
            return BadRequest(ModelState);

        _mapper.Map(produtoUpdateRequest, produto);

        var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
        _unitOfWork.Commit();

        var produtoDto = _mapper.Map<ProdutoDTOUpdateResponse>(produtoAtualizado);

        return Ok(produtoDto);
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