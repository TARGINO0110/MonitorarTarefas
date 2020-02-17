using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Monitorar_Tarefas.Data;
using Monitorar_Tarefas.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Monitorar_Tarefas.Controllers
{
    ////[Authorize]
    public class AvisosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AvisosController(ApplicationDbContext context)
        {
            _context = context;
        }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Avisos> Avisos { get; set; }

        // GET: Avisos
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

            var avisos = from a in _context.Avisos
                         select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                avisos = avisos.Where(a => a.TituloAviso.Contains(searchString)
                                       || a.DescricaoAviso.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    avisos = avisos.OrderBy(a => a.TituloAviso);
                    break;
                case "Date":
                    avisos = avisos.OrderBy(a => a.DataPostagemAviso);
                    break;
                case "date_desc":
                    avisos = avisos.OrderByDescending(a => a.DataExpiracaoAviso);
                    break;
                default:
                    avisos = avisos.OrderByDescending(a => a.DataPostagemAviso);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Avisos>.CreateAsync(
                avisos.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Avisos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avisos = await _context.Avisos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (avisos == null)
            {
                return NotFound();
            }

            return View(avisos);
        }

        // GET: Avisos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Avisos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TituloAviso,DescricaoAviso,DataPostagemAviso,DataExpiracaoAviso")] Avisos avisos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(avisos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(avisos);
        }

        // GET: Avisos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avisos = await _context.Avisos.FindAsync(id);
            if (avisos == null)
            {
                return NotFound();
            }
            return View(avisos);
        }

        // POST: Avisos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TituloAviso,DescricaoAviso,DataPostagemAviso,DataExpiracaoAviso")] Avisos avisos)
        {
            if (id != avisos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(avisos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AvisosExists(avisos.Id))
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
            return View(avisos);
        }

        // GET: Avisos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avisos = await _context.Avisos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (avisos == null)
            {
                return NotFound();
            }

            return View(avisos);
        }

        // POST: Avisos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var avisos = await _context.Avisos.FindAsync(id);
            _context.Avisos.Remove(avisos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AvisosExists(int id)
        {
            return _context.Avisos.Any(e => e.Id == id);
        }
    }
}
