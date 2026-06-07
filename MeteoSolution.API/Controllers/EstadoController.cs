using Microsoft.AspNetCore.Mvc;
using MeteoSolution.API.Models;
using MeteoSolution.API.Repositories;
using MeteoSolution.API.Controllers.DTOs;

namespace MeteoSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstadoController : ControllerBase
{
    private readonly EstadoRepository _repository;

    public EstadoController(EstadoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var estados = await _repository.GetAllAsync();
        return Ok(estados);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var estado = await _repository.GetByIdAsync(id);
        if (estado is null)
            return NotFound(new { message = $"Estado com id {id} não encontrado." });

        return Ok(estado);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EstadoDTO dto)
    {
        var estado = new Estado
        {
            Nome = dto.Nome,
            Sigla = dto.Sigla,
            PaisId = dto.PaisId
        };

        var criado = await _repository.CreateAsync(estado);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EstadoDTO dto)
    {
        var estadoAtualizado = new Estado
        {
            Nome = dto.Nome,
            Sigla = dto.Sigla,
            PaisId = dto.PaisId
        };

        var resultado = await _repository.UpdateAsync(id, estadoAtualizado);
        if (resultado is null)
            return NotFound(new { message = $"Estado com id {id} não encontrado." });

        return Ok(resultado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _repository.DeleteAsync(id);
        if (!deletado)
            return NotFound(new { message = $"Estado com id {id} não encontrado." });

        return NoContent();
    }
}