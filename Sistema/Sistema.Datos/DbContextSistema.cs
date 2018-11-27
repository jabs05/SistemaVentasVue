
using Microsoft.EntityFrameworkCore;
using Sistema.Datos.Maping.Almacen;
using Sistema.Entidades.Almacen;

namespace Sistema.Datos
{
    public class DbContextSistema: DbContext
    {
        public DbSet<Categoria> Categorias { get; set; }

        public DbContextSistema(DbContextOptions<DbContextSistema>options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoriaMap());
        }
    }
}
