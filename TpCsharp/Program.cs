using Ecole;

var n1 = new Note("Lovelace", "Ada", "Algo", 15, 3);
var n2 = new Note("Lovelace", "Ada", "Algo", 15, 3);
Console.WriteLine(n1 == n2);
Console.WriteLine(n1);
var n3 = n1 with { Valeur = 18 };
Console.WriteLine(n3);
Console.WriteLine(n1 == n3);

var e1 = new Etudiant("Lovelace", "Ada", 4);
var e2 = new Etudiant("Lovelace", "Ada", 4);
Console.WriteLine(e1 == e2);

var promo = new Promo("M1 Info");
try
{
    List<Note> notes = LecteurNotes.LireFichier("notes.csv");
    foreach (var note in notes)
    {
        Etudiant? etudiant = promo.Chercher(note.Nom);
        if (etudiant is null)
        {
            etudiant = new Etudiant(note.Nom, note.Prenom, 4);
            promo.Inscrire(etudiant);
        }
        etudiant.AjouterNote(note.Valeur, note.Coef);
    }
    promo.AfficherReleve();
}
catch (FileNotFoundException e)
{
    Console.Error.WriteLine($"Le fichier {e.FileName} est introuvable.");
}