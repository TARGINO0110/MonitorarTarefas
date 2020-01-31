﻿using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
            var projetos = from p in _context.Projetos
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                tarefas = tarefas.Where(t => t.NomeTarefa.Contains(searchString)
                                       || t.DescricaoTarefa.Contains(searchString));

                projetos = projetos.Where(p => p.NomeProjeto.Contains(searchString));
            }


            switch (sortOrder)
            {
                case "name_desc":
                    tarefas = tarefas.OrderByDescending(t => t.NomeTarefa);
                    break;
                case "Date":
                    tarefas = tarefas.OrderBy(t => t.DataInicioTarefa);
                    break;
                case "date_desc":
                    tarefas = tarefas.OrderByDescending(t => t.DataFinalizadoTarefa);
                    break;
                default:
                    projetos = projetos.OrderBy(p => p.NomeProjeto);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Tarefas>.CreateAsync(
                tarefas.AsNoTracking(), pageNumber ?? 1, pageSize));
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
                _context.Add(tarefas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
                    _context.Update(tarefas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TarefasExists(tarefas.Id))
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
            var tarefas = await _context.Tarefas.FindAsync(id);
            _context.Tarefas.Remove(tarefas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TarefasExists(int id)
        {
            return _context.Tarefas.Any(e => e.Id == id);
        }
    }
}
