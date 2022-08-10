using IngressoMVC.Data;
using IngressoMVC.Models;
using IngressoMVC.Models.ViewModels.Request;
using IngressoMVC.Models.ViewModels.Responsive;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngressoMVC.Controllers
{
    public class AtoresController : Controller
    {
        private IngressoDbContext _context;

        public AtoresController(IngressoDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Atores);
        }

        public IActionResult Detalhes(int id)
        {
            #region Método 1
            var resultado = _context.Atores
                .Include(at => at.AtoresFilmes)
                .ThenInclude(f => f.Filme)
                .FirstOrDefault(ator => ator.Id == id);
            if (resultado == null)
             View();
              GetAtoresDTO atorDTO = new GetAtoresDTO()
            { 
            Nome = resultado.Nome,
            Bio = resultado.Bio,
            FotoPerfilURL = resultado.FotoPerfilURL,
            FotoURLFilmes = resultado.AtoresFilmes.Select(af => af.Filme.ImagemURL).ToList(),
            NomeFilmes = resultado.AtoresFilmes.Select(af => af.Filme.Titulo).ToList()
            };
            return View(resultado
                );
            #endregion
            #region Método 2
            //var result = _context.Atores.Where(ator => ator.Id == id).Select(at => new GetAtoresDTO()
            //{
            //Bio = at.Bio,
            //FotoPerfilURL = at.FotoPerfilURL,
            //Nome = at.Nome,
            //NomeFilmes = at.AtoresFilmes.Select(fm => fm.Filme.Titulo).ToList(),
            //FotoURLFilmes = at.AtoresFilmes.Select(fm => fm.Filme.ImagemURL).ToList()
            //}).FirstOrDefault();

            #endregion
        }


        public IActionResult Criar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Criar([Bind("Nome,Bio,FotoPerfilURL")]PostAtorDTO atorDTO )
        {
            if (!ModelState.IsValid) return View(atorDTO);
            Ator ator = new Ator(atorDTO.Nome,atorDTO.Bio,atorDTO.FotoPerfilURL);
            _context.Atores.Add(ator);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Deletar(int id)
        {
            var result = _context.Atores.FirstOrDefault(a => a.Id == id);
            if (result == null) return View();
            
            return View(result);
        }
        [HttpPost, ActionName("Deletar")]
        public IActionResult ConfirmarDeletar(int id)
        {
            var result = _context.Atores.FirstOrDefault(a => a.Id == id);
            _context.Atores.Remove(result);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            
        }
        public IActionResult Atualizar(int? id)
        {
            if (id == null)
                return NotFound();

            var result = _context.Atores.FirstOrDefault(p => p.Id == id);

            if (result == null)
                return View("NotFound");

            return View(result);
        }

        [HttpPost]
        public IActionResult Atualizar(int id, PostAtorDTO atorDTO)
        {
            var ator = _context.Atores.FirstOrDefault(p => p.Id == id);

            if (!ModelState.IsValid)
                return View();

            ator.AtualizarDados(atorDTO.Nome, atorDTO.Bio, atorDTO.FotoPerfilURL);

            _context.Update(ator);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
