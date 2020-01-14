using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Monitorar_Tarefas.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Será necessário selecionar ou informar a categoria do seu projeto!")]
        [Display(Name = "Categoria")]
        public string NomeCategoria { get; set; }
    }
}
