// Statistiques sur une liste de notes
List<double> listeNotes = [12, 15.5, 9, 18];

double somme = 0;
double min = listeNotes[0];
double max = listeNotes[0];
foreach (double n in listeNotes)
{
    somme += n;
    if (n < min) min = n;
    if (n > max) max = n;
}
double moyenne = somme / listeNotes.Count;
Console.WriteLine($"Moyenne : {moyenne:F2} ({Bareme.Mention(moyenne)}), min : {min}, max : {max}");

// Notes par étudiant
Dictionary<string, List<double>> dico = new()
{
    ["Ada"] = [15, 16, 14],
    ["Alan"] = [9, 12],
    ["Grace"] = [17, 18.5, 16]
};
dico["Alan"].Add(14);

foreach (var (nom, notes) in dico)
{
    double total = 0;
    foreach (double n in notes)
        total += n;
    double moy = total / notes.Count;
    Console.WriteLine($"{nom} : {notes.Count} notes, moyenne {moy:F2}, {Bareme.Mention(moy)}");
}

// Clé absente
if (dico.TryGetValue("Inconnu", out var notesInconnu))
    Console.WriteLine($"Inconnu a {notesInconnu.Count} notes");
else
    Console.WriteLine("Aucun étudiant nommé Inconnu.");

// Filtrage par motif
object?[] valeurs = [4, 7, "", "bonjour", listeNotes, null, 3.14];
foreach (var v in valeurs)
    Console.WriteLine(Decrire(v));

static string Decrire(object? o) => o switch
{
    null => "null",
    int n when n % 2 == 0 => "entier pair",
    int => "entier impair",
    string { Length: 0 } => "chaîne vide",
    string s => $"chaîne de {s.Length} caractères",
    List<double> l => $"liste de {l.Count} éléments",
    _ => "autre"
};