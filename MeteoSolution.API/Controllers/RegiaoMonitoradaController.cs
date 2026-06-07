using Microsoft.AspNetCore.Mvc;
using MeteoSolution.API.Models;
using MeteoSolution.API.Repositories;
using MeteoSolution.API.Controllers.DTOs;

namespace MeteoSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegiaoMonitoradaController : ControllerBase
{
    private readonly RegiaoMonitoradaRepository _repository;

    public RegiaoMonitoradaController(RegiaoMonitoradaRepository repository)
    {
        _repository = repository;
    }

    private RegiaoMonitoradaResponseDTO ToResponse(RegiaoMonitorada r) => new()
    {
        Id = r.Id,
        Nome = r.Nome,
        Latitude = r.Latitude,
        Longitude = r.Longitude,
        AltitudeMedia = r.AltitudeMedia,
        DeclividadePercentual = r.DeclividadePercentual,
        CoberturaVegetalPercentual = r.CoberturaVegetalPercentual,
        ImpermeabilizacaoPercentual = r.ImpermeabilizacaoPercentual,
        DistanciaRioMetros = r.DistanciaRioMetros,
        TipoSolo = r.TipoSolo,
        NivelUrbanizacao = r.NivelUrbanizacao,
        Ativa = r.Ativa,
        Cidade = r.Cidade is null ? null : new CidadeResponseDTO
        {
            Id = r.Cidade.Id,
            Nome = r.Cidade.Nome,
            Latitude = r.Cidade.Latitude,
            Longitude = r.Cidade.Longitude,
            Estado = r.Cidade.Estado is null ? null : new EstadoResponseDTO
            {
                Id = r.Cidade.Estado.Id,
                Nome = r.Cidade.Estado.Nome,
                Sigla = r.Cidade.Estado.Sigla,
                Pais = r.Cidade.Estado.Pais is null ? null : new PaisResponseDTO
                {
                    Id = r.Cidade.Estado.Pais.Id,
                    Nome = r.Cidade.Estado.Pais.Nome,
                    CodigoIso = r.Cidade.Estado.Pais.CodigoIso
                }
            }
        }
    };

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var regioes = await _repository.GetAllAsync();
        return Ok(regioes.Select(ToResponse));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var regiao = await _repository.GetByIdAsync(id);
        if (regiao is null)
            return NotFound(new { message = $"Região com id {id} não encontrada." });

        return Ok(ToResponse(regiao));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RegiaoMonitoradaDTO dto)
    {
        var regiao = new RegiaoMonitorada
        {
            Nome = dto.Nome,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            AltitudeMedia = dto.AltitudeMedia,
            DeclividadePercentual = dto.DeclividadePercentual,
            CoberturaVegetalPercentual = dto.CoberturaVegetalPercentual,
            ImpermeabilizacaoPercentual = dto.ImpermeabilizacaoPercentual,
            DistanciaRioMetros = dto.DistanciaRioMetros,
            TipoSolo = dto.TipoSolo,
            NivelUrbanizacao = dto.NivelUrbanizacao,
            Ativa = dto.Ativa,
            CidadeId = dto.CidadeId
        };

        var criada = await _repository.CreateAsync(regiao);
        return CreatedAtAction(nameof(GetById), new { id = criada.Id }, ToResponse(criada));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RegiaoMonitoradaDTO dto)
    {
        var regiaoAtualizada = new RegiaoMonitorada
        {
            Nome = dto.Nome,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            AltitudeMedia = dto.AltitudeMedia,
            DeclividadePercentual = dto.DeclividadePercentual,
            CoberturaVegetalPercentual = dto.CoberturaVegetalPercentual,
            ImpermeabilizacaoPercentual = dto.ImpermeabilizacaoPercentual,
            DistanciaRioMetros = dto.DistanciaRioMetros,
            TipoSolo = dto.TipoSolo,
            NivelUrbanizacao = dto.NivelUrbanizacao,
            Ativa = dto.Ativa,
            CidadeId = dto.CidadeId
        };

        var resultado = await _repository.UpdateAsync(id, regiaoAtualizada);
        if (resultado is null)
            return NotFound(new { message = $"Região com id {id} não encontrada." });

        return Ok(ToResponse(resultado));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletado = await _repository.DeleteAsync(id);
        if (!deletado)
            return NotFound(new { message = $"Região com id {id} não encontrada." });

        return NoContent();
    }
}