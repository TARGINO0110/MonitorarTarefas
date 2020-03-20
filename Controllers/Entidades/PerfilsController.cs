using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Monitorar_Tarefas.Data;
using Monitorar_Tarefas.Models.Entidades;

namespace Monitorar_Tarefas.Controllers
{
    public class PerfilsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerfilsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Perfils
        public async Task<IActionResult> Index()
        {
            return View(await _context.Perfils.ToListAsync());
        }

        // GET: Perfils/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfil = await _context.Perfils
                .FirstOrDefaultAsync(m => m.Id == id);
            if (perfil == null)
            {
                return NotFound();
            }

            return View(perfil);
        }

        // GET: Perfils/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Perfils/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PerfilUsuario")] Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var validaPerfil = await _context.Perfils.AnyAsync(p => p.PerfilUsuario == perfil.PerfilUsuario);
                    switch (validaPerfil)
                    {
                        case true:
                            TempData["ErroSalvar"] = "Não é possivel cadastrar o perfil de acesso: " + perfil.PerfilUsuario.ToUpper() + "'\t, no momento este perfil já se encontra cadastrado.";
                            return View("Create");

                        default:
                            _context.Add(perfil);
                            await _context.SaveChangesAsync();
                            TempData["Salvar"] = "O perfil " + perfil.PerfilUsuario.ToUpper() + "'\t foi cadastrado com sucesso!.";
                            return RedirectToAction(nameof(Index));
                    }
                }
                catch
                {
                    TempData["ErroInesperado"] = "Não foi possivel cadastrar o perfil : '" + perfil.PerfilUsuario.ToUpper() + "'\t pois ocorreu um erro interno, tente novamente!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(perfil);
        }

        // GET: Perfils/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfil = await _context.Perfils.FindAsync(id);
            if (perfil == null)
            {
                return NotFound();
            }
            return View(perfil);
        }

        // POST: Perfils/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PerfilUsuario")] Perfil perfil)
        {
            if (id != perfil.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var validaPerfil = await _context.Perfils.AnyAsync(p => p.PerfilUsuario == perfil.PerfilUsuario && p.Id != perfil.Id);
                    switch (validaPerfil)
                    {
                        case true:
                            TempData["ErroSalvar"] = "Não é possivel atualizar o perfil de acesso: " + perfil.PerfilUsuario.ToUpper() + "'\t, no momento este perfil já se encontra cadastrado.";
                            return View("Edit");

                        default:
                            _context.Update(perfil);
                            await _context.SaveChangesAsync();
                            TempData["Editar"] = "O perfil " + perfil.PerfilUsuario.ToUpper() + "'\t foi atualizado com sucesso!.";
                            return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerfilExists(perfil.Id))
                    {
                        TempData["ErroInesperado"] = "Não foi possivel editar este perfil: '" + perfil.PerfilUsuario.ToUpper() + "'\t , tente novamente!";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(perfil);
        }

        // GET: Perfils/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfil = await _context.Perfils
                .FirstOrDefaultAsync(m => m.Id == id);
            if (perfil == null)
            {
                return NotFound();
            }

            return View(perfil);
        }

        // POST: Perfils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var perfil = await _context.Perfils.FindAsync(id);
                _context.Perfils.Remove(perfil);
                TempData["Deletar"] = "O perfil '" + perfil.PerfilUsuario.ToUpper() + "'\t foi deletado!";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            catch (Exception)
            {
                TempData["ErroInesperado"] = "Ocorreu um erro inesperado ao deletar o perfil, tente novamente!";
                return View("Delete");
            }
        }

        private bool PerfilExists(int id)
        {
            return _context.Perfils.Any(e => e.Id == id);
        }
    }
}
