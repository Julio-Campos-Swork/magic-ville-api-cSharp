using MagicVille_Api.Datos;
using MagicVille_Api.Modelos;
using MagicVille_Api.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVille_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        //creamos el servicio de logger que viene preinstalado en la api
        private readonly ILogger<VillaController> _logger;
        //aplicamos la configuracion para conectar con la base de datos
        private readonly ApplicationDbContext _db;
        //creamos el contructor que lo va ainicializar una vez iniciado aqui, lo podmeos ocupar en el resto del controlador lo que iniciemos como el db lo podremos ocupar
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db)
        {

            _logger = logger;
            _db = db;
        }
        [HttpGet]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            //obtenemos toda la lista de vilals de la lista de villas
            _logger.LogInformation("obtener villas");

            //archivos locales o lo que esta en nuestro VillaStore de ejemplo
            //return Ok(VillaStore.villaList);

            //conectamos a base de datos, retornamos lo que tengamos en base de datos
            return Ok(_db.Villas.ToList());
        }


        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {
                //usamos el logger para mostrar mensajes en la consola de depuracion
                _logger.LogError("Error, el id no es correcto" + id);
                return BadRequest();
            }
            //esto es para local
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            //esto es para conexion de base de datos
            var villa = _db.Villas.FirstOrDefault(villa => villa.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CrearVilla([FromBody] VillaDto villaDto)
        {

            //validamos que los datos del modelo sean validos
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //validamos si el nombre existe (en este caso)
            //obtenemos el primero que encuentrre que sea igual a lo que estamos ingresando

            //aplicamos la misma estrategia de reemplazo
            //if (VillaStore.villaList.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
            if (_db.Villas.FirstOrDefault(villa => villa.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
            {
                //creamos una respuesta perzonalizada con AddModelError
                ModelState.AddModelError("NameExist","El nombre introducido ya existe en la base de datos");
                return BadRequest(ModelState);
            }

            //validamos que todo el contenido exista
            if(villaDto == null)
            {
                return BadRequest(villaDto);
            }
            if(villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            // **** Todas estas lineas son exclusivas local *****
            //obtenemos los ID de forma decendente para obtener el ultimo id y asignar el nuevo id agregando un 1
            //villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            //agregamos a nuestra lista almacenada lo que se asigno en la entrada
            //VillaStore.villaList.Add(villaDto);
            //creamos una ruta con el nuevo valor ingresado

            //creamos modelo para agregarlo a la base
            Villa modelo = new()
            {
               
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                Tarifa = villaDto.Tarifa,
                Ocupantes = villaDto.Ocupantes,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                ImagenUrl = villaDto.ImagenUrl,
                Amenidad = villaDto.Amenidad
            };
            //agregamos los datos del modelo a la base de datos y salvamos  los datos
            _db.Villas.Add(modelo);
            _db.SaveChanges();
            return CreatedAtRoute("GetVilla", new {id = villaDto.Id}, villaDto);
        }
        //utilizamos el vervo HttpDelete que recibe un id (en este caso)
        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //utilizamos IActionResult por que no necesitamos obtener el modelo
        public IActionResult DeleteVilla(int id)
        {
            //Verificamos que ingresen un ID valido
            if (id == 0)
            {
                return BadRequest();
            }
            //verifiamos que exista el id en la base de datos (este caso la store) si no existe regresamos notfound
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            var villa = _db.Villas.FirstOrDefault(villa => villa.Id == id);

            if (villa == null)
            {
                return NotFound();
            }
            //removemos de la store o base de datos la villa que encontramos
            //VillaStore.villaList.Remove(villa);

            //delete base de datos
            _db.Villas.Remove(villa);
            _db.SaveChanges();

            //si se elimina regresamos no content por convencion
            return NoContent();
        }
        //el metodo put actualiza todo el modelo, si alguno no es correcto devolveremos error
        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //los datos tienen que venir del body en formato VillaDto
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if (villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }
            //el update se aplica primeramente buscando el registro deseado y despues sobreescribimos los valores, en este caso, local
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre = villaDto.Nombre;
            //villa.Ocupantes = villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;
            Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                Tarifa = villaDto.Tarifa,
                Ocupantes = villaDto.Ocupantes,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                ImagenUrl = villaDto.ImagenUrl,
                Amenidad = villaDto.Amenidad
            };
            _db.Villas.Update(modelo);
            _db.SaveChanges();
            return NoContent();
        }

        // referencia https://jsonpatch.com/
        // "path": "/nombre", la ruta (o valor a cambiar
    //"op": "replace", la operacion
    //"value": "el texto" lo que se cambia
        //Agregamos el metodo path para actualizar parcialmente apoyados de los paquetes JsonPatch
        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //Con JsonPatchDocument hacemos referencia que es tipo Villadto y le damos el nombre patchDto para identificar
        public IActionResult UpdatePartialVilla(int id,JsonPatchDocument<VillaDto> patchDto)
        {
            //validamos que exiatn los datos
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            //en este caso es especifico ya que hacemos ods llamadas tenemos que poner el metodo asnotracking para que no se cruce la informacion y tengamos problemas
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(villa => villa.Id == id);

            VillaDto villaDto = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                Tarifa = villa.Tarifa,
                Ocupantes = villa.Ocupantes,
                MetrosCuadrados = villa.MetrosCuadrados,
                ImagenUrl = villa.ImagenUrl,
                Amenidad = villa.Amenidad
            };

            if (villaDto == null) return BadRequest();
            patchDto.ApplyTo(villaDto, ModelState);
            if (!ModelState.IsValid)
            {
                //si no tenemos modelo valido, ya se por cualquiera de sus propiedadess retornaremos error
                return BadRequest(ModelState);
            }

            Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                Tarifa = villaDto.Tarifa,
                Ocupantes = villaDto.Ocupantes,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                ImagenUrl = villaDto.ImagenUrl,
                Amenidad = villaDto.Amenidad
            };
            _db.Villas.Update(modelo);
            _db.SaveChanges();


            //si todo va bien devolvemos no contentend indicando que todo se actualizo
            return NoContent();
        }
    }
   
}
