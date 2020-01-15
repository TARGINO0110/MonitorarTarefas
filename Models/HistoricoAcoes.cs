using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monitorar_Tarefas.Models
{
    public class HistoricoAcoes
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nº da Ação")]
        public long NumeroAcao { get; set; }
        [Display(Name = "Observações")]
        public string OnservacaoAcao { get; set; }
        [Display(Name = "Data/Hora")]
        public DateTime DataHoraAcao { get; set; }

        [ForeignKey("Projetos")]
        public int ProjetosId { get; set; }

        public virtual Projetos Projetos { get; set; }
    }
}
