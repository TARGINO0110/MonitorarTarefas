using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monitorar_Tarefas.Models
{
    public class Tarefas
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome da sua tarefa!")]
        [Display(Name = "Nome da tarefa")]
        public string NomeTarefa { get; set; }

        [Required(ErrorMessage = "Descreva sobre a sua tarefa!")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Escrava de 10 à 100 caracters!")]
        [Display(Name = "Descrição")]
        public string DescricaoTarefa { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Iniciado em")]
        public DateTime DataInicioTarefa { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Finalizado em")]
        public DateTime DataFinalizadoTarefa { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data da entrega")]
        public DateTime DataEntregaTarefa { get; set; }

        [Display(Name = "Situação")]
        public string Situacao { get; set; }

        [ForeignKey("Projetos")]
        public int ProjetoId { get; set; }

        [ForeignKey("Usuarios")]
        public int UsuarioId { get; set; }

        public virtual Projetos Projetos { get; set; }

        public virtual Usuarios Usuarios { get; set; }

    }
}
