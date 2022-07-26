using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IngressoMVC.Models.ViewModels.Request
{
    public class PostCategoriaDTO
    {
        [Required(ErrorMessage = "Categoria Obrigatória")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Nome do Categoria deve ter de 3 a 50 caractéres")]
        public string Nome { get; set; }
    }
}
