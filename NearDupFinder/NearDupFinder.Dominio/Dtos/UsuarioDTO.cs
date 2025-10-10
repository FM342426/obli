using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Dtos
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; } = DateTime.Today.AddYears(-18);
        public string Password { get; set; } = string.Empty;
        public string ConfirmarPassword { get; set; } = string.Empty;
        public List<string> RolesSeleccionados { get; set; } = new();
        
    }
}
