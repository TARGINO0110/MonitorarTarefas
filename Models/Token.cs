using System;
using System.ComponentModel.DataAnnotations;

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
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Validade do Token")]
        public DateTime DataValidadeToken { get; set; }
    }
}
