namespace Ecole;

public class Promo
{
    private readonly List<Etudiant> _etudiants = [];

    public Promo(string nom)
    {
        Nom = nom;
    }

    public string Nom { get; }

    public void Inscrire(Etudiant e)
    {
        _etudiants.Add(e);
    }

    public Etudiant? Chercher(string nom)
    {
        foreach (var e in _etudiants)
        {
            if (string.Equals(e.Nom, nom, StringComparison.OrdinalIgnoreCase))
                return e;
        }
        return null;
    }

    public Etudiant? Major()
    {
        Etudiant? meilleur = null;
        foreach (var e in _etudiants)
        {
            if (meilleur is null || e.Moyenne() > meilleur.Moyenne())
                meilleur = e;
        }
        return meilleur;
    }

    public double MoyenneGenerale()
    {
        if (_etudiants.Count == 0)
            return 0;
        double somme = 0;
        foreach (var e in _etudiants)
            somme += e.Moyenne();
        return somme / _etudiants.Count;
    }

    public void AfficherReleve()
    {
        Console.WriteLine($"Relevé de la promo {Nom}");
        foreach (var e in _etudiants)
            Console.WriteLine($"{e} - {Bareme.Mention(e.Moyenne())} - moyenne pondérée {e.MoyennePonderee():F2}");
    }
}