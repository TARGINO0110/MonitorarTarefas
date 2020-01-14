using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Monitorar_Tarefas.Models
{
    public class Usuarios
    {
        public Usuarios()
        {
            this.Projetos = new HashSet<Projetos>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "É necessário informar seu primeiro nome!")]
        [Display(Name = "Seu primeiro Nome")]
        public string NomeUsuario { get; set; }

        [Required(ErrorMessage = "É necessário informar seu  Sobrenome!")]
        [Display(Name = "Seu Sobrenome")]
        public string SobrenomeUsuario { get; set; }

        [StringLength(11, ErrorMessage = "Informe os 11 digitos do seu CPF!")]
        [Display(Name = "CPF")]
        public string CPF { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de nascimento")]
        public DateTime DataNascimento { get; set; }

        public virtual ICollection<Projetos> Projetos { get; set; }

    }
}
