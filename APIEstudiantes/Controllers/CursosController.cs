using APIEstudiantes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIEstudiantes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : Controller
    {
        private readonly ApplicationDBContext _db;

        public CursosController(ApplicationDBContext db)
        {
            _db = db;

        }

        // Endpoints

        // POST
        [Authorize]
        [HttpPost]
        public ActionResult<Curso> PostCurso(Curso curso)
        {
            if(curso == null || string.IsNullOrEmpty(curso.NombreCurso))
            {
                return BadRequest("El campo nombre curso es inválido");
            }
            _db.Add(curso);
            _db.SaveChanges();
            return Ok(curso);
        }

        // GET (ALL)
        [HttpGet]
        public ActionResult<List<Curso>> GetCursos()
        {
            var cursos = _db.Curso.Include(x => x.Estudiante).ToList();
            if (cursos.Count == 0)
            {
                return BadRequest("No hay registros en la bd");
            }
            return Ok(cursos);
        }

        // GET (ID)
        [HttpGet("{id}")]
        public ActionResult<Curso> GetCurso(int id)
        {
            var cursoEncontrado = _db.Curso.FirstOrDefault(x => x.Id == id);
            if (cursoEncontrado == null)
            {
                return NotFound();
            }
            return Ok(cursoEncontrado);
        }

        // PUT
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult<Curso> PutCurso(int id, Curso curso)
        {
            var cursoModificado = _db.Curso.FirstOrDefault(x => x.Id == id);
            if (cursoModificado == null)
            {
                return NotFound();
            }
            cursoModificado.NombreCurso = curso.NombreCurso;
            _db.SaveChanges();

            return Ok(cursoModificado);
        }


        // DELETE
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult<Curso> DeleteCurso(int id)
        {
            var cursoEliminado = _db.Curso.FirstOrDefault(el => el.Id == id);
            if (cursoEliminado == null)
            {
                return NotFound();
            }
            _db.Curso.Remove(cursoEliminado);
            _db.SaveChanges();
            return Ok("Curso eliminado de la base de datos");
        }
    }
}
