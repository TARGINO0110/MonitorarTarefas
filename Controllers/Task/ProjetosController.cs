using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Monitorar_Tarefas.Data;
using Monitorar_Tarefas.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Monitorar_Tarefas.Controllers
{

    public class ProjetosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjetosController(ApplicationDbContext context)
        {
            _context = context;
        }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Projetos> Tarefas { get; set; }
        // GET: Projetos
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var projetos = from p in _context.Projetos
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                projetos = projetos.Where(p => p.NomeProjeto.Contains(searchString)
                                        || p.DescricaoProjeto.Contains(searchString)
                                        || p.Categoria.NomeCategoria.Contains(searchString));
            }


            switch (sortOrder)
            {
                case "name_desc":
                    projetos = projetos.OrderBy(p => p.NomeProjeto);
                    break;
                case "Date":
                    projetos = projetos.OrderBy(p => p.DataInicioProjeto);
                    break;
                case "date_desc":
                    projetos = projetos.OrderByDescending(p => p.DataFinalizadoProjeto);
                    break;
                default:
                    projetos = projetos.OrderByDescending(p => p.DataInicioProjeto);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Projetos>.CreateAsync(
                projetos.AsNoTracking().Include(p => p.Categoria), pageNumber ?? 1, pageSize));
        }


        // GET: Projetos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projetos = await _context.Projetos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projetos == null)
            {
                return NotFound();
            }

            return View(projetos);
        }

        // GET: Projetos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "NomeCategoria");
            return View();
        }

        // POST: Projetos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeProjeto,DescricaoProjeto,DataInicioProjeto,DataFinalizadoProjeto,DataEntregaProjeto,CategoriaId")] Projetos projetos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projetos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "NomeCategoria", projetos.CategoriaId);
            return View(projetos);
        }

        // GET: Projetos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projetos = await _context.Projetos.FindAsync(id);
            if (projetos == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "NomeCategoria", projetos.CategoriaId);
            return View(projetos);
        }

        // POST: Projetos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeProjeto,DescricaoProjeto,DataInicioProjeto,DataFinalizadoProjeto,DataEntregaProjeto,CategoriaId")] Projetos projetos)
        {
            if (id != projetos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projetos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjetosExists(projetos.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "NomeCategoria", projetos.CategoriaId);
            return View(projetos);
        }

        // GET: Projetos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projetos = await _context.Projetos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projetos == null)
            {
                return NotFound();
            }

            return View(projetos);
        }

        // POST: Projetos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projetos = await _context.Projetos.FindAsync(id);
            _context.Projetos.Remove(projetos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjetosExists(int id)
        {
            return _context.Projetos.Any(e => e.Id == id);
        }
    }
}
