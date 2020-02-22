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

    public class TarefasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TarefasController(ApplicationDbContext context)
        {
            _context = context;
        }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Tarefas> Tarefas { get; set; }
        // GET: Tarefas
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

            var tarefas = from t in _context.Tarefas
                          select t;



            if (!String.IsNullOrEmpty(searchString))
            {
                tarefas = tarefas.Where(t => t.NomeTarefa.Contains(searchString)
                                       || t.DescricaoTarefa.Contains(searchString)
                                       || t.Projetos.NomeProjeto.Contains(searchString)
                                       || t.Usuarios.NomeUsuario.Contains(searchString));
            }


            tarefas = sortOrder switch
            {
                "name_desc" => tarefas.OrderBy(t => t.NomeTarefa),
                "Date" => tarefas.OrderBy(t => t.DataInicioTarefa),
                "date_desc" => tarefas.OrderByDescending(t => t.DataFinalizadoTarefa),
                _ => tarefas.OrderByDescending(t => t.DataInicioTarefa),
            };
            int pageSize = 4;
            return View(await PaginatedList<Tarefas>.CreateAsync(
                tarefas.AsNoTracking().Include(u => u.Projetos).Include(u => u.Usuarios), pageNumber ?? 1, pageSize));
        }


        // GET: Tarefas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefas = await _context.Tarefas
                .Include(t => t.Projetos)
                .Include(t => t.Usuarios)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tarefas == null)
            {
                return NotFound();
            }

            return View(tarefas);
        }

        // GET: Tarefas/Create
        public IActionResult Create()
        {
            ViewData["ProjetoId"] = new SelectList(_context.Projetos, "Id", "NomeProjeto");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeUsuario");
            return View();
        }

        // POST: Tarefas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeTarefa,DescricaoTarefa,DataInicioTarefa,DataFinalizadoTarefa,DataEntregaTarefa,Situacao,ProjetoId,UsuarioId")] Tarefas tarefas)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if ((tarefas.DataInicioTarefa >= DateTime.Today) && (tarefas.DataEntregaTarefa >= DateTime.Today) && (tarefas.DataFinalizadoTarefa >= DateTime.Today))
                    {
                        _context.Add(tarefas);
                        await _context.SaveChangesAsync();
                        TempData["Salvar"] = "Sua tarefa: '" + tarefas.NomeTarefa.ToUpper() + "'\t foi cadastrada com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErroSalvar"] = "A data de Inicio/Entrega ou final deverá ser atual ou posterior, tente novamente!";
                        return View("Create");
                    }
                }
                catch
                {
                    TempData["ErroInesperado"] = "Não foi possivel cadastrar a tarefa: '" + tarefas.NomeTarefa.ToUpper() + "'\t , tente novamente!";
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["ProjetoId"] = new SelectList(_context.Projetos, "Id", "NomeProjeto", tarefas.ProjetoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeUsuario", tarefas.UsuarioId);
            return View(tarefas);
        }

        // GET: Tarefas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefas = await _context.Tarefas.FindAsync(id);
            if (tarefas == null)
            {
                return NotFound();
            }
            ViewData["ProjetoId"] = new SelectList(_context.Projetos, "Id", "NomeProjeto", tarefas.ProjetoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeUsuario", tarefas.UsuarioId);
            return View(tarefas);
        }

        // POST: Tarefas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeTarefa,DescricaoTarefa,DataInicioTarefa,DataFinalizadoTarefa,DataEntregaTarefa,Situacao,ProjetoId,UsuarioId")] Tarefas tarefas)
        {
            if (id != tarefas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if ((tarefas.DataInicioTarefa >= DateTime.Today) && (tarefas.DataEntregaTarefa >= DateTime.Today) && (tarefas.DataFinalizadoTarefa >= DateTime.Today))
                    {
                        _context.Update(tarefas);
                        TempData["Editar"] = "Sua tarefa: '" + tarefas.NomeTarefa.ToUpper() + "'\t foi atualizado com sucesso!";
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        TempData["ErroSalvar"] = "A data de Inicio/Entrega ou final deverá ser atual ou posterior, tente novamente!";
                        ViewData["ProjetoId"] = new SelectList(_context.Projetos, "Id", "NomeProjeto", tarefas.ProjetoId);
                        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeUsuario", tarefas.UsuarioId);
                        return View("Edit");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TarefasExists(tarefas.Id))
                    {
                        TempData["ErroInesperado"] = "Não foi possivel editar a tarefa: '" + tarefas.NomeTarefa.ToUpper() + "'\t , tente novamente!";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjetoId"] = new SelectList(_context.Projetos, "Id", "NomeProjeto", tarefas.ProjetoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeUsuario", tarefas.UsuarioId);
            return View(tarefas);
        }

        // GET: Tarefas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefas = await _context.Tarefas
                .Include(t => t.Projetos)
                .Include(t => t.Usuarios)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tarefas == null)
            {
                return NotFound();
            }

            return View(tarefas);
        }

        // POST: Tarefas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var tarefas = await _context.Tarefas.FindAsync(id);
                _context.Tarefas.Remove(tarefas);
                TempData["Deletar"] = "A tarefa '" + tarefas.NomeTarefa.ToUpper() + "'\t foi deletado!";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErroInesperado"] = "Ocorreu um erro inesperado ao deletar o projeto, tente novamente!";
                return View("Delete");
            }
        }

        private bool TarefasExists(int id)
        {
            return _context.Tarefas.Any(e => e.Id == id);
        }
    }
}
