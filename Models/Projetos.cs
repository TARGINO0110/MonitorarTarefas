using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monitorar_Tarefas.Models
{
    public class Projetos
    {
        public Projetos()
        {
            this.Usuarios = new List<Usuarios>();
            this.Tarefas = new List<Tarefas>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome do seu projeto!")]
        [Display(Name = "Nome do projeto")]
        public string NomeProjeto { get; set; }

        [Required(ErrorMessage = "Descreva sobre o seu projeto!")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Escrava de 10 à 100 caracters!")]
        [Display(Name = "Descrição")]
        public string DescricaoProjeto { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Iniciado em")]
        public DateTime DataInicioProjeto { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Finalizado em")]
        public DateTime DataFinalizadoProjeto { get; set; }

        [Required(ErrorMessage = "É necessário informar a data de entrga do seu projeto!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data da entrega")]
        public DateTime DataEntregaProjeto { get; set; }

        [Display(Name = "Categoria")]
        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }

        public virtual Categoria Categoria { get; set; }
        public virtual ICollection<Usuarios> Usuarios { get; set; }
        public virtual ICollection<Tarefas> Tarefas { get; set; }

    }
}
