using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Monitorar_Tarefas.Models.Entidades
{
    public class Perfil
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Será necessário selecionar o tipo de perfil do seu usuário")]
        [Display(Name ="Tipo de perfil")]
        public string PerfilUsuario { get; set; }

        [Required(ErrorMessage = "Será necessário selecionar o código do perfil")]
        [Display(Name ="Codigo do perfil")]
        public int CodigoPerfil { get; set; }
    }
}
