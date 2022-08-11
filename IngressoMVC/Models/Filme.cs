using IngressoMVC.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace IngressoMVC.Models
{
    public class Filme : IEntidade
    {
        protected Filme() { }
        public Filme(string titulo, string descricao, decimal preco, string imageURL, int produtorId,
                     int cinemaId, DateTime lancamento, DateTime encerramento)
        {
            Titulo = titulo;
            Descricao = descricao;
            Preco = preco;
            ImagemURL = imageURL;
            ProdutorId = produtorId;
            CinemaId = cinemaId;
            DataLancamento = lancamento;
            DataEncerramento = encerramento;

            DataCadastro = DateTime.Now;
            DataAlteracao = DataCadastro;
        }
        public int Id { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataAlteracao { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public decimal Preco { get; private set; }
        public string ImagemURL { get; private set; }
        public DateTime DataLancamento { get; private set; }
        public DateTime DataEncerramento { get; private set; }

        #region relacionamentos
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }

        public int ProdutorId { get; set; }
        public Produtor produtor { get; set; }

        public List<AtorFilme> AtoresFilmes { get; set; }
        public List<FilmeCategoria> FilmesCategorias { get; set; }
        #endregion


        public void AlteraDados(string titulo,string descricao, decimal novoPreco, string imagemURL,DateTime lancamento,DateTime encerramento)
        {
            if (titulo.Length < 3 || novoPreco < 0)
            {
                return;
            }
            Titulo = titulo;
            Descricao = descricao;
            Preco = novoPreco;
            ImagemURL = imagemURL;
            DataAlteracao = DateTime.Now;
            DataLancamento = lancamento;
            DataEncerramento = encerramento;
        }
    }
}
