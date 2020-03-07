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
    ////[Authorize]
    public class TokensController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TokensController(ApplicationDbContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Token> Tokens { get; set; }

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

            var tokens = from t in _context.Tokens
                         select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                tokens = tokens.Where(t => t.Hash.Contains(searchString)
                                        || t.Usuarios.NomeUsuario.Contains(searchString)
                                        || t.Usuarios.SobrenomeUsuario.Contains(searchString));
            }

            tokens = sortOrder switch
            {
                "name_desc" => tokens.OrderBy(t => t.Hash),
                "Date" => tokens.OrderBy(t => t.DataValidadeToken),
                "date_desc" => tokens.OrderByDescending(t => t.DataValidadeToken),
                _ => tokens.OrderBy(t => t.DataValidadeToken),
            };
            int pageSize = 4;
            return View(await PaginatedList<Token>.CreateAsync(
                tokens.AsNoTracking().Include(t => t.Usuarios), pageNumber ?? 1, pageSize));
        }

        // GET: Tokens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var token = await _context.Tokens
                .Include(t => t.Usuarios)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (token == null)
            {
                return NotFound();
            }

            return View(token);
        }

        // GET: Tokens/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeUsuario");
            return View();
        }

        // POST: Tokens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Hash,DataValidadeToken,UsuarioId")] Token token)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var verificaHASH = await _context.Tokens.AnyAsync(t => t.Hash == token.Hash || t.Usuarios == token.Usuarios);
                    if (token.DataValidadeToken < DateTime.Today)
                    {
                        TempData["ErroSalvar"] = "A data de validade do token deverá ser atual ou posterior: '" + token.DataValidadeToken + "'\t , tente novamente!";
                        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeUsuario", token.UsuarioId);
                        return View("Create");
                    }

                    else if (verificaHASH == false)
                    {
                        _context.Add(token);
                        await _context.SaveChangesAsync();
                        TempData["Salvar"] = "Seu Token: '" + token.Hash.ToUpper() + "'\t foi cadastrado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }

                    else
                    {
                        TempData["ErroSalvar"] = "O seguinte Hash: '" + token.Hash + "'\t , ja está atribuido a um usuário cadastrado, tente novamente!";
                        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeUsuario", token.UsuarioId);
                        return View("Create");
                    }
                }
                catch (Exception)
                {
                    TempData["ErroInesperado"] = "Não foi possivel cadastrar o hash: '" + token.Hash.ToUpper() + "'\t , tente novamente!";
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeUsuario", token.UsuarioId);
            return View(token);
        }

        // GET: Tokens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var token = await _context.Tokens.FindAsync(id);
            if (token == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeUsuario", token.UsuarioId);
            return View(token);
        }

        // POST: Tokens/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Hash,DataValidadeToken,UsuarioId")] Token token)
        {
            if (id != token.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var verificaHASH = await _context.Tokens.AnyAsync(t => t.Hash == token.Hash || t.Usuarios == token.Usuarios && t.Id != token.Id);
                    if (token.DataValidadeToken < DateTime.Today)
                    {
                        TempData["ErroSalvar"] = "A data de validade do token deverá ser atual ou posterior: '" + token.DataValidadeToken + "'\t , tente novamente!";
                        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeUsuario", token.UsuarioId);
                        return View("Edit");
                    }

                    else if (verificaHASH == false)
                    {
                        _context.Update(token);
                        await _context.SaveChangesAsync();
                        TempData["Editar"] = "Seu Token: '" + token.Hash.ToUpper() + "'\t foi atualizado com sucesso!";
                    }

                    else
                    {
                        TempData["ErroSalvar"] = "O seguinte Hash: '" + token.Hash + "'\t , ja está atribuido a um usuário cadastrado, tente novamente!";
                        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeUsuario", token.UsuarioId);
                        return View("Edit");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TokenExists(token.Id))
                    {
                        TempData["ErroInesperado"] = "Não foi possivel editar o hash: '" + token.Hash.ToUpper() + "'\t , tente novamente!";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeUsuario", token.UsuarioId);
            return View(token);
        }

        // GET: Tokens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var token = await _context.Tokens
                .Include(t => t.Usuarios)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (token == null)
            {
                return NotFound();
            }

            return View(token);
        }

        // POST: Tokens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var token = await _context.Tokens.FindAsync(id);
                _context.Tokens.Remove(token);
                TempData["Delete"] = "O token '" + token.Hash.ToUpper() + "'\t foi deletado!";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErroInesperado"] = "Ocorreu um erro inesperado ao deletar o token, tente novamente!";
                return View("Delete");
            }
        }

        private bool TokenExists(int id)
        {
            return _context.Tokens.Any(e => e.Id == id);
        }
    }
}
