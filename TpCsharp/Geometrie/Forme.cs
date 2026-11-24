namespace Geometrie;

// Classe de base de toutes les formes ; abstract : on ne peut pas créer une Forme directement
public abstract class Forme : IDessinable, IComparable<Forme>
{
    // protected : seules les classes filles peuvent appeler ce constructeur
    protected Forme(string nom)
    {
        Nom = nom;
    }

    public string Nom { get; }
    // Le centre est une struct : le lire en donne une copie
    public Point2D Centre { get; init; }

    // Chaque forme calcule elle-même son aire et son périmètre
    public abstract double Aire();
    public abstract double Perimetre();

    // virtual : une classe fille peut la redéfinir, comme Cercle qui ajoute son rayon
    public virtual string Decrire() => $"{Nom} : aire {Aire():F2}, périmètre {Perimetre():F2}";

    // Compare deux formes par leur aire ; c'est ce qu'utilise List.Sort
    public int CompareTo(Forme? autre)
    {
        if (autre is null)
            return 1;
        return Aire().CompareTo(autre.Aire());
    }

    // Decrire étant virtual, c'est la version de la forme réelle qui s'exécute
    public void Dessiner()
    {
        Console.WriteLine(Decrire());
    }
}
