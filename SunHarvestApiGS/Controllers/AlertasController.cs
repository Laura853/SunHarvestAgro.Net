using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunHarvestApiGS.Data;
using SunHarvestApiGS.Models;

namespace SunHarvestApiGS.Controllers
{
    [ApiController]
    [Route("api/v1/alertas")]
    public class AlertasController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public AlertasController(AppDbContext _dbcontext)
        {
            dbContext = _dbcontext;
        }

        /// <summary>Busca um alerta pelo ID.</summary>
        /// <param nome="id">ID do alerta.</param>
        /// <returns>Alerta encontrado ou 404.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var alerta = await dbContext.Alertas
                .Include(a => a.Fazenda)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (alerta == null) return NotFound("Alerta não encontrado.");

            return Ok(new
            {
                alerta.Id,
                alerta.Mensagem,
                alerta.Severidade,
                alerta.Confirmado,
                alerta.DataCriacao,
                fazenda = new
                {
                    alerta.Fazenda.Id,
                    alerta.Fazenda.Nome
                }
            });
        }

        /// <summary>Lista alertas de uma fazenda.</summary>
        /// <param nome="fazendaId">ID da fazenda.</param>
        /// <returns>Lista de alertas da fazenda ou 404.</returns>
        [HttpGet("fazenda/{fazendaId}")]
        public async Task<IActionResult> GetByFazenda(int fazendaId, [FromQuery] string? severidade)
        {
            var fazenda = await dbContext.Fazendas.FindAsync(fazendaId);
            if (fazenda == null) return NotFound("Fazenda não encontrada.");

            var query = dbContext.Alertas.Where(a => a.FazendaId == fazendaId);

            if (!string.IsNullOrEmpty(severidade))
                query = query.Where(a => a.Severidade.ToLower() == severidade.ToLower());

            var alertas = await query.ToListAsync();

            if (!alertas.Any()) return NotFound("Nenhum alerta encontrado para esta fazenda.");

            return Ok(alertas.Select(a => new
            {
                a.Id,
                a.Mensagem,
                a.Severidade,
                a.Confirmado,
                a.DataCriacao
            }));
        }

        /// <summary>Cadastra um novo alerta.</summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/v1/alertas
        ///     {
        ///         "mensagem": "Bomba operando a 40% da vazão esperada",
        ///         "severidade": "alto",
        ///         "confirmado": false,
        ///         "fazendaId": 1
        ///     }
        ///
        /// severidade: baixo, medio, alto, crítico
        /// </remarks>
        /// <returns>Alerta criado com status 201.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AlertaRequest request)
        {
            if (string.IsNullOrEmpty(request.Mensagem))
                return BadRequest("Mensagem é obrigatória.");
            if (string.IsNullOrEmpty(request.Severidade))
                return BadRequest("Severidade é obrigatória.");

            var severidadesValidas = new[] { "baixo", "medio", "alto", "crítico" };
            if (!severidadesValidas.Contains(request.Severidade.ToLower()))
                return BadRequest("Severidade inválida. Use: baixo, medio, alto ou crítico.");

            var fazenda = await dbContext.Fazendas.FindAsync(request.FazendaId);
            if (fazenda == null)
                return NotFound($"Fazenda com ID {request.FazendaId} não encontrada.");

            var alerta = new Alerta
            {
                Mensagem = request.Mensagem,
                Severidade = request.Severidade.ToLower(),
                DataCriacao = DateTime.UtcNow,
                Confirmado = request.Confirmado,
                FazendaId = request.FazendaId
            };

            dbContext.Alertas.Add(alerta);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = alerta.Id }, new
            {
                alerta.Id,
                alerta.Mensagem,
                alerta.Severidade,
                alerta.DataCriacao,
                alerta.Confirmado,
                alerta.FazendaId
            });
        }

        /// <summary>Atualiza um alerta.</summary>
        /// <param nome="id">ID do alerta a ser atualizado.</param>
        /// <returns>204 em caso de sucesso ou 404 se não encontrado.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AlertaRequest request)
        {
            var alertaC = await dbContext.Alertas.FindAsync(id);
            if (alertaC == null) return NotFound("Alerta não encontrado.");

            var severidadesValidas = new[] { "baixo", "medio", "alto", "critico" };
            if (!severidadesValidas.Contains(request.Severidade.ToLower()))
                return BadRequest("Severidade inválida. Use: baixo, medio, alto ou critico.");

            var fazendas = await dbContext.Fazendas.FindAsync(request.FazendaId);
            if (fazendas == null)
                return NotFound($"Fazenda com ID {request.FazendaId} não encontrada.");

            alertaC.Mensagem = request.Mensagem;
            alertaC.Severidade = request.Severidade.ToLower();
            alertaC.Confirmado = request.Confirmado;
            alertaC.FazendaId = request.FazendaId;

            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>Remove um alerta pelo ID.</summary>
        /// <param nome="id">ID do alerta a ser removido.</param>
        /// <returns>204 em caso de sucesso ou 404 se não encontrado.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var alerta = await dbContext.Alertas.FindAsync(id);
            if (alerta == null) return NotFound("Alerta não encontrado.");

            dbContext.Alertas.Remove(alerta);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
