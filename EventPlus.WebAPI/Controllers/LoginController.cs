using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EventPlus.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public LoginController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    [HttpPost]
    public IActionResult Login(LoginDTO loginDto)
    {
        try
        {
            Usuario usuarioBuscado = _usuarioRepository.BuscarPorEmailESenha(loginDto.Email!, loginDto.Senha!);

            if (usuarioBuscado == null)
            {
                return NotFound("Email ou Senha inválidos!");
            }

            //caso encontre o usuario, prosseguir para criação do token 
            //1 - Definir as informações(claims) que serão fornecidas no token (Payload)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, usuarioBuscado.Senha),

                new Claim(JwtRegisteredClaimNames.Email, usuarioBuscado.Email!)

                //existe a possibilidade de criar uma claim personalizada
                //new Claim("Claim Personalizada", "Valor da claim personalizada")
            };

            //2 - definir a chave de acesso token
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes
                ("eventos-chave-autenticacao-webapi-dev"));

            //3 - Definir as credenciais do token(Header)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //4 - Gerar o token
            var token = new JwtSecurityToken
                (
                 issuer: "api_event",

                 audience: "api_event",

                 claims: claims,

                 expires: DateTime.Now.AddMinutes(5),

                 signingCredentials: creds
                );

            //5 - Retornar o token criado
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
        catch (Exception)
        {

            throw;
        }
    }
}
