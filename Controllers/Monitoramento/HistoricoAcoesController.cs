using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Monitorar_Tarefas.Data;
using Monitorar_Tarefas.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Monitorar_Tarefas.Controllers
{
    public class HistoricoAcoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistoricoAcoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HistoricoAcoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.HistoricoAcoes.ToListAsync());
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
