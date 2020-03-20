using Monitorar_Tarefas.Models.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Monitorar_Tarefas.Models.Monitoramento
{
    public class Permissoes
    {
        [Key]
        public int Id { get; set; }
        public bool PermEmpresa { get; set; }
        public bool PermPerfil { get; set; }
        public bool PermUsuarios { get; set; }
        public bool PermHistoricoAcoes { get; set; }
        public bool PermPermissoes { get; set; }
        public bool PermToken { get; set; }
        public bool PermAvisos { get; set; }
        public bool PermCategoria { get; set; }
        public bool PermProjetos { get; set; }
        public bool PermTarefas { get; set; }

        [ForeignKey("Perfils")]
        public int PerfilId { get; set; }

        public virtual Perfil Perfil { get; set; }
    }
}
