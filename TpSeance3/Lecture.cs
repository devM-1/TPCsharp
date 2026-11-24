using System.Globalization;

public static class Lecture
{
    // Transforme une ligne en Note, ou lève une FormatException qui explique pourquoi c'est impossible
    public static Note ParserStrict(string ligne, int numero)
    {
        var c = ligne.Split(',');
        if (c.Length != 5)
            throw new FormatException($"Ligne {numero} : 5 champs attendus, {c.Length} trouvés.");
        if (!double.TryParse(c[3], CultureInfo.InvariantCulture, out var valeur))
            throw new FormatException($"Ligne {numero} : la note « {c[3]} » n'est pas un nombre.");
        if (!int.TryParse(c[4], CultureInfo.InvariantCulture, out var coef))
            throw new FormatException($"Ligne {numero} : le coefficient « {c[4]} » n'est pas un entier.");
        // Si la note sort de 0 à 20, c'est le record qui lève l'ArgumentOutOfRangeException : on la laisse remonter
        return new Note(c[0], c[1], c[2], valeur, coef);
    }

    // Lit toutes les notes : les lignes valides sont gardées, les autres sont signalées puis ignorées
    public static List<Note> LireNotes(string chemin)
    {
        var notes = new List<Note>();
        int numero = 1;
        foreach (var ligne in File.ReadLines(chemin).Skip(1))
        {
            numero++;
            try
            {
                notes.Add(ParserStrict(ligne, numero));
            }
            catch (FormatException e)
            {
                Console.Error.WriteLine(e.Message);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Error.WriteLine($"Ligne {numero} : {e.ParamName} hors limites ({e.ActualValue}).");
            }
        }
        return notes;
    }

    public static List<Inscrit> LireInscrits(string chemin) => File.ReadLines(chemin)
        .Skip(1)
        .Select(ligne => ligne.Split(','))
        .Select(c => new Inscrit(c[0], c[1], c[2], c[3]))
        .ToList();
}