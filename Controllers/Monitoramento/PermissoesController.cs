using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Monitorar_Tarefas.Data;
using Monitorar_Tarefas.Models.Monitoramento;

namespace Monitorar_Tarefas.Controllers.Monitoramento
{
    public class PermissoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PermissoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Permissoes> Permissoes { get; set; }

        // GET: Permissoes
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var permissoes = from p in _context.Permissoes
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                permissoes = permissoes.Where(p => p.Perfil.PerfilUsuario.Contains(searchString));
            }

            permissoes = sortOrder switch
            {
                "name_desc" => permissoes.OrderBy(p => p.Perfil.PerfilUsuario),
                _ => permissoes.OrderByDescending(p => p.Id),
            };
            int pageSize = 4;
            return View(await PaginatedList<Permissoes>.CreateAsync(
                permissoes.AsNoTracking().Include(p => p.Perfil), pageNumber ?? 1, pageSize));
        }

        // GET: Permissoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permissoes = await _context.Permissoes
                .Include(p => p.Perfil)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (permissoes == null)
            {
                return NotFound();
            }

            return View(permissoes);
        }

        // GET: Permissoes/Create
        public IActionResult Create()
        {
            ViewData["PerfilId"] = new SelectList(_context.Perfils, "Id", "PerfilUsuario");
            return View();
        }

        // POST: Permissoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PermEmpresa,PermPerfil,PermUsuarios,PermHistoricoAcoes,PermPermissoes,PermToken,PermAvisos,PermCategoria,PermProjetos,PermTarefas,PerfilId")] Permissoes permissoes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permissoes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PerfilId"] = new SelectList(_context.Perfils, "Id", "PerfilUsuario", permissoes.PerfilId);
            return View(permissoes);
        }

        // GET: Permissoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permissoes = await _context.Permissoes.FindAsync(id);
            if (permissoes == null)
            {
                return NotFound();
            }
            ViewData["PerfilId"] = new SelectList(_context.Perfils, "Id", "PerfilUsuario", permissoes.PerfilId);
            return View(permissoes);
        }

        // POST: Permissoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PermEmpresa,PermPerfil,PermUsuarios,PermHistoricoAcoes,PermPermissoes,PermToken,PermAvisos,PermCategoria,PermProjetos,PermTarefas,PerfilId")] Permissoes permissoes)
        {
            if (id != permissoes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permissoes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermissoesExists(permissoes.Id))
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
            ViewData["PerfilId"] = new SelectList(_context.Perfils, "Id", "PerfilUsuario", permissoes.PerfilId);
            return View(permissoes);
        }

        // GET: Permissoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permissoes = await _context.Permissoes
                .Include(p => p.Perfil)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (permissoes == null)
            {
                return NotFound();
            }

            return View(permissoes);
        }

        // POST: Permissoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permissoes = await _context.Permissoes.FindAsync(id);
            _context.Permissoes.Remove(permissoes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermissoesExists(int id)
        {
            return _context.Permissoes.Any(e => e.Id == id);
        }
    }
}
