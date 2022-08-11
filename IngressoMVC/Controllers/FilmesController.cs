using IngressoMVC.Data;
using IngressoMVC.Models;
using IngressoMVC.Models.ViewModels.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            var result = _context.Filmes
                .Include(p => p.produtor)
                .Include(c => c.Cinema)
                .Include(fc => fc.FilmesCategorias).ThenInclude(c => c.Categoria)
                .Include(af => af.AtoresFilmes).ThenInclude(a => a.Ator)
                .FirstOrDefault(f => f.Id == id);

            return View(result);
        }
        public IActionResult Criar()
        {
            DadosDropDow();


            return View();
        }
        public void DadosDropDow()
        {
            var resp = new PostFilmeDropDownDTO()
            {
                Atores = _context.Atores.OrderBy(x => x.Nome).ToList(),
                Cinemas = _context.Cinemas.OrderBy(x => x.Nome).ToList(),
                Produtores = _context.Produtores.OrderBy(x => x.Nome).ToList(),
                Categorias = _context.Categorias.OrderBy(x => x.Nome).ToList()
            };

            ViewBag.Atores = new SelectList(resp.Atores, "Id", "Nome");
            ViewBag.Categorias = new SelectList(resp.Categorias, "Id", "Nome");
            ViewBag.Cinemas = new SelectList(resp.Cinemas, "Id", "Nome");
            ViewBag.Produtores = new SelectList(resp.Produtores, "Id", "Nome");

        }
        [HttpPost]IActionResult CriarFilmecomCAtegoriasEAtores(PostFilmeDTO filmeDto)
        {
            if (!ModelState.IsValid) 
            {
                DadosDropDow();
                return View();
            }
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
            foreach (var categoria in filmeDto.CategoriasId)
            {
                int? categoriaId = _context.Categorias.Where(c => c.Id == categoria).FirstOrDefault().Id;

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
            var result = _context.Filmes.Include(x => x.AtoresFilmes).ThenInclude(x => x.Ator)
                                        .Include(x => x.FilmesCategorias).ThenInclude(x => x.Categoria)
                                        .FirstOrDefault(x => x.Id == id);
            if (result == null) return View("NotFound");
            var resp = new PostFilmeDTO()
            {
                Titulo = result.Titulo,
                Descricao = result.Descricao,
                Preco = result.Preco,
                ImagemURL = result.ImagemURL,
                DataLancamento = result.DataLancamento,
                DataEncerramento = result.DataEncerramento,
                CinemaId = result.CinemaId,
                ProdutorId = result.ProdutorId,
                AtoresId = result.AtoresFilmes.Select(x => x.AtorId).ToList(),
                CategoriasId = result.FilmesCategorias.Select(x => x.CategoriaId).ToList()
            };
            DadosDropDow();
            return View(resp);
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
        public IActionResult Buscar(string filtroBusca)
        {
            var resultado = _context.Filmes.ToList();

            if (!string.IsNullOrEmpty(filtroBusca))
            {
                filtroBusca = filtroBusca.ToLower();
                var resultdaoBusca = resultado.Where(x => x.Titulo.ToLower().Contains(filtroBusca) ||
                                                          x.Descricao.ToLower().Contains(filtroBusca)).ToList();
                return View(nameof(Index), resultdaoBusca);
            };
            return View(nameof(Index), resultado);
            
        }
    }
}
