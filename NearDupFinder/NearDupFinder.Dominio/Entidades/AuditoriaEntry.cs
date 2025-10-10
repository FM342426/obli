namespace NearDupFinder.Dominio.Entidades;

public class AuditoriaEntry
{
    public DateTime Timestamp { get; private set; }
    public string Usuario { get; private set; }
    public string Accion { get; private set; }
    
    public string Detalles { get; private set; }
    
    public AuditoriaEntry(string usuario, string accion, string detalles = "")
    {
        this.Timestamp = DateTime.UtcNow;
        this.Usuario = usuario;  
        this.Accion = accion;
        this.Detalles = detalles;
    }
}