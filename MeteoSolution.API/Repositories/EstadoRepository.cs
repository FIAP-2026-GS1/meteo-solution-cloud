using Microsoft.EntityFrameworkCore;
using MeteoSolution.API.Data;
using MeteoSolution.API.Models;

namespace MeteoSolution.API.Repositories;

public class EstadoRepository
{
    private readonly AppDbContext _context;

    public EstadoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Estado>> GetAllAsync()
    {
        return await _context.Estados
            .Include(e => e.Pais)
            .ToListAsync();
    }

    public async Task<Estado?> GetByIdAsync(int id)
    {
        return await _context.Estados
            .Include(e => e.Pais)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Estado> CreateAsync(Estado estado)
    {
        _context.Estados.Add(estado);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(estado.Id) ?? estado;
    }

    public async Task<Estado?> UpdateAsync(int id, Estado estadoAtualizada)
    {
        var estado = await _context.Estados.FindAsync(id);
        if (estado is null) return null;

        estado.Nome = estadoAtualizada.Nome;
        estado.Sigla = estadoAtualizada.Sigla;
        estado.PaisId = estadoAtualizada.PaisId;

        await _context.SaveChangesAsync();
        return estado;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var estado = await _context.Estados.FindAsync(id);
        if (estado is null) return false;

        _context.Estados.Remove(estado);
        await _context.SaveChangesAsync();
        return true;
    }
}