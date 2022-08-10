using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngressoMVC.Models.ViewModels.Request
{
    public class PostFilmeDTO
    {
    
            public string Titulo { get; set; }
            public string Descricao { get; set; }
            public decimal Preco { get; set; }
            public string ImagemURL { get; set; }
            public DateTime DataLancamento { get; set; }
            public DateTime DataEncerramento { get; set; }

            #region relacionamentos
            public int CinemaId { get; set; }

            public int ProdutorId { get; set; }

            public List<int> AtoresId { get; set; }
            public List<string> Categorias { get; set; }
            #endregion
        
    }
}
