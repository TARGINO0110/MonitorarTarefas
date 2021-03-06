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

            avisos = sortOrder switch
            {
                "name_desc" => avisos.OrderBy(a => a.TituloAviso),
                "Date" => avisos.OrderBy(a => a.DataPostagemAviso),
                "date_desc" => avisos.OrderByDescending(a => a.DataExpiracaoAviso),
                _ => avisos.OrderByDescending(a => a.DataPostagemAviso),
            };
            int pageSize = 4;
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
                try
                {
                    var validarTexto = await _context.Avisos.AnyAsync(a => a.TituloAviso == avisos.TituloAviso || a.DescricaoAviso == avisos.DescricaoAviso);
                    switch (validarTexto)
                    {
                        case true:
                            TempData["ErroSalvar"] = "Já existe o título " + avisos.TituloAviso + " ou Descrição " + avisos.DescricaoAviso + " cadastrado na base de dados'\t , tente novamente!";
                            return View("Create");

                        default:
                            if (avisos.DataPostagemAviso < DateTime.Today)
                            {
                                TempData["ErroSalvar"] = "A sua data de postagem deve ser posterior ou atual, não é possivel ser em: '" + avisos.DataPostagemAviso + "'\t , tente novamente!";
                                return View("Create");
                            }
                            else if (avisos.DataExpiracaoAviso < DateTime.Today)
                            {
                                TempData["ErroSalvar"] = "A sua data de expiração deve ser posterior ou atual, não é possivel ser em: '" + avisos.DataExpiracaoAviso + "'\t , tente novamente!";
                                return View("Create");
                            }
                            else
                            {
                                _context.Add(avisos);
                                await _context.SaveChangesAsync();
                                TempData["Salvar"] = "Seu aviso: '" + avisos.TituloAviso.ToUpper() + "'\t foi cadastrado com sucesso!";
                                return RedirectToAction(nameof(Index));
                            }
                    }
                }
                catch
                {
                    TempData["ErroInesperado"] = "Não foi possivel cadastrar o aviso: '" + avisos.TituloAviso.ToUpper() + "'\t , tente novamente!";
                    return RedirectToAction(nameof(Index));
                }

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
                    var validarTexto = await _context.Avisos.AnyAsync(a => a.TituloAviso == avisos.TituloAviso || a.DescricaoAviso == avisos.DescricaoAviso || a.Id != avisos.Id);
                    switch (validarTexto)
                    {
                        case true :
                            TempData["ErroSalvar"] = "Já existe o título " + avisos.TituloAviso + " ou Descrição " + avisos.DescricaoAviso + " cadastrado na base de dados'\t , tente novamente!";
                            return View("Edit");
                        
                        default:
                            if (avisos.DataPostagemAviso < DateTime.Today)
                            {
                                TempData["ErroSalvar"] = "A sua data de postagem deve ser posterior ou atual, não é possivel ser em: '" + avisos.DataPostagemAviso + "'\t , tente novamente!";
                                return View("Edit");
                            }
                            else if (avisos.DataExpiracaoAviso < DateTime.Today)
                            {
                                TempData["ErroSalvar"] = "A sua data de expiração deve ser posterior ou atual, não é possivel ser em: '" + avisos.DataExpiracaoAviso + "'\t , tente novamente!";
                                return View("Edit");
                            }
                            else
                            {
                                _context.Add(avisos);
                                await _context.SaveChangesAsync();
                                TempData["Salvar"] = "Seu aviso: '" + avisos.TituloAviso.ToUpper() + "'\t foi atualizado com sucesso!";
                                return RedirectToAction(nameof(Index));
                            }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AvisosExists(avisos.Id))
                    {
                        TempData["ErroInesperado"] = "Não foi possivel editar o aviso: '" + avisos.TituloAviso.ToUpper() + "'\t , tente novamente!";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
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
            try
            {
                var avisos = await _context.Avisos.FindAsync(id);
                _context.Avisos.Remove(avisos);
                TempData["Deletar"] = "O aviso '" + avisos.TituloAviso.ToUpper() + "'\t foi deletado!";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErroInesperado"] = "Ocorreu um erro inesperado ao deletar o aviso, tente novamente!";
                return View("Delete");
            }
        }

        private bool AvisosExists(int id)
        {
            return _context.Avisos.Any(e => e.Id == id);
        }
    }
}
