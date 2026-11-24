namespace Ecole;

// Une promo : un nom et la liste de ses étudiants
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

    // Cherche un étudiant par son nom, sans tenir compte des majuscules ; null s'il n'existe pas
    public Etudiant? Chercher(string nom)
    {
        foreach (var e in _etudiants)
        {
            if (string.Equals(e.Nom, nom, StringComparison.OrdinalIgnoreCase))
                return e;
        }
        return null;
    }

    // L'étudiant qui a la meilleure moyenne ; null si la promo est vide
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

    // Moyenne des moyennes des étudiants, 0 si la promo est vide
    public double MoyenneGenerale()
    {
        if (_etudiants.Count == 0)
            return 0;
        double somme = 0;
        foreach (var e in _etudiants)
            somme += e.Moyenne();
        return somme / _etudiants.Count;
    }

    // Une ligne par étudiant, avec sa mention et sa moyenne pondérée
    public void AfficherReleve()
    {
        Console.WriteLine($"Relevé de la promo {Nom}");
        foreach (var e in _etudiants)
            Console.WriteLine($"{e} - {Bareme.Mention(e.Moyenne())} - moyenne pondérée {e.MoyennePonderee():F2}");
    }
}
