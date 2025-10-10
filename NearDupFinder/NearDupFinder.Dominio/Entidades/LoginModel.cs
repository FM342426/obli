using System.ComponentModel.DataAnnotations;

namespace NearDupFinder.Dominio.Entidades;

public class LoginModel
{
    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Password { get; set; } = "";
}