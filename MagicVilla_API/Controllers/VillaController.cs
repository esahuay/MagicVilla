using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDBContext _db;

        public VillaController(ILogger<VillaController> logger, ApplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Obtener todas las villas");
            return Ok(_db.Villas.ToList());
        }

        /*
         * Nombre utilizado para llamar la ruta.
         */
        [HttpGet("id:int",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error con la villa id: " + id);
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CrearVilla([FromBody] VillaDTO villaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_db.Villas.FirstOrDefault(x => x.Nombre.ToLower() == villaDTO.Nombre.ToLower()) != null){
                ModelState.AddModelError("Nombre Existe", "La villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }


            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }

            if(villaDTO.Id>0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Villa modelo = new()
            {
                Nombre = villaDTO.Nombre,
                Detalle = villaDTO.Detalle,
                ImagenUrl = villaDTO.ImagenURL,
                Ocupantes = villaDTO.Ocupantes,
                Tarifa = villaDTO.Tarifa,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Amenidad = villaDTO.Amenidades,
                Fechacreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now
            };
            _db.Villas.Add(modelo);
            _db.SaveChanges();

            /*
                villaDTO.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
                VillaStore.villaList.Add(villaDTO);
            */

            return CreatedAtRoute("GetVilla", new {id = villaDTO.Id}, villaDTO);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult EliminarVilla(int id) 
        { 
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            
            _db.Villas.Remove(villa);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id) { 
                return BadRequest();
            }
            /*
            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            villa.Nombre = villaDTO.Nombre;
            villa.Ocupantes = villaDTO.Ocupantes;
            villa.MetrosCuadrados = villaDTO.MetrosCuadrados;
            */

            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }

            Villa modelo = new()
            {
                Id = villaDTO.Id,
                Nombre = villaDTO.Nombre,
                Detalle = villaDTO.Detalle,
                ImagenUrl = villaDTO.ImagenURL,
                Ocupantes = villaDTO.Ocupantes,
                Tarifa = villaDTO.Ocupantes,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Amenidad = villaDTO.Amenidades,
                Fechacreacion = villa.Fechacreacion,
                FechaActualizacion = DateTime.Now
            };
            _db.Villas.Update(modelo);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);

            if (villa == null) return BadRequest();
            
            VillaDTO villadto = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImagenURL = villa.ImagenUrl,
                Ocupantes = villa.Ocupantes,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Amenidades = villa.Amenidad
            };

            patchDTO.ApplyTo(villadto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImagenUrl = villa.ImagenUrl,
                Ocupantes = villa.Ocupantes,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Amenidad = villa.Amenidad,
                FechaActualizacion = DateTime.Now
            };

            _db.Villas.Update(modelo);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
