using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monitorar_Tarefas.Models
{
    public class Usuarios
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "É necessário informar o seu primeiro nome!")]
        [Display(Name = "Seu primeiro Nome")]
        public string NomeUsuario { get; set; }

        [Required(ErrorMessage = "É necessário informar o seu  Sobrenome!")]
        [Display(Name = "Seu Sobrenome")]
        public string SobrenomeUsuario { get; set; }

        [Required(ErrorMessage = "É necessário informar sim ou não!")]
        [Display(Name = "Gerente de projetos?")]
        public bool GerenteProjeto { get; set; }

        [Display(Name = "Token")]
        public bool TokenAcesso { get; set; }

        [Required(ErrorMessage = "Informe os 11 digitos do seu CPF!")]
        [StringLength(14, ErrorMessage = "Verifique se o número do seu CPF está correto")]
        [Display(Name = "CPF")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "É necessário informar o número do seu Telefone ou Celular!")]
        [StringLength(14, ErrorMessage = "Verifique se o número do seu Telefone ou Celular está correto!")]
        [Display(Name = "Telefone/Celular")]
        public string TelefoneCelular { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data de nascimento")]
        public DateTime DataNascimento { get; set; }

        [ForeignKey("Colaborador da Empresa")]
        public int EmpresaId { get; set; }

        public virtual Empresa Empresa { get; set; }
    }
}
