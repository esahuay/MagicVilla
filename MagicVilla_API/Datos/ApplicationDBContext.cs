using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Datos
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Villa> Villas { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                    {
                        Id=1,
                        Nombre="Villa Real",
                        Detalle = "Detalle de la villa...",
                        ImagenUrl = "",
                        Ocupantes = 5,
                        MetrosCuadrados = 50,
                        Tarifa = 200,
                        Amenidad = "",
                        Fechacreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now
                    },
                    new Villa()
                    {
                        Id = 2,
                        Nombre = "Preimum vista a la piscina",
                        Detalle = "Detalle de la villa...",
                        ImagenUrl = "",
                        Ocupantes = 5,
                        MetrosCuadrados = 50,
                        Tarifa = 200,
                        Amenidad = "",
                        Fechacreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now
                    }
                );
        }

    }
}
