using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;

public class InstituicaoDTO
{
    [Required(ErrorMessage = "O titulo da instituição é obrigatório!")]
    public string? NomeFantasia { get; set; }
    public string? Cnpj { get; set; }
    public string? Endereco { get; set; }
}
