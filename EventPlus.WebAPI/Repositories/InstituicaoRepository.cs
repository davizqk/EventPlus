using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories;

public class InstituicaoRepository : IInstituicaoRepository
{
    private readonly EventContext _context;

    // Injeção de dependência: Recebe o contexto pelo construtor
    public InstituicaoRepository(EventContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Atualiza uma instituição usando o rastreamento automático
    /// </summary>
    /// <param name="id">id da intituição para ser atualizado</param>
    /// <param name="instituicao">Novos dados da instituição</param>
    public void Atualizar(Guid id, Instituicao instituicao)
    {
        var instituicaoBuscada = _context.Instituicaos.Find(id)!;

        if (instituicaoBuscada != null)
        {
            instituicaoBuscada.Cnpj = String.IsNullOrWhiteSpace(instituicao.Cnpj) ? 
                instituicaoBuscada.Cnpj : instituicao.Cnpj;
            instituicaoBuscada.Endereco = String.IsNullOrWhiteSpace(instituicao.Endereco) ?
                instituicaoBuscada.Endereco : instituicao.Endereco; 
            instituicaoBuscada.NomeFantasia = String.IsNullOrWhiteSpace(instituicao.NomeFantasia) ? 
                instituicaoBuscada.NomeFantasia : instituicao.NomeFantasia;

            _context.Instituicaos.Update(instituicaoBuscada);
            _context.SaveChanges();
        }
    }

    /// <summary>
    /// Busca um tipo de instituição por id
    /// </summary>
    /// <param name="id">id da instituição a ser buscada</param>
    /// <returns>Objeto da instituicao com as informações da instituição buscada</returns>
    public Instituicao BuscarPorID(Guid id)
    {
        return _context.Instituicaos.Find(id)!;
    }

    /// <summary>
    /// Cadastra uma nova instituição
    /// </summary>
    /// <param name="instituicao">instituição a ser cadastrada</param>
    public void Cadastrar(Instituicao instituicao)
    {
        _context.Instituicaos.Add(instituicao);
        _context.SaveChanges();
    }

    /// <summary>
    /// Deleta uma instituição
    /// </summary>
    /// <param name="id">id da instituição a ser deletadaa</param>
    public void Deletar(Guid id)
    {
        var instituicaoBuscada = _context.Instituicaos.Find(id);

        if (instituicaoBuscada != null)
        {
            _context.Instituicaos.Remove(instituicaoBuscada);
            _context.SaveChanges();
        }
    }

    /// <summary>
    /// Busca a lista das instituições cadastradas
    /// </summary>
    /// <returns></returns>
    public List<Instituicao> Listar()
    {
        return _context.Instituicaos.OrderBy(Instituicao => Instituicao.NomeFantasia).ToList();
    }
}
