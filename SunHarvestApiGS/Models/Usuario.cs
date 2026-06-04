using System.ComponentModel.DataAnnotations;

namespace SunHarvestApiGS.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatóri.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Senha { get; set; }

        //Um usuário pode ter mais de uma fazenda

        public ICollection<Fazenda> Fazendas { get; set; }


    }
}
