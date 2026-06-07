using Microsoft.EntityFrameworkCore;
using MeteoSolution.API.Data;
using MeteoSolution.API.Models;

namespace MeteoSolution.API.Repositories;

public class CidadeRepository
{
    private readonly AppDbContext _context;

    public CidadeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Cidade>> GetAllAsync()
    {
        return await _context.Cidades
            .Include(c => c.Estado)
            .ThenInclude(e => e.Pais)
            .ToListAsync();
    }

    public async Task<Cidade?> GetByIdAsync(int id)
    {
        return await _context.Cidades
            .Include(c => c.Estado)
            .ThenInclude(e => e.Pais)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Cidade> CreateAsync(Cidade cidade)
    {
        _context.Cidades.Add(cidade);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(cidade.Id) ?? cidade;
    }

    public async Task<Cidade?> UpdateAsync(int id, Cidade cidadeAtualizada)
    {
        var cidade = await _context.Cidades.FindAsync(id);
        if (cidade is null) return null;

        cidade.Nome = cidadeAtualizada.Nome;
        cidade.Latitude = cidadeAtualizada.Latitude;
        cidade.Longitude = cidadeAtualizada.Longitude;
        cidade.EstadoId = cidadeAtualizada.EstadoId;

        await _context.SaveChangesAsync();
        return cidade;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cidade = await _context.Cidades.FindAsync(id);
        if (cidade is null) return false;

        _context.Cidades.Remove(cidade);
        await _context.SaveChangesAsync();
        return true;
    }
}