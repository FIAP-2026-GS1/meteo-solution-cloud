namespace MeteoSolution.API.Controllers.DTOs;

public class PaisResponseDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CodigoIso { get; set; } = string.Empty;
}