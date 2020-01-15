using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Monitorar_Tarefas.Models;

namespace Monitorar_Tarefas.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Projetos> Projetos { get; set; }
        public virtual DbSet<Tarefas> Tarefas { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<Avisos> Avisos { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<HistoricoAcoes> HistoricoAcoes { get; set; }

    }
}
