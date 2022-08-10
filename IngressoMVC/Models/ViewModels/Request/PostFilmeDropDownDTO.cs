using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngressoMVC.Models.ViewModels.Request
{
    public class PostFilmeDropDownDTO
    {
        public PostFilmeDropDownDTO()
        {
            Cinemas = new List<Cinema>();
            Produtores = new List<Produtor>();
            Atores = new List<Ator>();
            Categorias = new List<Categoria>();
        }
        public List<Cinema> Cinemas { get; set; }
        public List<Produtor> Produtores { get; set; }
        public List<Ator> Atores { get; set;  }
        public List <Categoria> Categorias { get; set;  }

    }
}
