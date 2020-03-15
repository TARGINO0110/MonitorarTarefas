using Monitorar_Tarefas.Models.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monitorar_Tarefas.Models
{
    public class Token
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "O Token deve conter 50 caracteres para efetuar sua validação.")]
        [Display(Name = "Hash do token")]
        public string Hash { get; set; }

        [Required(ErrorMessage = "É necessario informar a data de validade do token!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Validade do Token")]
        public DateTime DataValidadeToken { get; set; }

        [Required(ErrorMessage ="Selecione o perfil do token")]
        [Display(Name ="Perfil do Token")]
        public string PerfilToken { get; set; }

        [ForeignKey("Usuarios")]
        public int UsuarioId { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}
