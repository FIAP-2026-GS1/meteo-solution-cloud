namespace MeteoSolution.API.Controllers.DTOs;

public class EstadoResponseDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Sigla { get; set; } = string.Empty;
    public PaisResponseDTO? Pais { get; set; }
}