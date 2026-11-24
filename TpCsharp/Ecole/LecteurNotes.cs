using System.Globalization;

namespace Ecole;

// Lecture du fichier de notes au format CSV
public static class LecteurNotes
{
    // Transforme une ligne en Note, ou lève une NoteInvalideException qui explique le problème
    public static Note LireLigne(string ligne, int numero)
    {
        string[] champs = ligne.Split(',');
        if (champs.Length != 5)
            throw new NoteInvalideException(numero, $"5 champs attendus, {champs.Length} trouvés.");
        // InvariantCulture : le séparateur décimal est le point, comme dans le fichier
        if (!double.TryParse(champs[3], CultureInfo.InvariantCulture, out double valeur))
            throw new NoteInvalideException(numero, $"la note « {champs[3]} » n'est pas un nombre.");
        if (valeur is < 0 or > 20)
            throw new NoteInvalideException(numero, $"la note {valeur} est en dehors de [0, 20].");
        if (!int.TryParse(champs[4], CultureInfo.InvariantCulture, out int coef) || coef < 1)
            throw new NoteInvalideException(numero, $"le coefficient « {champs[4]} » n'est pas valide.");
        return new Note(champs[0], champs[1], champs[2], valeur, coef);
    }

    // Lit le fichier ligne par ligne : les lignes invalides sont signalées puis ignorées
    public static List<Note> LireFichier(string chemin)
    {
        List<Note> notes = [];
        // using ferme le fichier automatiquement à la fin de la méthode
        using var lecteur = new StreamReader(chemin);
        // On saute la ligne d'en-tête
        lecteur.ReadLine();
        int numero = 1;
        string? ligne;
        while ((ligne = lecteur.ReadLine()) is not null)
        {
            numero++;
            if (string.IsNullOrWhiteSpace(ligne))
                continue;
            try
            {
                notes.Add(LireLigne(ligne, numero));
            }
            catch (NoteInvalideException e)
            {
                Console.Error.WriteLine($"Ligne {e.Ligne} ignorée : {e.Message}");
            }
        }
        return notes;
    }
}
