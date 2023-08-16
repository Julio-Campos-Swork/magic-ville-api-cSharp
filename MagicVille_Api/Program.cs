using MagicVille_Api;
using MagicVille_Api.Datos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//agregamos el newtonsofjson
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//agregamos la conexion a base de datos
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    //mediante la opcion de la configuracion indicamos que usaremos en este caso sqlserver y leemos el archiv configuracion para obtener la cadena de configuracion
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//servicio para lel mapeo
builder.Services.AddAutoMapper(typeof(MappingConfig));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
