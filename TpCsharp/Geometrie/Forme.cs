namespace Geometrie;

public abstract class Forme : IDessinable, IComparable<Forme>
{
    protected Forme(string nom)
    {
        Nom = nom;
    }

    public string Nom { get; }
    public Point2D Centre { get; init; }

    public abstract double Aire();
    public abstract double Perimetre();

    public virtual string Decrire() => $"{Nom} : aire {Aire():F2}, périmètre {Perimetre():F2}";

    public int CompareTo(Forme? autre)
    {
        if (autre is null)
            return 1;
        return Aire().CompareTo(autre.Aire());
    }

    public void Dessiner()
    {
        Console.WriteLine(Decrire());
    }
}