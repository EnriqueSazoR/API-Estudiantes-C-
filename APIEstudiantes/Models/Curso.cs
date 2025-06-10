using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIEstudiantes.Models
{
    public class Curso
    {
        // Propiedades
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string? NombreCurso { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [ValidateNever]
        public List<Estudiante> Estudiante { get; set; } = null!;
    }
}
