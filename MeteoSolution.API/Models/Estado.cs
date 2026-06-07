namespace MeteoSolution.API.Models;

public class Estado
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Sigla { get; set; } = string.Empty;

    public int PaisId { get; set; }
    public Pais Pais { get; set; } = null!;

    public ICollection<Cidade> Cidades { get; set; } 
        = new List<Cidade>();
}