using System.ComponentModel.DataAnnotations;

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
