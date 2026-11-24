namespace Geometrie;

// Un carré est un rectangle aux côtés égaux ; sealed : aucune classe ne peut en hériter
public sealed class Carre : Rectangle
{
    public Carre(double cote) : base("Carré", cote, cote)
    {
    }
}
