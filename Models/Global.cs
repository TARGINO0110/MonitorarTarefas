using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitorar_Tarefas.Models
{
    public class Global
    {
        public Empresa Empresa { get; set; }
        public Usuarios Usuarios { get; set; }
        public Projetos Projetos { get; set; }
        public Tarefas Tarefas { get; set; }
        public Categoria Categoria { get; set; }
        public Avisos Avisos { get; set; }
        public HistoricoAcoes HistoricoAcoes { get; set; }
        public Token Token { get; set; }

    }
}
