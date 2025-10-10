namespace NearDupFinder.Dominio.Entidades;

public static class RolesConstantes
{
    public const string ADMINISTRADOR = "Administrador";
    public const string REVISOR_CATALOGO = "Revisor de Catálogo";

    public static readonly List<string> TodosLosRoles = new()
    {
        ADMINISTRADOR,
        REVISOR_CATALOGO
    };
}