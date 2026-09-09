using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Usuarios.API.Data;
using Usuarios.API.DTOs;
using Usuarios.API.Models;

namespace Usuarios.API.Controllers;

[ApiController]                    // valida o modelo automaticamente e devolve 400 sozinho
[Route("api/[controller]")]        // [controller] vira "usuarios" (nome da classe sem o sufixo)
public class UsuariosController : ControllerBase
{
    private readonly UsuariosDbContext _contexto;

    // O ASP.NET entrega o contexto pronto graças ao AddDbContext do Program.cs.
    public UsuariosController(UsuariosDbContext contexto)
    {
        _contexto = contexto;
    }

    // ─────────── GET /api/usuarios ───────────
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioRespostaDto>>> Listar([FromQuery] string? cargo,[FromQuery] bool? ativo)
    {
        var consulta = _contexto.Usuarios.AsQueryable();

        if (!string.IsNullOrWhiteSpace(cargo))
            consulta = consulta.Where(u => u.Cargo == cargo);

        if (ativo.HasValue)
            consulta = consulta.Where(u => u.Ativo == ativo.Value);

        var usuarios = await consulta
            .Select(u => new UsuarioRespostaDto      // projeção entra no SQL
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Idade = u.Idade,
                Cargo = u.Cargo,
                Ativo = u.Ativo,
                DataCadastro = u.DataCadastro
            })
            .ToListAsync();

        return Ok(usuarios);                          // 200 + lista
    }

    // ─────────── GET /api/usuarios/5 ───────────
    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioRespostaDto>> BuscarPorId(int id)
    {
        var usuario = await _contexto.Usuarios.FindAsync(id);

        if (usuario is null)
            return NotFound();                        // 404

        return Ok(new UsuarioRespostaDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Idade = usuario.Idade,
            Cargo = usuario.Cargo,
            Ativo = usuario.Ativo,
            DataCadastro = usuario.DataCadastro
        });
    }

    // ─────────── POST /api/usuarios ───────────
    [HttpPost]
    public async Task<ActionResult<UsuarioRespostaDto>> Criar([FromBody] CriarUsuarioDto dto)
    {
        var jaExiste = await _contexto.Usuarios.AnyAsync(u => u.Email == dto.Email);
        if (jaExiste)
            return Conflict(new { mensagem = "Já existe um usuário com este e-mail." });

        // 1. MONTA a entidade a partir do DTO
        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Idade = dto.Idade,
            Cargo = dto.Cargo,
            Ativo = true,
            DataCadastro = DateTime.UtcNow
        };

        // 2. SALVA no banco (aqui o Id é gerado)
        _contexto.Usuarios.Add(usuario);
        await _contexto.SaveChangesAsync();  // só chega aqui se o e-mail estiver livre

        // 3. RESPONDE com o DTO de saída
        var resposta = new UsuarioRespostaDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Idade = usuario.Idade,
            Cargo = usuario.Cargo,
            Ativo = usuario.Ativo,
            DataCadastro = usuario.DataCadastro
        };

        return CreatedAtAction(nameof(BuscarPorId), new { id = usuario.Id }, resposta);
    }

    // ─────────── PUT /api/usuarios/5 ───────────
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarUsuarioDto dto)
    {
        // exclui o próprio usuário da checagem de duplicidade
        var emailEmUso = await _contexto.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id != id);
        if (emailEmUso)
            return Conflict(new { mensagem = "Já existe um usuário com este e-mail." });

        // 1. BUSCA o usuário existente
        var usuario = await _contexto.Usuarios.FindAsync(id);

        if (usuario is null)
            return NotFound();                    // 404

        // 2. ATUALIZA os campos do objeto encontrado
        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;
        usuario.Idade = dto.Idade;
        usuario.Cargo = dto.Cargo;
        usuario.Ativo = dto.Ativo;
        // DataCadastro não muda: é a data de criação.

        // 3. SALVA
        await _contexto.SaveChangesAsync();

        // 4. RESPONDE
        return NoContent();                       // 204
    }

    // ─────────── DELETE /api/usuarios/5 ───────────
    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(int id)  // sem DTO: id na URL, resposta vazia
    {
        var usuario = await _contexto.Usuarios.FindAsync(id);

        if (usuario is null)
            return NotFound();                        // 404

        _contexto.Usuarios.Remove(usuario);
        await _contexto.SaveChangesAsync();

        return NoContent();                           // 204
    }
}