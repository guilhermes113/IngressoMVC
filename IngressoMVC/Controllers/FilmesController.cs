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
        
        [HttpPost]IActionResult CriarFilmecomCAtegoriasEAtores(PostFilmeDTO filmeDto)
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

            foreach (var atorId in filmeDto.AtoresId)
            {
                    var novoAtor = new AtorFilme(atorId, filme.Id);
                    _context.AtoresFilmes.Add(novoAtor);
                    _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
            
               
        public IActionResult Deletar(int id)
        {
            var result = _context.Filmes.FirstOrDefault(x => x.Id == id);
            if (result == null) return View("NotFound");
            return View(result);
        }
        [HttpPost, ActionName("Deletar")]
        public IActionResult ConfirmaDeletar(int id) 
        {
            var result = _context.Filmes.FirstOrDefault(x => x.Id == id);
            if (result == null) return View("NotFound");
            _context.Remove(result);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Atualizar(int id)
        {
            var result = _context.Filmes.FirstOrDefault(x => x.Id == id);
            if (result == null) return View("NotFound");
            return View(result);
        }
        [HttpPost]
        public IActionResult Atualizar(int id, PostFilmeDTO filmeDTO)
        {
            var result = _context.Filmes.FirstOrDefault(x => x.Id == id);
            if (!ModelState.IsValid) return View(result);
            result.AlteraDados(filmeDTO.Titulo,filmeDTO.Descricao,filmeDTO.Preco,filmeDTO.ImageURL) ;
            _context.Update(result);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
