using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Monitorar_Tarefas.Data;
using Monitorar_Tarefas.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Monitorar_Tarefas.Controllers
{
    public class AvisosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AvisosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Avisos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Avisos.ToListAsync());
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
