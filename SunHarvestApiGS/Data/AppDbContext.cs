using Microsoft.EntityFrameworkCore;
using SunHarvestApiGS.Models;

namespace SunHarvestApiGS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Fazenda> Fazendas { get; set; }

        public DbSet<Alerta> Alertas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Nomes das tabelas no Oracle 

            modelBuilder.Entity<Usuario>().ToTable("TB_USUARIO");
            modelBuilder.Entity<Fazenda>().ToTable("TB_FAZENDA");
            modelBuilder.Entity<Alerta>().ToTable("TB_ALERTA");

            //Email do usuário é único

            modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}
