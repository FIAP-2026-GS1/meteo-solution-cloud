namespace MeteoSolution.API.Controllers.DTOs;

public class EstadoDTO
{
    public string Nome { get; set; } = string.Empty;
    public string Sigla { get; set; } = string.Empty;
    public int PaisId { get; set; }
}