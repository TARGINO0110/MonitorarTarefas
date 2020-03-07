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
    ////[Authorize]
    public class EmpresasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpresasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Empresa> Empresas { get; set; }

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

            var empresas = from e in _context.Empresas
                           select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                empresas = empresas.Where(e => e.NomeEmpresa.Contains(searchString)
                                            || e.CNPJ.Contains(searchString)
                                            || e.EmailEmpresa.Contains(searchString)
                                            || e.PorteEmpresa.Contains(searchString));
            }

            empresas = sortOrder switch
            {
                "name_desc" => empresas.OrderBy(a => a.NomeEmpresa),
                "Date" => empresas.OrderBy(a => a.DataFundacao),
                "date_desc" => empresas.OrderByDescending(a => a.DataFundacao),
                _ => empresas.OrderByDescending(a => a.CNPJ),
            };
            int pageSize = 4;
            return View(await PaginatedList<Empresa>.CreateAsync(
                empresas.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // GET: Empresas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empresas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeEmpresa,EnderecoEmpresa,TelefoneEmpresa,EmailEmpresa,CNPJ,DataFundacao,PorteEmpresa")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var verificaCNPJ = await _context.Empresas.AnyAsync(e=> e.CNPJ == empresa.CNPJ);
                    if (empresa.DataFundacao > DateTime.Today)
                    {
                        TempData["ErroSalvar"] = "A data da fundação da sua empresa deverá ser atual ou anterior: '" + empresa.DataFundacao + "'\t , tente novamente!";
                        return View("Create");
                    }

                    else if (verificaCNPJ == false)
                    {
                        _context.Add(empresa);
                        await _context.SaveChangesAsync();
                        TempData["Salvar"] = "Sua empresa: '" + empresa.NomeEmpresa.ToUpper() + "'\t foi cadastrada com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["ErroSalvar"] = "O CNPJ " + empresa.CNPJ.ToUpper() + " ja está cadastrado, tente novamente!";
                        return View("Create");
                    }
                }
                catch
                {
                    TempData["ErroInesperado"] = "Não foi possivel cadastrar a empresa: '" + empresa.NomeEmpresa.ToUpper() + "'\t , tente novamente!";
                    return RedirectToAction(nameof(Index));
                }
                
            }
            return View(empresa);
        }

        // GET: Empresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        // POST: Empresas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeEmpresa,EnderecoEmpresa,TelefoneEmpresa,EmailEmpresa,CNPJ,DataFundacao,PorteEmpresa")] Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var verificaCNPJ = await _context.Empresas.AnyAsync(e => e.CNPJ == empresa.CNPJ && e.Id != empresa.Id);
                    if (empresa.DataFundacao > DateTime.Today)
                    {
                        TempData["ErroSalvar"] = "A data da fundação da sua empresa deverá ser atual ou anterior: '" + empresa.DataFundacao + "'\t , tente novamente!";
                        return View("Edit");
                    }
                    else if (verificaCNPJ == false)
                    {
                        _context.Update(empresa);
                        await _context.SaveChangesAsync();
                        TempData["Editar"] = "Sua empresa: '" + empresa.NomeEmpresa.ToUpper() + "'\t foi atualizada com sucesso!";
                    }
                    else
                    {
                        TempData["ErroSalvar"] = "O CNPJ " + empresa.CNPJ.ToUpper() + " ja está cadastrado, tente novamente!";
                        return View("Edit");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.Id))
                    {
                        TempData["ErroInesperado"] = "Não foi possivel editar a empresa: '" + empresa.NomeEmpresa.ToUpper() + "'\t , tente novamente!";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var empresa = await _context.Empresas.FindAsync(id);
                _context.Empresas.Remove(empresa);
                TempData["Deletar"] = "A empresa '" + empresa.CNPJ.ToUpper() + "'\t foi deletado!";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception)
            {
                TempData["ErroInesperado"] = "Ocorreu um erro inesperado ao deletar a empresa, tente novamente!";
                return View("Delete");
            }
            
        }

        private bool EmpresaExists(int id)
        {
            return _context.Empresas.Any(e => e.Id == id);
        }
    }
}
