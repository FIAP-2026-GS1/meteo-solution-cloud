namespace MeteoSolution.API.Models;

public class Cidade
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // Relacionamento com Estado
    public int EstadoId { get; set; }
    public Estado Estado { get; set; } = null!;

    // Navigation Property reversa
    public ICollection<RegiaoMonitorada> RegioesMonitoradas { get; set; } 
        = new List<RegiaoMonitorada>();
}