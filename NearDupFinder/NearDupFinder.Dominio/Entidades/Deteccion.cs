namespace NearDupFinder.Dominio.Entidades;
public class Deteccion
{
    public int Id { get; set; }
    public int ItemAId { get; private set; }
    public int ItemBId { get; private set; }
    public DateTime FechaDeteccion { get; private set; }
    public string Estado { get; private set; }
    public double JaccardTitulo { get; private set; }

    public double JaccardDescripcion { get; private set; }
    public int MarcaEq { get; private set; }
    
    public int ModeloEq { get; private set; }
    
    public double Score { get; private set; }

    public Catalogo Catalogo { get; private set; }

    public List<string> TokensCompartidosTitulo { get; private set; }
    public List<string> TokensCompartidosDescripcion { get; private set; }

    
    public Deteccion(Item itemA, Item itemB)
    {
        if (itemA.Catalogo?.Id != itemB.Catalogo?.Id)
            throw new InvalidOperationException("Los items deben ser del mismo catálogo");
        
        ItemAId = itemA.Id;
        ItemBId = itemB.Id;
        Catalogo = itemA.Catalogo;
        FechaDeteccion = DateTime.UtcNow;
        Estado = "Pendiente";
        
        CalcularMetricas(itemA, itemB);
    }

    public void CalcularMetricas(Item itemA, Item itemB)
    {
        JaccardTitulo = ComparadorDeItems.CalcularJaccardTitulo(itemA.Titulo, itemB.Titulo);
        JaccardDescripcion = ComparadorDeItems.CalcularJaccardDescripcion(itemA.Descripcion, itemB.Descripcion);
        MarcaEq = ComparadorDeItems.CalcularMarcaEq(itemA.Marca, itemB.Marca);
        ModeloEq = ComparadorDeItems.CalcularModeloEq(itemA.Modelo, itemB.Modelo);
        Score = CalculadoraScore.CalcularScoreEntreItems(itemA, itemB);
        TokensCompartidosTitulo = ObtenerTokensCompartidos(itemA.Titulo, itemB.Titulo);
        TokensCompartidosDescripcion = ObtenerTokensCompartidos(itemA.Descripcion, itemB.Descripcion);

    }
    public List<string> ObtenerTokensCompartidos(string textoA, string textoB)
    {
        var tokensA = Tokenizer.Tokenize(Normalizador.Normalizar(textoA));
        var tokensB = Tokenizer.Tokenize(Normalizador.Normalizar(textoB));

        return tokensA.Intersect(tokensB).ToList();
    }

    public void Confirmar() => Estado = "Confirmado"; 
    public void Descartar() => Estado = "Descartado";
    public int IdMenor => Math.Min(ItemAId, ItemBId);
    public int IdMayor => Math.Max(ItemAId, ItemBId);
}