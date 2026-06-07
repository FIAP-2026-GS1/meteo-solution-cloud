using Microsoft.EntityFrameworkCore;
using MeteoSolution.API.Data;
using MeteoSolution.API.Models;

namespace MeteoSolution.API.Repositories;

public class PaisRepository
{
    private readonly AppDbContext _context;

    public PaisRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Pais>> GetAllAsync()
    {
        return await _context.Paises.ToListAsync();
    }

    public async Task<Pais?> GetByIdAsync(int id)
    {
        return await _context.Paises.FindAsync(id);
    }

    public async Task<Pais> CreateAsync(Pais pais)
    {
        _context.Paises.Add(pais);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(pais.Id) ?? pais;
    }   

    public async Task<Pais?> UpdateAsync(int id, Pais paisAtualizado)
    {
        var pais = await _context.Paises.FindAsync(id);
        if (pais is null) return null;

        pais.Nome = paisAtualizado.Nome;
        pais.CodigoIso = paisAtualizado.CodigoIso;

        await _context.SaveChangesAsync();
        return pais;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var pais = await _context.Paises.FindAsync(id);
        if (pais is null) return false;

        _context.Paises.Remove(pais);
        await _context.SaveChangesAsync();
        return true;
    }
}