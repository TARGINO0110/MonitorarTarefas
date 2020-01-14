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
            this.Tarefas = new HashSet<Tarefas>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "Nome do projeto")]
        public string NomeProjeto { get; set; }

        [Display(Name = "Descrição")]
        public string DescricaoProjeto { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Iniciado em")]
        public DateTime DataInicioProjeto { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Finalizado em")]
        public DateTime DataFinalizadoProjeto { get; set; }

        [Required(ErrorMessage = "É necessário informar a data de entrga do seu projeto!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data da entrega")]
        public DateTime DataEntregaProjeto { get; set; }


        [ForeignKey("Usuarios")]
        public int UsuarioId { get; set; }

        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }

        [Display(Name = "Usuário")]
        public virtual Usuarios Usuarios { get; set; }

        public virtual Categoria Categoria { get; set; }

        public ICollection<Tarefas> Tarefas { get; set; }

        public DateTime Datacriacao => DataInicioProjeto = DateTime.Now;
    }
}
