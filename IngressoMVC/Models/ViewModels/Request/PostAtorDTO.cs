using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IngressoMVC.Models.ViewModels.Request
{
    public class PostAtorDTO
    {
        [Required(ErrorMessage ="Nome do Ator é  Obrigatório")]
        [StringLength(50,MinimumLength=3,ErrorMessage ="Nome do Ator deve ter de 3 a 50 caractéres")]
        public string Nome { get; set; }
        public string Bio { get; set; }
        [Required(ErrorMessage ="Imagem Obrigatória")]
        public string FotoPerfilURL { get; set; }
    }
}
