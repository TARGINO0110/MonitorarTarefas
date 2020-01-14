using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Monitorar_Tarefas.Models
{
    public class Tarefas
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nome da tarefa")]
        public string NomeTarefa { get; set; }

        [Display(Name = "Descrição")]
        public string DescricaoTarefa { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Iniciado em")]
        public DateTime DataInicioTarefa { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Finalizado em")]
        public DateTime DataFinalizadoTarefa { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data da entrega")]
        public DateTime DataEntregaTarefa { get; set; }

        [Display(Name = "Situação")]
        public string Situacao { get; set; }

        [ForeignKey("Projetos")]
        public int ProjetoId { get; set; }

        public virtual Projetos Projetos { get; set; }

        public DateTime Datacriacao => DataInicioTarefa = DateTime.Now;
    }
}
