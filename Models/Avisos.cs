using System;
using System.ComponentModel.DataAnnotations;

namespace Monitorar_Tarefas.Models
{
    public class Avisos
    {
        public int Id { get; set; }

        [Display(Name = "Título do aviso")]
        public string TituloAviso { get; set; }

        [Display(Name = "Descrição do aviso")]
        public string DescricaoAviso { get; set; }

        [Required(ErrorMessage = "É necessario informar a data da postagem!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data da postagem")]
        public DateTime DataPostagemAviso { get; set; }
    }
}
