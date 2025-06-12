using System.ComponentModel.DataAnnotations;

namespace APIEstudiantes.Models
{
    public class Usuario
    {
        // Propiedades
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string NombreUsuario { get; set; } = null!;
        [MinLength(8, ErrorMessage = "La Contraseña debe tener al menos 8 caracteres")]
        public string Clave { get; set; } = null!;

    }
}
