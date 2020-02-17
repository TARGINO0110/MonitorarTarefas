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
    //[Authorize]
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }
        public string NameSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Categoria> Categorias { get; set; }

        // GET: Categorias
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;
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

            var categoria = from c in _context.Categorias
                            select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                categoria = categoria.Where(c => c.NomeCategoria.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    categoria = categoria.OrderBy(c => c.NomeCategoria);
                    break;
                default:
                    categoria = categoria.OrderBy(c => c.NomeCategoria);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Categoria>.CreateAsync(
                categoria.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeCategoria")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(categoria);
                    await _context.SaveChangesAsync();
                    TempData["Salvar"] = "Sua categoria: '" + categoria.NomeCategoria.ToUpper() + "'\t foi cadastrado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    TempData["ErroInesperado"] = "Não foi possivel cadastrar a categoria: '" + categoria.NomeCategoria.ToUpper() + "'\t , tente novamente!";
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeCategoria")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    TempData["Editar"] = "Sua categoria: '" + categoria.NomeCategoria.ToUpper() + "'\t foi atualizado com sucesso!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
                    {
                        TempData["ErroInesperado"] = "Não foi possivel editar a categoria: '" + categoria.NomeCategoria.ToUpper() + "'\t , tente novamente!";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var categoria = await _context.Categorias.FindAsync(id);
                TempData["Deletar"] = "A categoria '" + categoria.NomeCategoria.ToUpper() + "'\t foi deletado!";
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErroInesperado"] = "Ocorreu um erro inesperado ao deletar a categoria, tente novamente!";
                return View("Delete");
            }

        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}
