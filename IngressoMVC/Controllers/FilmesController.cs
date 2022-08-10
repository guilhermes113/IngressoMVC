using IngressoMVC.Data;
using IngressoMVC.Models;
using IngressoMVC.Models.ViewModels.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var dadosDropdown = DadosDropDow();
            ViewBag.Atores = new SelectList(dadosDropdown.Atores, "Id", "Nome");
            ViewBag.Categorias = new SelectList(dadosDropdown.Categorias, "Id", "Nome");
            ViewBag.Cinemas = new SelectList(dadosDropdown.Cinemas, "Id", "Nome");
            ViewBag.Produtores = new SelectList(dadosDropdown.Produtores, "Id", "Nome");



            return View();
        }
        public PostFilmeDropDownDTO DadosDropDow()
        {
            var resp = new PostFilmeDropDownDTO()
            {
                Atores = _context.Atores.OrderBy(x => x.Nome).ToList(),
                Cinemas = _context.Cinemas.OrderBy(x => x.Nome).ToList(),
                Produtores = _context.Produtores.OrderBy(x => x.Nome).ToList(),
                Categorias = _context.Categorias.OrderBy(x => x.Nome).ToList()
            };
            return resp;
        }
        [HttpPost]IActionResult CriarFilmecomCAtegoriasEAtores(PostFilmeDTO filmeDto)
        {
            Filme filme = new Filme
                (
                    filmeDto.Titulo,
                    filmeDto.Descricao,
                    filmeDto.Preco,
                    filmeDto.ImagemURL,
                    filmeDto.ProdutorId,
                    filmeDto.CinemaId,
                    filmeDto.DataLancamento,
                    filmeDto.DataEncerramento
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
            result.AlteraDados(filmeDTO.Titulo,filmeDTO.Descricao,filmeDTO.Preco,filmeDTO.ImagemURL,filmeDTO.DataLancamento,filmeDTO.DataEncerramento) ;
            _context.Update(result);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Teste()
        {
            ViewData["TesteData"] = "Olá,View Data";
            ViewBag.OutroTeste = "Olá,View Data";

            return View();
        }
    }
}
