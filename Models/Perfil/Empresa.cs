using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monitorar_Tarefas.Models
{
    public class Empresa
    {
        public Empresa()
        {
            this.Usuarios = new List<Usuarios>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "É necessário informar o nome da empresa!")]
        [Display(Name = "Nome da empresa")]
        public string NomeEmpresa { get; set; }

        [Required(ErrorMessage = "É necessário informar o endereço da empresa!")]
        [StringLength(100, ErrorMessage = "Preencha com até 100 caracteres!")]
        [Display(Name = "Endereço")]
        public string EnderecoEmpresa { get; set; }

        [Required(ErrorMessage = "Preencha o número do telefone da empresa!")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O número do telefone preenchido está incorreto!")]
        [Display(Name = "Nº Telefone/Celular")]
        public string TelefoneEmpresa { get; set; }

        [Required(ErrorMessage = "É necessário informar o E-mail da empresa!")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "O E-mail informado está incorreto!")]
        [Display(Name = "Email da empresa")]
        public string EmailEmpresa { get; set; }

        [Required(ErrorMessage = "Preencha o número do CNPJ da empresa!")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O número do CNPJ preenchido está incorreto!")]
        [Display(Name = "Nº CNPJ")]
        public string CNPJ { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fundado em")]
        public DateTime DataFundacao { get; set; }

        [Required(ErrorMessage = "Selecione o porte da empresa!")]
        [Display(Name = "Porte")]
        public string PorteEmpresa { get; set; }

        public virtual ICollection<Usuarios> Usuarios { get; set; }


    }
}