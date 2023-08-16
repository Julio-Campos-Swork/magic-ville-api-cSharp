using MagicVille_Api.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicVille_Api.Datos
{
    //creamos la clase que va a crear la base de datos con la conexion de entity framework
    public class ApplicationDbContext :DbContext
    {
        //aseguramos el constructor de la clase para hacer la conexion directa con la base de datos dandole las opciones de base
        //recordar ejecutar los comandos de migracion para la base de datos "add-migration "nombrequequeramosdarle""  "update-database" con eso creamos la base de datos
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
            
        }       //db set crea la base de datos apartir de un modelo, en este caso Villa
        public DbSet<Villa> Villas { get; set; }

        //podemos crear registros por default en dado caso que lo necesitemos con este metodo del modelbuilder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id=1,
                    Nombre="Inicial",
                    Detalle="Default",
                    Tarifa=1,
                    Ocupantes=1,
                    MetrosCuadrados=10,
                    ImagenUrl="/images/1.jpg",
                    Amenidad="si",
                    FechaCreacion= DateTime.Now,
                    FechaActualizacion= DateTime.Now
                });
        }
    }
}
