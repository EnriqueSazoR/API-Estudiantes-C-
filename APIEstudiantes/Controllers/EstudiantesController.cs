using APIEstudiantes.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIEstudiantes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : Controller
    {
        private readonly ApplicationDBContext _db;

        public EstudiantesController(ApplicationDBContext db)
        {
            _db = db;
        }

        // Endpoints

        //POST
        [HttpPost]
        public ActionResult<Estudiante> PostEstudiante([FromBody] Estudiante estudiante)
        {
            if (estudiante == null || string.IsNullOrEmpty(estudiante.Nombre) || estudiante.Calificacion < 0 || estudiante.Calificacion > 10)
            {
                return BadRequest("Datos inválidos");
            }
            _db.Add(estudiante);
            _db.SaveChanges();
            //estudiantes.Add(estudiante);

            return Ok(estudiante);
        }

        //GETALL
        [HttpGet]
        public ActionResult<List<Estudiante>> GetEstudiantes()
        {
            var estudiantes = _db.Estudiante.ToList();
            if (estudiantes == null || estudiantes.Count == 0)
            {
                return BadRequest("No hay datos");
            }
            return Ok(estudiantes);

        }

        //GET(ID)
        [HttpGet("{id}")]
        public ActionResult<Estudiante> GetEstudiante(int id)
        {
            var estudianteBuscado = _db.Estudiante.FirstOrDefault(x => x.Id == id);
            if (estudianteBuscado == null)
            {
                return NotFound();
            }
            return Ok(estudianteBuscado);
        }

        //PUT(ID)
        [HttpPut("{id}")]
        public ActionResult<Estudiante> PutEstudiante(int id, Estudiante estudiante)
        {
            var estudianteModificado = _db.Estudiante.FirstOrDefault(e => e.Id == id);
            if (estudianteModificado == null)
            {
                return NotFound();
            }
            estudianteModificado.Nombre = estudiante.Nombre;
            estudianteModificado.Calificacion = estudiante.Calificacion;
            _db.SaveChanges();

            return Ok(estudianteModificado);

        }

        //DELETE(ID)
        [HttpDelete("id")]
        public ActionResult<Estudiante> DeleteEstudiante(int id)
        {
            var estudianteEliminado = _db.Estudiante.FirstOrDefault(e => e.Id == id);
            if (estudianteEliminado == null)
            {
                return NotFound();
            }
            _db.Estudiante.Remove(estudianteEliminado);
            _db.SaveChanges();
            return Ok();

        }
    }
}
