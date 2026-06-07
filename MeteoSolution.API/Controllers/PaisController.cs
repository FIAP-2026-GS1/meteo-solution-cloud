using Microsoft.AspNetCore.Mvc;
using MeteoSolution.API.Models;
using MeteoSolution.API.Repositories;
using MeteoSolution.API.Controllers.DTOs;

namespace MeteoSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaisController : ControllerBase
{
    private readonly PaisRepository _repository;

    public PaisController(PaisRepository repository)
    {
        _repository = repository;
    }

    private PaisResponseDTO ToResponse(Pais p) => new()
    {
        Id = p.Id,
        Nome = p.Nome,
        CodigoIso = p.CodigoIso
    };

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var paises = await _repository.GetAllAsync();
        return Ok(paises.Select(ToResponse));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var pais = await _repository.GetByIdAsync(id);
        if (pais is null)
            return NotFound(new { message = $"País com id {id} não encontrado." });

        return Ok(ToResponse(pais));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PaisDTO dto)
    {
        var pais = new Pais { Nome = dto.Nome, CodigoIso = dto.CodigoIso };
        var criado = await _repository.CreateAsync(pais);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, ToResponse(criado));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PaisDTO dto)
    {
        var resultado = await _repository.UpdateAsync(id, new Pais { Nome = dto.Nome, CodigoIso = dto.CodigoIso });
        if (resultado is null)
            return NotFound(new { message = $"País com id {id} não encontrado." });

        return Ok(ToResponse(resultado));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _repository.DeleteAsync(id);
        if (!deletado)
            return NotFound(new { message = $"País com id {id} não encontrado." });

        return NoContent();
    }
}