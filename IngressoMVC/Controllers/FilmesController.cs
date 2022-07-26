using IngressoMVC.Data;
using IngressoMVC.Models;
using IngressoMVC.Models.ViewModels.Request;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngressoMVC.Controllers
{
    public class FilmesController : Controller
    {
        private IngressoDbContext _context;

        public FilmesController(IngressoDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Filmes);
        }
        public IActionResult Detalhes(int id)
        {
            return View(_context.Filmes.Find(id));
        }
        public IActionResult Criar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Criar(PostFilmeDTO filmeDto)
        {
               Filme filme = new Filme
                (
                    filmeDto.Titulo,
                    filmeDto.Descricao,
                    filmeDto.Preco,
                    filmeDto.ImageURL,
                    _context.Produtores.FirstOrDefault(x => x.Id == filmeDto.ProdutorId).Id
                ); 

            _context.Add(filme);
            _context.SaveChanges();

            //Incluir Relacionamentos
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult CriarFilmeCAP(PostFilmeDTO filmeDto)
        {
            Filme filme = new Filme
                (
                    filmeDto.Titulo,
                    filmeDto.Descricao,
                    filmeDto.Preco,
                    filmeDto.ImageURL,
                    _context.Produtores.FirstOrDefault(x => x.Id == filmeDto.ProdutorId).Id
                );
            _context.Add(filme);
            _context.SaveChanges();

            //Incluir Relacionamentos
            foreach (var categoria in filmeDto.Categorias)
            {
                int? categoriaId = _context.Categorias.Where(c => c.Nome == categoria).FirstOrDefault().Id;

                if (categoriaId != null)
                {
                    var novaCategoria = new FilmeCategoria(filme.Id, categoriaId.Value);
                    _context.FilmesCategorias.Add(novaCategoria);
                    _context.SaveChanges();
                }
            }

            foreach (var ator in filmeDto.NomeAtores)
            {
                int? atorId = _context.Atores.Where(a => a.Nome == ator).FirstOrDefault().Id;

                if (atorId != null)
                {
                    var novoAtor = new AtorFilme(atorId.Value, filme.Id);
                    _context.AtoresFilmes.Add(novoAtor);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Deletar(int id)
        {
            return View();
        }
        public IActionResult Atualizar(int id)
        {
            return View();
        }
    }
}
