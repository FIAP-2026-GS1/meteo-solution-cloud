namespace MeteoSolution.API.Controllers.DTOs;

public class RegiaoMonitoradaResponseDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double AltitudeMedia { get; set; }
    public double DeclividadePercentual { get; set; }
    public double CoberturaVegetalPercentual { get; set; }
    public double ImpermeabilizacaoPercentual { get; set; }
    public double DistanciaRioMetros { get; set; }
    public string TipoSolo { get; set; } = string.Empty;
    public string NivelUrbanizacao { get; set; } = string.Empty;
    public bool Ativa { get; set; }
    public CidadeResponseDTO? Cidade { get; set; }
}