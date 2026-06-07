namespace MeteoSolution.API.Controllers.DTOs;

public class CidadeDTO
{
    public string Nome { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int EstadoId { get; set; }
}