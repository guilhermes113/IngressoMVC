using IngressoMVC.Data;
using IngressoMVC.Models;
using IngressoMVC.Models.ViewModels.Request;
using IngressoMVC.Models.ViewModels.Responsive;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngressoMVC.Controllers
{
    public class ProdutoresController : Controller
    {
        private IngressoDbContext _context;

        public ProdutoresController(IngressoDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Produtores);
        }
        public IActionResult Detalhes(int id)
        {
            var produtor = _context.Produtores.Find(id);
            //var result = _context.Atores.Where(at => at.Id == id)
            //.Select(at => new GetAtoresDTO()
            //{
            //Bio = at.Bio,
            //FotoPerfilURL = at.FotoPerfilURL,
            //Nome = at.Nome,
            //NomeFilmes = at.AtoresFilmes.Select(fm => fm.Filme.Titulo).ToList(),
            //FotoURLFilmes = at.AtoresFilmes.Select(fm => fm.Filme.ImageURL).ToList()
            //}).FirstOrDefault();
            var result = _context.Produtores.Where(prod => prod.Id == id)
                .Select(prod => new GetProdutoresDTO()
                {
                    Nome = prod.Nome,
                    Bio = prod.Bio,
                    FotoPerfilURL = prod.FotoPerfilURL,
                    NomeFilmes = prod.Filmes.Select(filme => filme.Titulo).ToList(),
                    FotoURLFilmes = prod.Filmes.Select(filme => filme.ImageURL).ToList()
                }).FirstOrDefault();

            return View(result);
        }
        public IActionResult Criar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Criar(PostProdutorDTO postProdutoresDTO)
        {
            if (!ModelState.IsValid) return View(postProdutoresDTO);
            Produtor produtor = new Produtor(postProdutoresDTO.Nome,postProdutoresDTO.Bio,postProdutoresDTO.FotoPerfilURL);
            _context.Produtores.Add(produtor);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Deletar(int id)
        {
            var result = _context.Produtores.FirstOrDefault(a => a.Id == id);
            if (result == null) return View();
            ;
            return View(result);
        }
        [HttpPost, ActionName("Deletar")]
        public IActionResult ConfirmarDeletar(int id)
        {
            var result = _context.Produtores.FirstOrDefault(a => a.Id == id);
            _context.Produtores.Remove(result);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Atualizar(int? id)
        {
            if (id == null)
                return NotFound();

            var result = _context.Produtores.FirstOrDefault(p => p.Id == id);

            if (result == null)
                return View();

            return View(result);
        }

        [HttpPost]
        public IActionResult Atualizar(int id, PostProdutorDTO produtorDTO)
        {
            var produtor = _context.Produtores.FirstOrDefault(p => p.Id == id);

            if (!ModelState.IsValid)
                return View(produtor);

            produtor.AtualizarDados(produtorDTO.Nome, produtorDTO.Bio, produtorDTO.FotoPerfilURL);

            _context.Update(produtor);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
