using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Interfaces;
using NearDupFinder.Dominio.Utiles;

namespace NearDupFinder.Aplicacion;

     public class UsuarioService : IUsuarioService
   {
       private readonly IRepositorioUsuarios _repositorio;
       private readonly IAuditoriaService _auditoriaService; 
       public UsuarioService(IRepositorioUsuarios repositorio, IAuditoriaService auditoriaService)
       {
           _repositorio = repositorio;
           _auditoriaService = auditoriaService; 
       }
 
       public async Task<IEnumerable<Usuario>> ObtenerTodos()
       {
           return await _repositorio.ObtenerTodos();
       }

       public async Task<Usuario?> ObtenerPorId(int id)
       {
           if (id <= 0) 
               return null;
           
           return await _repositorio.ObtenerPorId(id);
       }

       public async Task<Usuario?> ObtenerPorEmail(string email)
       {
           if (string.IsNullOrWhiteSpace(email)) 
               return null;
           
           return await _repositorio.ObtenerPorEmail(email.Trim());
       }

       public async Task<Usuario> CrearUsuario(Usuario usuario)
       {
           await ValidarUsuarioParaCreacion(usuario);
         
           var usuarioCreado = await _repositorio.Agregar(usuario);
          
           var rolesAsignados = string.Join(", ", usuario.Roles);
           var detalles = $@"
            Se registraron los siguientes datos para el nuevo usuario:
              - ID: {usuario.Id}
              - Nombre: {usuario.Nombre} {usuario.Apellido}
              - Email: {usuario.Email}
              - Fec.Nacimiento: {usuario.FechaNacimiento}
              - Roles Asignados: [{rolesAsignados}] 
            "; 

           await _auditoriaService.LogAccion(
               accion: $"Alta de usuario: {usuario.Email}",
               detalles: detalles.Trim()  
           );
           
           return usuarioCreado;
       }

       public async Task<Usuario> ActualizarUsuario(Usuario usuario)
       {
           await ValidarUsuarioParaActualizacion(usuario);
           var usuarioOriginal = await _repositorio.ObtenerPorId(usuario.Id);
           var usuarioEditado = await _repositorio.Actualizar(usuario);
           
           var detallesLog = new System.Text.StringBuilder();

           // si cambio el nombre
           if (usuarioOriginal.Nombre != usuarioEditado.Nombre)
           {
               detallesLog.AppendLine($"  - Nombre cambiado de '{usuarioOriginal.Nombre}' a '{usuarioEditado.Nombre}'.");
           }
           
           // si cambio el ape
           if (usuarioOriginal.Apellido != usuarioEditado.Apellido)
           {
               detallesLog.AppendLine($"  - Apellido cambiado de '{usuarioOriginal.Apellido}' a '{usuarioEditado.Apellido}'.");
           }
           
           // si cambio la fec nac
           if (usuarioOriginal.FechaNacimiento != usuarioEditado.FechaNacimiento)
           {
               detallesLog.AppendLine($"  - FechaNacimiento cambiada de '{usuarioOriginal.FechaNacimiento}' a '{usuarioEditado.FechaNacimiento}'.");
           }
           
           // si hubo cambio en el mail
           if (usuarioOriginal.Email != usuarioEditado.Email)
           {
               detallesLog.AppendLine($"  - Email cambiado de '{usuarioOriginal.Email}' a '{usuarioEditado.Email}'.");
           }

           // Compara los Roles 
           var rolesOriginales = string.Join(", ", usuarioOriginal.Roles.OrderBy(r => r));
           var rolesEditados = string.Join(", ", usuarioEditado.Roles.OrderBy(r => r));
           if (rolesOriginales != rolesEditados)
           {
               detallesLog.AppendLine($"  - Roles cambiados de [{rolesOriginales}] a [{rolesEditados}].");
           } 

           // si se detectaron cambios  se registra en la auditoría.
           if (detallesLog.Length > 0)
           {
               await _auditoriaService.LogAccion(
                   accion: $"Edición de usuario: {usuarioEditado.Email}",
                   detalles: "Se detectaron los siguientes cambios:\n" + detallesLog.ToString()
               );
           }
           return usuarioEditado;
       }
       public async Task<bool> EliminarUsuario(int id)
       {
           if (id <= 0) 
               return false;

           var usuario = await _repositorio.ObtenerPorId(id);
           
           if (usuario == null)
               return false;
            
           if (usuario.Email.Equals("admin@gmail.com", StringComparison.OrdinalIgnoreCase))
           {
                  throw new Exception("No se puede eliminar al usuario administrador principal del sistema.");
           }
           
           var eliminado = await _repositorio.Eliminar(id);
           
           if (eliminado)
           {
               
               var rolesDelUsuario = string.Join(", ", usuario.Roles);

               await _auditoriaService.LogAccion(
                   accion: $"Baja de usuario: {usuario.Email}",
                   detalles: $@"Se eliminó permanentemente al usuario:
              - ID: {usuario.Id}
              - Nombre: {usuario.Nombre}
              - Email: {usuario.Email}
              - Roles que tenía: [{rolesDelUsuario}]"
               );
           }

           
           return eliminado;
       }

       public async Task<bool> ValidarEmail(string email)
       {
           if (string.IsNullOrWhiteSpace(email)) 
               return false;
           
           if (!Validador.IsEmailValido(email)) 
               return false;

           return !await _repositorio.ExisteEmail(email.Trim());
       }

       public async Task<bool> ValidarEmailEdicion(string email, int idUsuario)
       {
           if (string.IsNullOrWhiteSpace(email)) 
               return false;

          
           if (!Validador.IsEmailValido(email))
               return false;

           return !await _repositorio.ExisteEmailExcluyendoId(email.Trim(), idUsuario);
       }

       public async Task<IEnumerable<Usuario>> ObtenerPorRol(string rol)
       {
           if (string.IsNullOrWhiteSpace(rol)) 
               return Enumerable.Empty<Usuario>();
           
           return await _repositorio.ObtenerPorRol(rol);
       }

       public async Task<IEnumerable<Usuario>> ObtenerAdministradores()
       {
           return await _repositorio.ObtenerPorRol(RolesConstantes.ADMINISTRADOR);
       }

       public async Task<IEnumerable<Usuario>> ObtenerRevisores()
       {
           return await _repositorio.ObtenerPorRol(RolesConstantes.REVISOR_CATALOGO);
       } 
       private async Task ValidarUsuarioParaCreacion(Usuario usuario)
       {
           ValidarDatosBasicos(usuario);
         
           if (string.IsNullOrWhiteSpace(usuario.PasswordHash))
           {
               throw new ArgumentException("La contraseña es obligatoria.");
           }

           Validador.IsPasswordValido(usuario.Password);
           
           if (await _repositorio.ExisteEmail(usuario.Email))
           {
               throw new InvalidOperationException("Ya existe un usuario con ese email.");
           }
          
           ValidarRoles(usuario);
       }

       private async Task ValidarUsuarioParaActualizacion(Usuario usuario)
       {
           ValidarDatosBasicos(usuario);
            
           var usuarioExistente = await _repositorio.ObtenerPorId(usuario.Id);
           
           if (usuarioExistente == null)
           {
               throw new InvalidOperationException("Usuario no encontrado.");
           }
           
           if (await _repositorio.ExisteEmailExcluyendoId(usuario.Email, usuario.Id))
           {
               throw new InvalidOperationException("Ya existe otro usuario con ese email.");
           }

           if (!String.IsNullOrEmpty(usuario.Password))
           {
               Validador.IsPasswordValido(usuario.Password);
           }
           
           ValidarRoles(usuario);
       }

       private static void ValidarDatosBasicos(Usuario usuario)
       {
           if (string.IsNullOrWhiteSpace(usuario.Nombre))
               throw new ArgumentException("El nombre es obligatorio.");
        
           if (string.IsNullOrWhiteSpace(usuario.Email))
               throw new ArgumentException("El email es obligatorio.");

           if (!Validador.IsEmailValido(usuario.Email))
               throw new ArgumentException("El formato del email no es válido.");
       }
        private static void ValidarRoles(Usuario usuario)
       {
           if (usuario.Roles?.Any() != true)
           {
               throw new ArgumentException("El usuario debe tener al menos un rol asignado.");
           }

         
           foreach (var rol in usuario.Roles)
           {
               if (!RolesConstantes.TodosLosRoles.Contains(rol))
               {
                   throw new ArgumentException($"El rol '{rol}' no es válido.");
               }
           }
       }

   }