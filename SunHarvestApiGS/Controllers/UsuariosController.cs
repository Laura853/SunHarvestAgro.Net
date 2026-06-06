using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunHarvestApiGS.Data;
using SunHarvestApiGS.Models;

namespace SunHarvestApiGS.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios")]
    public class UsuariosController : ControllerBase
    {

        private readonly AppDbContext dbContext;
        public UsuariosController(AppDbContext _dbcontext)
        {
            dbContext = _dbcontext;
        }

        /// <summary>Lista todos os usuários cadastrados.</summary>
        /// <returns>Lista de usuários ou 404.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await dbContext.Usuarios.ToListAsync();
            if (!usuarios.Any()) return NotFound("Nenhum usuário cadastrado");
            return Ok(usuarios.Select(u => new
            {
                u.Id,
                u.Nome,
                u.Email
            }));
        }

        /// <summary>Busca um usuário pelo ID.</summary>
        /// <param nome="id">ID do usuário.</param>
        /// <returns>Usuário encontrado ou 404.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await dbContext.Usuarios
                .Include(u => u.Fazendas)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null) return NotFound("Usuário não encontrado.");

            return Ok(new
            {
                usuario.Id,
                usuario.Nome,
                usuario.Email,
                fazendas = usuario.Fazendas?.Select(f => new
                {
                    f.Id,
                    f.Nome,
                    f.TipoCultura,
                    f.TipoSolo
                })
            });
        }

        /// <summary>Busca um usuário pelo email.</summary>
        /// <param nome="email">Email do usuário.</param>
        /// <returns>Usuário encontrado ou 404.</returns>
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await dbContext.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) return NotFound("Usuário não encontrado.");

            return Ok(new
            {
                user.Id,
                user.Nome,
                user.Email
            });
        }

        /// <summary>Cadastra um novo usuário.</summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/v1/usuarios
        ///     {
        ///         "nome": "João Silva",
        ///         "email": "joao@email.com",
        ///         "senha": "senha123"
        ///     }
        /// </remarks>
        /// <returns>Usuário criado com status 201.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioRequest request)
        {
            if (string.IsNullOrEmpty(request.Nome))
                return BadRequest("Nome é obrigatório.");
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest("Email é obrigatório.");
            if (string.IsNullOrEmpty(request.Senha))
                return BadRequest("Senha é obrigatória.");

            // Verifica se o email já está cadastrado
            var emailExiste = await dbContext.Usuarios.AnyAsync(u => u.Email == request.Email);
            if (emailExiste)
                return BadRequest("Email já cadastrado.");

            var usuarioC = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                Senha = request.Senha
            };

            dbContext.Usuarios.Add(usuarioC);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = usuarioC.Id }, new
            {
                usuarioC.Id,
                usuarioC.Nome,
                usuarioC.Email
            });
        }

        /// <summary>Atualiza os dados de um usuário.</summary>
        /// <param nome="id">ID do usuário a ser atualizado.</param>
        /// <returns>204 em caso de sucesso ou 404 se não encontrado.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioRequest request)
        {
            var usuarioC = await dbContext.Usuarios.FindAsync(id);
            if (usuarioC == null) return NotFound("Usuário não encontrado.");

            // Verifica se o novo email já pertence a outro usuário
            var emailExiste = await dbContext.Usuarios
                .AnyAsync(u => u.Email == request.Email && u.Id != id);
            if (emailExiste)
                return BadRequest("Email já cadastrado por outro usuário.");

            usuarioC.Nome = request.Nome;
            usuarioC.Email = request.Email;
            usuarioC.Senha = request.Senha;

            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>Remove um usuário pelo ID.</summary>
        /// <param nome="id">ID do usuário a ser removido.</param>
        /// <returns>204 em caso de sucesso ou 404 se não encontrado.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioC = await dbContext.Usuarios.FindAsync(id);
            if (usuarioC == null) return NotFound("Usuário não encontrado.");

            dbContext.Usuarios.Remove(usuarioC);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
