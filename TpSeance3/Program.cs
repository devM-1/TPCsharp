using System.Globalization;

// Lecture des notes comme à la séance 2 (on la rendra plus stricte à l'étape 1)
List<Note> notes = File.ReadLines("notes.csv")
    .Skip(1)
    .Select(Parser)
    .OfType<Note>()
    .ToList();

// Lecture des inscrits : on découpe chaque ligne et on ajoute le quatrième champ, le code postal
List<Inscrit> inscrits = File.ReadLines("etudiants.csv")
    .Skip(1)
    .Select(ligne => ligne.Split(','))
    .Select(c => new Inscrit(c[0], c[1], c[2], c[3]))
    .ToList();

Console.WriteLine($"{notes.Count} notes valides et {inscrits.Count} inscrits.");

static Note? Parser(string ligne)
{
    var c = ligne.Split(',');
    if (c.Length != 5) return null;
    if (!double.TryParse(c[3], CultureInfo.InvariantCulture, out var v)) return null;
    if (v < 0 || v > 20) return null;
    if (!int.TryParse(c[4], out var coef)) return null;
    return new Note(c[0], c[1], c[2], v, coef);
}