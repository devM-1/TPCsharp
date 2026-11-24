// La promo réécrite en LINQ : chaque méthode est une requête sur la liste des notes
public class Promo(string nom, IEnumerable<Note> notes)
{
    private readonly List<Note> _notes = notes.ToList();
    public string Nom { get; } = nom;

    // Les prénoms des étudiants, sans doublon
    public IEnumerable<string> Etudiants() => _notes
        .Select(n => n.Prenom)
        .Distinct();

    // Moyenne pondérée d'un étudiant, 0 s'il n'a aucune note
    public double Moyenne(string prenom)
    {
        // On fige les notes de l'étudiant pour ne parcourir la liste qu'une seule fois
        var sesNotes = _notes.Where(n => n.Prenom == prenom).ToList();
        // Sans ce test, un étudiant sans note donnerait 0 / 0, c'est-à-dire NaN
        return sesNotes.Count == 0
            ? 0
            : sesNotes.Sum(n => n.Valeur * n.Coef) / sesNotes.Sum(n => n.Coef);
    }

    // Moyenne des moyennes des étudiants ; DefaultIfEmpty évite l'exception de Average sur une promo vide
    public double MoyenneGenerale() => Etudiants()
        .Select(Moyenne)
        .DefaultIfEmpty(0)
        .Average();

    // L'étudiant qui a la meilleure moyenne ; MaxBy renvoie null si la promo est vide
    public string? Major() => Etudiants().MaxBy(Moyenne);

    // Chaque étudiant avec sa moyenne, de la meilleure à la moins bonne
    public IEnumerable<(string Prenom, double Moyenne)> Classement() => Etudiants()
        .Select(p => (Prenom: p, Moyenne: Moyenne(p)))
        .OrderByDescending(c => c.Moyenne);

    // Pour chaque matière, le nombre de notes au moins égales à 10
    public IReadOnlyDictionary<string, int> NbAdmisParMatiere() => _notes
        .GroupBy(n => n.Matiere)
        .ToDictionary(g => g.Key, g => g.Count(n => n.Valeur >= 10));

    // Une ligne par étudiant, dans l'ordre du classement, avec son rang et sa mention
    public IEnumerable<string> Releve() => Classement()
        .Select((c, i) => $"{i + 1}. {c.Prenom} {c.Moyenne:F2} {Bareme.Mention(c.Moyenne)}");
}