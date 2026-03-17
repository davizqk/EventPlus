using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO;

public class UsuarioDTO
{
    [Required(ErrorMessage = "O Nome do usuário é obrigatório!")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "O Email do usuário é obrigatório!")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "A Senha do usuário é obrigatório!")]
    public string? Senha { get; set; }
    public Guid? IdTipoUsuario { get; set; }
}
