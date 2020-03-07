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
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Usuarios> Usuarios { get; set; }

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

            var usuarios = from u in _context.Usuarios
                           select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                usuarios = usuarios.Where(u => u.NomeUsuario.Contains(searchString)
                                            || u.SobrenomeUsuario.Contains(searchString)
                                            || u.CPF.Contains(searchString)
                                            || u.Empresa.NomeEmpresa.Contains(searchString));
            }

            usuarios = sortOrder switch
            {
                "name_desc" => usuarios.OrderBy(u => u.NomeUsuario),
                "Date" => usuarios.OrderBy(u => u.DataNascimento),
                "date_desc" => usuarios.OrderByDescending(u => u.DataNascimento),
                _ => usuarios.OrderByDescending(u => u.NomeUsuario),
            };
            int pageSize = 4;
            return View(await PaginatedList<Usuarios>.CreateAsync(
                usuarios.AsNoTracking().Include(u => u.Empresa), pageNumber ?? 1, pageSize));
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios
                .Include(u => u.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "NomeEmpresa");
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeUsuario,SobrenomeUsuario,GerenteProjeto,CPF,TelefoneCelular,DataNascimento,EmpresaId")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var verificaCPF = await _context.Usuarios.AnyAsync(u => u.CPF == usuarios.CPF);
                    if (usuarios.DataNascimento > DateTime.Today)
                    {
                        TempData["ErroSalvar"] = "A sua data de nascimento deve ser anterior, não é possivel ser em: '" + usuarios.DataNascimento + "'\t , tente novamente!";
                        ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "NomeEmpresa");
                        return View("Create");
                    }
                    else if (verificaCPF == false)
                    {
                        _context.Add(usuarios);
                        await _context.SaveChangesAsync();
                        TempData["Create"] = "O Usuário: '" + usuarios.NomeUsuario.ToUpper() + "'\t foi cadastrado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErroSalvar"] = "O CPF " + usuarios.CPF.ToUpper() + " ja está cadastrado na base de dados de outro usuário, tente novamente!";
                        ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "NomeEmpresa");
                        return View("Create");
                    }
                }
                catch
                {
                    TempData["ErroInesperado"] = "Não foi possivel cadastrar o usuario: '" + usuarios.NomeUsuario.ToUpper() + "'\t , tente novamente!";
                    return RedirectToAction(nameof(Index));
                }

            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "NomeEmpresa");
            return View(usuarios);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "NomeEmpresa");
            return View(usuarios);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeUsuario,SobrenomeUsuario,GerenteProjeto,CPF,TelefoneCelular,DataNascimento,EmpresaId")] Usuarios usuarios)
        {
            if (id != usuarios.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var verificaCPF = await _context.Usuarios.AnyAsync(u => u.CPF == usuarios.CPF && u.Id != usuarios.Id);
                    if (usuarios.DataNascimento > DateTime.Today)
                    {
                        TempData["ErroSalvar"] = "A sua data de nascimento deve ser anterior, não é possivel ser em: '" + usuarios.DataNascimento + "'\t , tente novamente!";
                        ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "NomeEmpresa");
                        return View("Edit");
                    }
                    else if (verificaCPF == false)
                    {
                        _context.Add(usuarios);
                        await _context.SaveChangesAsync();
                        TempData["Editar"] = "O Usuário: '" + usuarios.NomeUsuario.ToUpper() + "'\t foi atualizado com sucesso!";

                    }
                    else
                    {
                        TempData["ErroSalvar"] = "O CPF " + usuarios.CPF.ToUpper() + " ja está cadastrado na base de dados de outro usuário, tente novamente!";
                        ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "NomeEmpresa");
                        return View("Edit");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuariosExists(usuarios.Id))
                    {
                        TempData["ErroInesperado"] = "Não foi possivel editar o usuário: '" + usuarios.NomeUsuario.ToUpper() + "'\t , tente novamente!";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "NomeEmpresa");
            return View(usuarios);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios
                .Include(u => u.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var usuarios = await _context.Usuarios.FindAsync(id);
                _context.Usuarios.Remove(usuarios);
                TempData["Delete"] = "O Usuário '" + usuarios.NomeUsuario.ToUpper() + "'\t foi deletado!";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErroInesperado"] = "Ocorreu um erro inesperado ao deletar o token, tente novamente!";
                return View("Delete");
            }
        }

        private bool UsuariosExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
