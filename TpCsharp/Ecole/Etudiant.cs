namespace Ecole;

// Un étudiant et ses notes ; deux étudiants sont égaux s'ils ont le même nom, le même prénom et la même date de naissance
public class Etudiant : IEquatable<Etudiant>
{
    // Les notes et leurs coefficients sont rangés dans le même ordre
    private readonly List<double> _notes = [];
    private readonly List<int> _coefs = [];
    private int _annee;

    public Etudiant(string nom, string prenom, int annee)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nom);
        ArgumentException.ThrowIfNullOrWhiteSpace(prenom);
        Nom = nom;
        Prenom = prenom;
        // On passe par la propriété pour que l'année soit vérifiée dès la création
        Annee = annee;
    }

    public string Nom { get; }
    public string Prenom { get; }
    // Fixée uniquement à la création
    public DateOnly Naissance { get; init; }

    // Modifiable, mais toujours entre 1 et 5
    public int Annee
    {
        get => _annee;
        set
        {
            if (value is < 1 or > 5)
                throw new ArgumentOutOfRangeException(nameof(value), "L'année doit être comprise entre 1 et 5.");
            _annee = value;
        }
    }

    // Propriétés calculées à chaque lecture
    public string NomComplet => $"{Prenom} {Nom.ToUpper()}";
    public int NbNotes => _notes.Count;
    // Les notes sont lisibles mais pas modifiables : il faut passer par AjouterNote
    public IReadOnlyList<double> Notes => _notes.AsReadOnly();

    // Le coefficient vaut 1 si on ne le précise pas
    public void AjouterNote(double n, int coef = 1)
    {
        if (n is < 0 or > 20)
            throw new ArgumentOutOfRangeException(nameof(n), "La note doit être comprise entre 0 et 20.");
        if (coef < 1)
            throw new ArgumentOutOfRangeException(nameof(coef), "Le coefficient doit être au moins égal à 1.");
        _notes.Add(n);
        _coefs.Add(coef);
    }

    // Moyenne simple, 0 s'il n'y a aucune note
    public double Moyenne()
    {
        if (_notes.Count == 0)
            return 0;
        double somme = 0;
        foreach (double n in _notes)
            somme += n;
        return somme / _notes.Count;
    }

    // Somme des notes multipliées par leur coefficient, divisée par la somme des coefficients
    public double MoyennePonderee()
    {
        if (_notes.Count == 0)
            return 0;
        double somme = 0;
        int totalCoefs = 0;
        for (int i = 0; i < _notes.Count; i++)
        {
            somme += _notes[i] * _coefs[i];
            totalCoefs += _coefs[i];
        }
        return somme / totalCoefs;
    }

    // Les années 1 à 3 deviennent L1 à L3, les années 4 et 5 deviennent M1 et M2
    public override string ToString()
    {
        string niveau = Annee <= 3 ? $"L{Annee}" : $"M{Annee - 3}";
        return $"{NomComplet} ({niveau}) : {Moyenne():F2}";
    }

    // On ne compare que ce qui ne change jamais après la création
    public bool Equals(Etudiant? autre)
    {
        if (autre is null)
            return false;
        return Nom == autre.Nom && Prenom == autre.Prenom && Naissance == autre.Naissance;
    }

    // Version héritée d'object, utilisée par certaines collections
    public override bool Equals(object? obj) => Equals(obj as Etudiant);

    // Deux étudiants égaux doivent avoir le même code, sinon le HashSet ne les reconnaît pas
    public override int GetHashCode() => HashCode.Combine(Nom, Prenom, Naissance);
}
