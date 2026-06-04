using System.ComponentModel.DataAnnotations;

namespace SunHarvestApiGS.Models
{
    public class Alerta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Mensagem é obrigatória")]
        public string Mensagem { get; set; }

        [Required(ErrorMessage = "Severidade é obrigatória.")]
        public string Severidade { get; set; }

        [Required(ErrorMessage = "A data do alerta é obrigatória.")]
        public DateTime Data { get; set; } = DateTime.Now;

        //FK para Fazenda

        [Required(ErrorMessage = "O ID da fazenda é obrigatório.")]
        public int FazendaId { get; set; }
        public Fazenda? Fazenda { get; set; }
    }
}
