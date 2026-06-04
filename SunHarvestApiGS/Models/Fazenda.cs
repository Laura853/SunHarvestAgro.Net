using System.ComponentModel.DataAnnotations;

namespace SunHarvestApiGS.Models
{
    public class Fazenda
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da fazenda é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A latitude é obrigatória")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "A longitude é obrigatória")]
        public double Longitude { get; set; }

        [Required(ErrorMessage = "A altitude é obrigatória")]
        public double Altitude { get; set; }

        [Required(ErrorMessage = "O tipo de solo é obrigatório")]
        public string TipoSolo { get; set; }

        [Required(ErrorMessage = "A área é obrigatória")]
        public double Area { get; set; }

        [Required(ErrorMessage = "O tipo de cultura é obrigatório")]
        public string TipoCultura { get; set; }

        [Required(ErrorMessage = "A eficiência de irrigação é obrigatória")]
        public double Irrigacao { get; set; }

        [Required(ErrorMessage = "A potência da bomba é obrigatória")]
        public double PotenciaBomba { get; set; }

        [Required(ErrorMessage = "A capacidade do painel solar é obrigatória")]
        public double CapacidadePainel { get; set; }


        //FK para Usuario

        [Required(ErrorMessage = "O ID do usuário é obrigatório")]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        //Uma fazenda pode ter vários alertas

        public ICollection<Alerta> Alertas { get; set; }
    }
}
