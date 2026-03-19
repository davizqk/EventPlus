using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces;

public interface IUsuarioRepository
{
    List<Usuario> Listar();
    void Cadastrar(Usuario usuario);
    Usuario BuscarPorId(Guid id);
    Usuario BuscarPorEmailESenha(string Email, string Senha);
}
