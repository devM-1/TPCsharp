using System.Globalization;

// On saute l'en-tête, on transforme chaque ligne en Note, et OfType écarte les lignes invalides (null)
List<Note> notes = File.ReadLines("notes.csv")
    .Skip(1)
    .Select(Parser)
    .OfType<Note>()
    .ToList();

// Chaque ligne est découpée pour créer un Inscrit
List<Inscrit> inscrits = File.ReadLines("etudiants.csv")
    .Skip(1)
    .Select(ligne => ligne.Split(','))
    .Select(c => new Inscrit(c[0], c[1], c[2]))
    .ToList();

// On signale chaque ligne écartée avec le prénom, la matière et la note fautive
var lignesInvalides = File.ReadLines("notes.csv")
    .Skip(1)
    .Where(ligne => Parser(ligne) is null)
    .Select(ligne => ligne.Split(','));
foreach (var c in lignesInvalides)
    Console.Error.WriteLine(c.Length >= 4
        ? $"La ligne de {c[1]} en {c[2]} ({c[3]}) est écartée."
        : $"La ligne « {string.Join(',', c)} » est écartée.");

Console.WriteLine($"{notes.Count} notes valides et {inscrits.Count} inscrits.");

// Chaque exercice est dans son propre fichier
Step1.Executer(notes);
Step2.Executer(notes);
Step3.Executer(notes, inscrits);
Step4.Executer(notes);
Step5.Executer(notes);

// Transforme une ligne en Note, ou renvoie null si la ligne est invalide
static Note? Parser(string ligne)
{
    var c = ligne.Split(',');
    if (c.Length != 5) return null;
    if (!double.TryParse(c[3], CultureInfo.InvariantCulture, out var v)) return null;
    if (v < 0 || v > 20) return null;
    if (!int.TryParse(c[4], out var coef)) return null;
    return new Note(c[0], c[1], c[2], v, coef);
}
