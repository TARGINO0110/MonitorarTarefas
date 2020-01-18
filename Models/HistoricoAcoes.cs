using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monitorar_Tarefas.Models
{
    public class HistoricoAcoes
    {
        public HistoricoAcoes()
        {
            this.Usuarios = new List<Usuarios>();
            this.Projetos = new List<Projetos>();
        }

        [Key]
        public int Id { get; set; }
        [Display(Name = "Nº da Ação")]
        public long NumeroAcao { get; set; }
        [Display(Name = "Observações")]
        public string OnservacaoAcao { get; set; }
        [Display(Name = "Data/Hora")]
        public DateTime DataHoraAcao { get; set; }

        public virtual ICollection<Usuarios> Usuarios { get; set; }
        public virtual ICollection<Projetos> Projetos { get; set; }

    }
}
