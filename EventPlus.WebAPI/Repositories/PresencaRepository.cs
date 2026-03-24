using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories;

public class PresencaRepository : IPresencaRepository
{
    private readonly EventContext _eventoContext;

    public PresencaRepository(EventContext eventContext)
    {
        _eventoContext = eventContext;
    }

    public void Atualizar(Guid IdPresencaEvento, Presenca presenca)
    {
        var presencaBuscada = _eventoContext.Presencas.Find(IdPresencaEvento);

        if (presencaBuscada != null)
        {
            presencaBuscada.Situacao = !presencaBuscada.Situacao;

            _eventoContext.SaveChanges();
        }
    }

    /// <summary>
    /// Busca presença por id
    /// </summary>
    /// <param name="id">id da presença a ser buscada</param>
    /// <returns>presença buscada</returns>
    public Presenca BuscarPorId(Guid id)
    {
        return _eventoContext.Presencas
            .Include(p => p.IdEventoNavigation)
            .ThenInclude(e => e!.IdInstituicaoNavigation)
            .FirstOrDefault(p => p.IdPresenca == id)!;
    }

    public void Deletar(Guid IdPresenca)
    {
        var presencaBuscada = _eventoContext.Presencas.Find(IdPresenca);

        if (presencaBuscada != null)
        {
            _eventoContext.Presencas.Remove(presencaBuscada);
            _eventoContext.SaveChanges();
        }
    }

    public void Inscrever(Presenca Inscricao)
    {
        _eventoContext.Presencas.Add(Inscricao);
        _eventoContext.SaveChanges();
    }

    public List<Presenca> Listar()
    {
        return _eventoContext.Presencas.OrderBy(Presenca => Presenca.Situacao).ToList();
    }

    /// <summary>
    /// Lista as presenças de um usuário específico
    /// </summary>
    /// <param name="IdUsuario">id do usuário para filtragem</param>
    /// <returns>uma lista de presenças de um usuário específico</returns>
    public List<Presenca> ListarMinhas(Guid IdUsuario)
    {
        return _eventoContext.Presencas
            .Include(p => p.IdEventoNavigation)
            .ThenInclude(e => e!.IdInstituicaoNavigation)
            .Where(p => p.IdUsuario == IdUsuario)
            .ToList();
    }
}
