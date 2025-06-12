using APIEstudiantes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIEstudiantes.Controllers
{
    [Route("/api[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDBContext db, IConfiguration configuration)
        {   
            _db = db;
            _configuration = configuration;
        }

        //Endponint POST
        [HttpPost("login")]
        public ActionResult<Usuario> postUsuario([FromBody] UsuarioLoginDto login)
        {
           if(string.IsNullOrEmpty(login.NombreUsuario) || string.IsNullOrEmpty(login.Clave))
           {
                return BadRequest("Crendeciales Inválidas");
           }

            var usuario = _db.Usuario.FirstOrDefault(u => u.NombreUsuario == login.NombreUsuario);
            if (usuario == null || usuario.Clave != login.Clave)
            {
                return Unauthorized("Usuario o contraseña incorrectos");
            }

            var token = GenerarJwtToken(usuario);
            return Ok(new { Token = token });
           
        }

        private string GenerarJwtToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtConfig:Issuer"],
                audience: _configuration["JwtConfig:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       
    }


    // DTO para la entrada del login
    public class UsuarioLoginDto
    {
        public string NombreUsuario { get; set; } = null!;
        public string Clave { get; set; } = null!;
    }
}
