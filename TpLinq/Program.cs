using System.Globalization;

List<Note> notes = File.ReadLines("notes.csv")
    .Skip(1)
    .Select(Parser)
    .OfType<Note>()
    .ToList();

var avecWhere = File.ReadLines("notes.csv")
    .Skip(1)
    .Select(Parser)
    .Where(n => n is not null)
    .ToList();

List<Inscrit> inscrits = File.ReadLines("etudiants.csv")
    .Skip(1)
    .Select(l => l.Split(','))
    .Select(c => new Inscrit(c[0], c[1], c[2]))
    .ToList();

var lignesInvalides = File.ReadLines("notes.csv")
   .Skip(1)
    .Where(ligne => Parser(ligne) is null)
    .Select(ligne => ligne.Split(','));
foreach (var c in lignesInvalides)
    Console.Error.WriteLine(c.Length >= 4
        ? $"La ligne de {c[1]} en {c[2]} ({c[3]}) est écartée."
        : $"La ligne « {string.Join(',', c)} » est écartée.");


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