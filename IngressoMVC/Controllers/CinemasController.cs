using IngressoMVC.Data;
using IngressoMVC.Models;
using IngressoMVC.Models.ViewModels.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngressoMVC.Controllers
{
    public class CinemasController : Controller
    {
        private IngressoDbContext _context;

        public CinemasController(IngressoDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Cinemas);
        }
        public IActionResult Detalhes(int id)
        {
            var cinema = _context.Cinemas.Include(f => f.Filmes).FirstOrDefault(c => c.Id == id);
            if (cinema == null) return View("NotFound");


            return View(cinema);
        }
        public IActionResult Criar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Criar(PostCinemaDTO cinemaDTO)
        {
            if (!ModelState.IsValid || cinemaDTO.LogoURL.EndsWith(".jpg")) return View(cinemaDTO);
            Cinema cinema = new Cinema(cinemaDTO.Nome,cinemaDTO.Descricao,cinemaDTO.LogoURL);
            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Deletar(int id)
        {
            var result = _context.Cinemas.FirstOrDefault();
            if (result == null)
            { return View("NotFound"); }
            return View();
        }
        [HttpPost,ActionName("Deletar")]
        public IActionResult ConfirmarDeletar(int id) 
        {
            var result = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
            if(result == null)
            {
                return View("NotFound");
            }
            _context.Cinemas.Remove(result);  
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Atualizar(int? id)
        {
                if (id == null)
                return NotFound();

            var result = _context.Cinemas.FirstOrDefault(a => a.Id == id);

            if (result == null)
                return View();

            
            return View(result);
        }

        [HttpPost]
        public IActionResult Atualizar(int id, PostCinemaDTO cinemaDTO)
        {
            var cinema = _context.Cinemas.FirstOrDefault(a => a.Id == id);

            if (!ModelState.IsValid)
                return View(cinema);

            cinema.AtualizarDados(cinemaDTO.Nome, cinemaDTO.Descricao, cinemaDTO.LogoURL);

            _context.Update(cinema);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
