using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces;

public interface IInstituicaoRepository 
{
    List<Instituicao> Listar();
    void Cadastrar(Instituicao instituicao);
    void Atualizar(Guid id, Instituicao instituicao);
    void Deletar(Guid id);
    Instituicao BuscarPorID(Guid id);
}
