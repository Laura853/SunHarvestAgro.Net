using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SunHarvestApiGS.Data;
using SunHarvestApiGS.Models;

namespace SunHarvestApiGS.Controllers
{
    [ApiController]
    [Route("api/v1/fazendas")]
    public class FazendasController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public FazendasController(AppDbContext _dbcontext)
        {
            dbContext = _dbcontext;
        }

        /// <summary>Lista todas as fazendas cadastradas.</summary>
        /// <returns>Lista de fazendas ou 404.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fazendas = await dbContext.Fazendas
                .ToListAsync();
            if (!fazendas.Any()) return NotFound("Nenhuma fazenda cadastrada.");

            return Ok(fazendas.Select(f => new
            {
                f.Id,
                f.Nome,
                f.TipoCultura,
                f.TipoSolo,
                f.Area,
                f.Latitude,
                f.Longitude,
                f.Altitude
            }));
        }

        /// <summary>Busca uma fazenda pelo ID.</summary>
        /// <param nome="id">ID da fazenda.</param>
        /// <returns>Fazenda encontrada ou 404.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var fazenda = await dbContext.Fazendas
                .Include(f => f.Usuario)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fazenda == null) return NotFound("Fazenda não encontrada.");

            return Ok(new
            {
                fazenda.Id,
                fazenda.Nome,
                fazenda.Latitude,
                fazenda.Longitude,
                fazenda.Altitude,
                fazenda.TipoCultura,
                fazenda.TipoSolo,
                fazenda.Area,
                fazenda.Irrigacao,
                fazenda.CapacidadePainel,
                fazenda.PotenciaBomba,
                fazenda.AzimutGraus,
                fazenda.DataCadastro,
                fazenda.IdDispositivoIot,
                fazenda.TaxaDesempenho,
                fazenda.DataAtualizacao,
                usuario = new
                {
                    fazenda.Usuario.Id,
                    fazenda.Usuario.Nome,
                    fazenda.Usuario.Email
                }
            });
        }

        /// <summary>Lista fazendas de um usuário.</summary>
        /// <param nome="usuarioId">ID do usuário.</param>
        /// <returns>Lista de fazendas do usuário ou 404.</returns>
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var usuario = await dbContext.Usuarios.FindAsync(usuarioId);
            if (usuario == null) return NotFound("Usuário não encontrado.");

            var fazendas = await dbContext.Fazendas
                .Where(f => f.UsuarioId == usuarioId)
                .ToListAsync();

            if (!fazendas.Any()) return NotFound("Nenhuma fazenda encontrada para este usuário.");

            return Ok(fazendas.Select(f => new
            {
                f.Id,
                f.Nome,
                f.TipoCultura,
                f.TipoSolo,
                f.Area,
                f.Irrigacao,
                f.CapacidadePainel,
                f.PotenciaBomba,
                f.Latitude,
                f.Longitude,
                f.Altitude,
                f.AzimutGraus,
                f.DataCadastro,
                f.IdDispositivoIot,
                f.TaxaDesempenho,
                f.DataAtualizacao

            }));
        }


        /// <summary>Cadastra uma nova fazenda.</summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/v1/fazendas
        ///     {
        ///         "nome": "Fazenda São João",
        ///         "latitude": -10.5,
        ///         "longitude": -45.2,
        ///         "altitude": 500,
        ///         "tipoCultura": "Soja",
        ///         "tipoSolo": "Franco",
        ///         "area": 5,
        ///         "irrigacao": 85,
        ///         "capacidadePainel": 3000,
        ///         "potenciaBomba": 1100,
        ///         "azimutGraus": 45,
        ///         "taxaDesempenho": 85,
        ///         "idDispositivoIot": "device_123",
        ///         "usuarioId": 1,
        ///         
        ///     }
        /// </remarks>
        /// <returns>Fazenda criada com status 201.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FazendaRequest request)
        {
            if (string.IsNullOrEmpty(request.Nome))
                return BadRequest("Nome da fazenda é obrigatório.");
            if (string.IsNullOrEmpty(request.TipoCultura))
                return BadRequest("Tipo de cultura é obrigatório.");
            if (string.IsNullOrEmpty(request.TipoSolo))
                return BadRequest("Tipo de solo é obrigatório.");

            // Valida se o usuário existe
            var usuario = await dbContext.Usuarios.FindAsync(request.UsuarioId);
            if (usuario == null)
                return NotFound($"Usuário com ID {request.UsuarioId} não encontrado.");

            var fazendaC = new Fazenda
            {
                Nome = request.Nome,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Altitude = request.Altitude,
                TipoCultura = request.TipoCultura,
                TipoSolo = request.TipoSolo,
                Area = request.Area,
                Irrigacao = request.Irrigacao,
                CapacidadePainel = request.CapacidadePainel,
                PotenciaBomba = request.PotenciaBomba,
                AzimutGraus = request.AzimutGraus,
                DataCadastro = DateTime.UtcNow,
                TaxaDesempenho = request.TaxaDesempenho,
                IdDispositivoIot = request.IdDispositivoIot,
                UsuarioId = request.UsuarioId
            };

            dbContext.Fazendas.Add(fazendaC);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = fazendaC.Id }, new
            {
                fazendaC.Id,
                fazendaC.Nome,
                fazendaC.TipoCultura,
                fazendaC.TipoSolo,
                fazendaC.Area,
                fazendaC.Latitude,
                fazendaC.Longitude,
                fazendaC.Altitude,
                fazendaC.Irrigacao,
                fazendaC.CapacidadePainel,
                fazendaC.PotenciaBomba,
                fazendaC.AzimutGraus,
                fazendaC.DataCadastro,
                fazendaC.TaxaDesempenho,
                fazendaC.IdDispositivoIot,
                fazendaC.UsuarioId
            });
        }

        /// <summary>Atualiza os dados de uma fazenda.</summary>
        /// <param nome="id">ID da fazenda a ser atualizada.</param>
        /// <returns>204 em caso de sucesso ou 404 se não encontrado.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FazendaRequest request)
        {
            var fazendaC = await dbContext.Fazendas.FindAsync(id);
            if (fazendaC == null) return NotFound("Fazenda não encontrada.");

            var usuario = await dbContext.Usuarios.FindAsync(request.UsuarioId);
            if (usuario == null)
                return NotFound($"Usuário com ID {request.UsuarioId} não encontrado.");

            fazendaC.Nome = request.Nome;
            fazendaC.Latitude = request.Latitude;
            fazendaC.Longitude = request.Longitude;
            fazendaC.Altitude = request.Altitude;
            fazendaC.TipoCultura = request.TipoCultura;
            fazendaC.TipoSolo = request.TipoSolo;
            fazendaC.Area = request.Area;
            fazendaC.Irrigacao = request.Irrigacao;
            fazendaC.CapacidadePainel = request.CapacidadePainel;
            fazendaC.PotenciaBomba = request.PotenciaBomba;
            fazendaC.DataAtualizacao = DateTime.UtcNow;
            fazendaC.TaxaDesempenho = request.TaxaDesempenho;
            fazendaC.IdDispositivoIot = request.IdDispositivoIot;
            fazendaC.AzimutGraus = request.AzimutGraus;
            fazendaC.UsuarioId = request.UsuarioId;

            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>Remove uma fazenda pelo ID.</summary>
        /// <param nome="id">ID da fazenda a ser removida.</param>
        /// <returns>204 em caso de sucesso ou 404 se não encontrado.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var fazendaC = await dbContext.Fazendas.FindAsync(id);
            if (fazendaC == null) return NotFound("Fazenda não encontrada.");

            dbContext.Fazendas.Remove(fazendaC);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
