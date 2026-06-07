using Microsoft.AspNetCore.Mvc;
using MeteoSolution.API.Models;
using MeteoSolution.API.Repositories;
using MeteoSolution.API.Controllers.DTOs;

namespace MeteoSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CidadeController : ControllerBase
{
    private readonly CidadeRepository _repository;

    public CidadeController(CidadeRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cidades = await _repository.GetAllAsync();
        return Ok(cidades);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var cidade = await _repository.GetByIdAsync(id);
        if (cidade is null)
            return NotFound(new { message = $"Cidade com id {id} não encontrada." });

        return Ok(cidade);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CidadeDTO dto)
    {
        var cidade = new Cidade
        {
            Nome = dto.Nome,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            EstadoId = dto.EstadoId
        };

        var criada = await _repository.CreateAsync(cidade);
        return CreatedAtAction(nameof(GetById), new { id = criada.Id }, criada);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CidadeDTO dto)
    {
        var cidadeAtualizada = new Cidade
        {
            Nome = dto.Nome,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            EstadoId = dto.EstadoId
        };

        var resultado = await _repository.UpdateAsync(id, cidadeAtualizada);
        if (resultado is null)
            return NotFound(new { message = $"Cidade com id {id} não encontrada." });

        return Ok(resultado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _repository.DeleteAsync(id);
        if (!deletado)
            return NotFound(new { message = $"Cidade com id {id} não encontrada." });

        return NoContent();
    }
}