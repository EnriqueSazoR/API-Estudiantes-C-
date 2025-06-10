using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIEstudiantes.Models
{
    public class Estudiante
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public string? Nombre { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public double Calificacion { get; set; }

        // Llave foranea - realación entre estudiante y curso
        public int CursoId { get; set; }

       
        [ForeignKey("CursoId")]
        [JsonIgnore]
        [ValidateNever]
        public Curso? Curso { get; set; } = null!;
    }
}
