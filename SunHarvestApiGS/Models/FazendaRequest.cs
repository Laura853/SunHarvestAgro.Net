using System.ComponentModel.DataAnnotations;

namespace SunHarvestApiGS.Models
{
    public class FazendaRequest
    {
        [Required(ErrorMessage = "Nome da fazenda é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Latitude é obrigatória.")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitude é obrigatória.")]
        public double Longitude { get; set; }

        [Range(0, 5000, ErrorMessage = "Altitude deve ser entre 0 e 5000 metros.")]
        public double Altitude { get; set; }

        [Required(ErrorMessage = "Tipo de cultura é obrigatório.")]
        public string TipoCultura { get; set; }

        [Required(ErrorMessage = "Tipo de solo é obrigatório.")]
        public string TipoSolo { get; set; }

        [Range(0.1, 10000, ErrorMessage = "Área deve ser entre 0.1 e 10000 hectares.")]
        public double Area { get; set; }

        [Range(0, 100, ErrorMessage = "Eficiência de irrigação deve ser entre 0 e 100%.")]
        public double Irrigacao { get; set; }

        [Range(0, 100000, ErrorMessage = "Capacidade do painel deve ser entre 0 e 100000 watts.")]
        public double CapacidadePainel { get; set; }

        [Range(0, 50000, ErrorMessage = "Potência da bomba deve ser entre 0 e 50000 watts.")]
        public double PotenciaBomba { get; set; }
        public float? AzimutGraus { get; set; }
        public float? TaxaDesempenho { get; set; }
        public string? IdDispositivoIot { get; set; }

        [Required(ErrorMessage = "UsuarioId é obrigatório.")]
        public int UsuarioId { get; set; }
    }
}
