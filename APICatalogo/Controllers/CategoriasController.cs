using APICatalogo.DTOs;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoriasController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
    {
        var metadata = new
        {
            categorias.TotalCount,
            categorias.PageSize,
            categorias.CurrentPage,
            categorias.TotalPages,
            categorias.HasNext,
            categorias.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

        return Ok(categoriasDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasPagination([FromQuery] CategoriasParameters categoriasParameters)
    {
        var categorias = _unitOfWork.CategoriaRepository.GetCategorias(categoriasParameters);
        return ObterCategorias(categorias);
    }

    [HttpGet("filter/nome/pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasFiltradas([FromQuery] CategoriasFiltroNome categoriasFiltroParameters)
    {
        var categorias = _unitOfWork.CategoriaRepository.GetCategoriasFiltroNome(categoriasFiltroParameters);
        return ObterCategorias(categorias);
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<CategoriaDTO>> Get()
    {
        var categorias = _unitOfWork.CategoriaRepository.GetAll();
        var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

        return Ok(categoriasDto);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id)
    {
        var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound("Categoria não encotrada");
        }

        var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

        return Ok(categoriaDto);
    }

    [HttpPost]
    public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
    {
        if(categoriaDto is null)
        {
            return BadRequest();
        }

        var categoria = _mapper.Map<Categoria>(categoriaDto);

        var categoriaCriada = _unitOfWork.CategoriaRepository.Create(categoria);
        _unitOfWork.Commit();

        var novaCategoriaDto = _mapper.Map<CategoriaDTO>(categoriaCriada);

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
        {
            return BadRequest("Dados inválidos");
        }

        var categoria = _mapper.Map<Categoria>(categoriaDto);

        var categoriaAtualizada = _unitOfWork.CategoriaRepository.Update(categoria);
        _unitOfWork.Commit();

        var categoriaAtualizadaDto = _mapper.Map<CategoriaDTO>(categoriaAtualizada);

        return Ok(categoriaAtualizadaDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> Delete(int id)
    {
        var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound("Categoria não encontrada");
        }

        var categoriaExcluida = _unitOfWork.CategoriaRepository.Delete(categoria);
        _unitOfWork.Commit();

        var categoriaExcluidaDto = _mapper.Map<CategoriaDTO>(categoriaExcluida);

        return Ok(categoriaExcluidaDto);
    }
}