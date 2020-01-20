using System;
using System.ComponentModel.DataAnnotations;

namespace Monitorar_Tarefas.Models
{
    public class Avisos
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o título do aviso")]
        [Display(Name = "Título do aviso")]
        public string TituloAviso { get; set; }
        [Required(ErrorMessage = "Descreva o aviso a ser postado!")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Escrava de 10 à 100 caracters!")]
        [Display(Name = "Descrição do aviso")]
        public string DescricaoAviso { get; set; }

        [Required(ErrorMessage = "É necessario informar a data da postagem!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data da postagem")]
        public DateTime DataPostagemAviso { get; set; }

        [Required(ErrorMessage = "É necessario informar a data de expiração!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de expiração")]
        public DateTime DataExpiracaoAviso { get; set; }
    }
}
