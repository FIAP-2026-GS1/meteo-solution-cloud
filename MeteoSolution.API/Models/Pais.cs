namespace MeteoSolution.API.Models;

public class Pais
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CodigoIso { get; set; } = string.Empty;

    public ICollection<Estado> Estados { get; set; } 
        = new List<Estado>();
}