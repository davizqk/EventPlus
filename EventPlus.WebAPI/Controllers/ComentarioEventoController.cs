using Azure;
using Azure.AI.ContentSafety;
using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventPlus.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ComentarioEventoController : ControllerBase
{
    private readonly ContentSafetyClient _contentSafetyClient;
    private readonly IComentarioEventoRepository _comentarioEventoRepository;
    public ComentarioEventoController(ContentSafetyClient contentSafetyClient,
        IComentarioEventoRepository comentarioEventoRepository)
    {
        _comentarioEventoRepository = comentarioEventoRepository;
        _contentSafetyClient = contentSafetyClient;
    }

    [HttpGet]
    public IActionResult Listar(Guid IdEvento)
    {
        try
        {
            return Ok(_comentarioEventoRepository.Listar(IdEvento));
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }

    }

    /// <summary>
    /// Endpoint da API que cadastra e modera um comentário
    /// </summary>
    /// <param name="comentarioEvento">comentário a ser moderado</param>
    /// <returns>Status Code 201 e o comentário criado</returns>
    [HttpPost]
    public async Task<IActionResult> Cadastrar(ComentarioEventoDTO comentarioEvento)
    {
        try
        {
            if (string.IsNullOrEmpty(comentarioEvento.Descricao))
            {
                return BadRequest("O texto a ser moderado não pode estar vazio");
            }

            var request = new AnalyzeTextOptions(comentarioEvento.Descricao);

            Response<AnalyzeTextResult> response = await
                _contentSafetyClient.AnalyzeTextAsync(request);

            bool temConteudoImproprio = response.Value.CategoriesAnalysis.Any
                (comentario => comentario.Severity > 0);

            var novoComentarioEvento = new ComentarioEvento
            {
                Descricao = comentarioEvento.Descricao!,
                IdUsuario = comentarioEvento.IdUsuario!,
                IdEvento = comentarioEvento.IdEvento!,
                DataComentarioEvento = DateTime.Now!,
                Exibe = !temConteudoImproprio
            };

            _comentarioEventoRepository.Cadastrar(novoComentarioEvento);
            return StatusCode(201, novoComentarioEvento);
        }
        catch (Exception erro)
        {
            return BadRequest(erro.Message);
        }
    }



    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        try
        {
            _comentarioEventoRepository.Deletar(id);
            return NoContent();
        }
        catch (Exception erro)
        {

            return BadRequest(erro.Message);
        }
    }
}

