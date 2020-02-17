using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monitorar_Tarefas.Data;
using Monitorar_Tarefas.Models;
using System;
using System.Linq;

namespace Monitorar_Tarefas.Controllers
{
    ////[Authorize]
    public class RegTokenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegTokenController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            Token model = new Token();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(Token model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var tokenUser = (from t in _context.Tokens
                                     where t.Hash == model.Hash
                                     select t).SingleOrDefault();

                    if (tokenUser == null)
                    {
                        ViewBag.Message = "Token inválido, tente novamente !";
                        return View(model);
                    }
                    else
                    {
                        TempData["tokenSuccess"] = "O Token foi validado com sucesso!";
                        return RedirectToAction("Projetos", "Index");
                    }
                }

                catch (Exception ex)
                {
                    ViewBag.Message = "Não foi possível processar o Token.";
                    return View(model + ex.Message);
                }
            }
            else
            {
                ViewBag.Message = "Informe o Token !";
                return View(model);
            }
        }
    }
}

