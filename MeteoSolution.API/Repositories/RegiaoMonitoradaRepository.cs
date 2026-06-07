using Microsoft.EntityFrameworkCore;
using MeteoSolution.API.Data;
using MeteoSolution.API.Models;

namespace MeteoSolution.API.Repositories;

public class RegiaoMonitoradaRepository
{
    private readonly AppDbContext _context;

    public RegiaoMonitoradaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<RegiaoMonitorada>> GetAllAsync()
    {
        return await _context.RegioesMonitoradas
            .Include(r => r.Cidade)
            .ThenInclude(c => c.Estado)
            .ThenInclude(e => e.Pais)
            .ToListAsync();
    }

    public async Task<RegiaoMonitorada?> GetByIdAsync(int id)
    {
        return await _context.RegioesMonitoradas
            .Include(r => r.Cidade)
            .ThenInclude(c => c.Estado)
            .ThenInclude(e => e.Pais)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<RegiaoMonitorada> CreateAsync(RegiaoMonitorada regiao)
    {
        _context.RegioesMonitoradas.Add(regiao);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(regiao.Id) ?? regiao;
    }

    public async Task<RegiaoMonitorada?> UpdateAsync(int id, RegiaoMonitorada regiaoAtualizada)
    {
        var regiao = await _context.RegioesMonitoradas.FindAsync(id);
        if (regiao is null) return null;

        regiao.Nome = regiaoAtualizada.Nome;
        regiao.Latitude = regiaoAtualizada.Latitude;
        regiao.Longitude = regiaoAtualizada.Longitude;
        regiao.AltitudeMedia = regiaoAtualizada.AltitudeMedia;
        regiao.DeclividadePercentual = regiaoAtualizada.DeclividadePercentual;
        regiao.CoberturaVegetalPercentual = regiaoAtualizada.CoberturaVegetalPercentual;
        regiao.ImpermeabilizacaoPercentual = regiaoAtualizada.ImpermeabilizacaoPercentual;
        regiao.DistanciaRioMetros = regiaoAtualizada.DistanciaRioMetros;
        regiao.TipoSolo = regiaoAtualizada.TipoSolo;
        regiao.NivelUrbanizacao = regiaoAtualizada.NivelUrbanizacao;
        regiao.Ativa = regiaoAtualizada.Ativa;
        regiao.CidadeId = regiaoAtualizada.CidadeId;

        await _context.SaveChangesAsync();
        return regiao;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var regiao = await _context.RegioesMonitoradas.FindAsync(id);
        if (regiao is null) return false;

        _context.RegioesMonitoradas.Remove(regiao);
        await _context.SaveChangesAsync();
        return true;
    }
}