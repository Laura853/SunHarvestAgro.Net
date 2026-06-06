using System.ComponentModel.DataAnnotations;

namespace SunHarvestApiGS.Models
{
    public class AlertaRequest
    {
        [Required(ErrorMessage = "Mensagem é obrigatória.")]
        public string Mensagem { get; set; }

        [Required(ErrorMessage = "Severidade é obrigatória.")]
        public string Severidade { get; set; }
        public bool Confirmado { get; set; } = false;

        [Required(ErrorMessage = "FazendaId é obrigatório.")]
        public int FazendaId { get; set; }
    }
}

