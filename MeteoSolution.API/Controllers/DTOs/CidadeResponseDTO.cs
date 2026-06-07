namespace MeteoSolution.API.Controllers.DTOs;

public class CidadeResponseDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public EstadoResponseDTO? Estado { get; set; }
}