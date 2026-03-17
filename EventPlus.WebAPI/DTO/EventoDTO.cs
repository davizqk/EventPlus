namespace EventPlus.WebAPI.DTO;

public class EventoDTO
{
    public DateTime? DataEvento { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public Guid? IdTipoEvento { get; set; }

}
