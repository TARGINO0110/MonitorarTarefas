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
    public class HistoricoAcoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistoricoAcoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public PaginatedList<HistoricoAcoes> HistoricoAcoes { get; set; }

        // GET: HistoricoAcoes
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

            var historico = from h in _context.HistoricoAcoes
                            select h;

            if (!String.IsNullOrEmpty(searchString))
            {
                historico = historico.Where(h => h.OnservacaoAcao.Contains(searchString));
                                       
            }

            switch (sortOrder)
            {
                case "name_desc":
                    historico = historico.OrderBy(h => h.OnservacaoAcao);
                    break;
                case "Date":
                    historico = historico.OrderBy(h => h.DataHoraAcao);
                    break;
                case "date_desc":
                    historico = historico.OrderByDescending(h => h.DataHoraAcao);
                    break;
                default:
                    historico = historico.OrderByDescending(h => h.OnservacaoAcao);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<HistoricoAcoes>.CreateAsync(
                historico.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: HistoricoAcoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historicoAcoes = await _context.HistoricoAcoes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (historicoAcoes == null)
            {
                return NotFound();
            }

            return View(historicoAcoes);
        }

        // GET: HistoricoAcoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HistoricoAcoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroAcao,OnservacaoAcao,DataHoraAcao")] HistoricoAcoes historicoAcoes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(historicoAcoes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(historicoAcoes);
        }

        // GET: HistoricoAcoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historicoAcoes = await _context.HistoricoAcoes.FindAsync(id);
            if (historicoAcoes == null)
            {
                return NotFound();
            }
            return View(historicoAcoes);
        }

        // POST: HistoricoAcoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroAcao,OnservacaoAcao,DataHoraAcao")] HistoricoAcoes historicoAcoes)
        {
            if (id != historicoAcoes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historicoAcoes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoricoAcoesExists(historicoAcoes.Id))
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
            return View(historicoAcoes);
        }

        // GET: HistoricoAcoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historicoAcoes = await _context.HistoricoAcoes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (historicoAcoes == null)
            {
                return NotFound();
            }

            return View(historicoAcoes);
        }

        // POST: HistoricoAcoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var historicoAcoes = await _context.HistoricoAcoes.FindAsync(id);
            _context.HistoricoAcoes.Remove(historicoAcoes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoricoAcoesExists(int id)
        {
            return _context.HistoricoAcoes.Any(e => e.Id == id);
        }
    }
}
