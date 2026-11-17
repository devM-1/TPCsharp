namespace Ecole;

public class Etudiant
{
    private readonly List<double> _notes = [];
    private int _annee;

    public Etudiant(string nom, string prenom, int annee)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nom);
        ArgumentException.ThrowIfNullOrWhiteSpace(prenom);
        Nom = nom;
        Prenom = prenom;
        Annee = annee;
    }

    public string Nom { get; }
    public string Prenom { get; }
    public DateOnly Naissance { get; init; }

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

    public string NomComplet => $"{Prenom} {Nom.ToUpper()}";
    public int NbNotes => _notes.Count;
    public IReadOnlyList<double> Notes => _notes.AsReadOnly();

    public void AjouterNote(double n)
    {
        if (n is < 0 or > 20)
            throw new ArgumentOutOfRangeException(nameof(n), "La note doit être comprise entre 0 et 20.");
        _notes.Add(n);
    }

    public double Moyenne()
    {
        if (_notes.Count == 0)
            return 0;
        double somme = 0;
        foreach (double n in _notes)
            somme += n;
        return somme / _notes.Count;
    }

    public override string ToString()
    {
        string niveau = Annee <= 3 ? $"L{Annee}" : $"M{Annee - 3}";
        return $"{NomComplet} ({niveau}) : {Moyenne():F2}";
    }
}