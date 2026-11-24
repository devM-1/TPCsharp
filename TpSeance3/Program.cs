// 1.1 Une note invalide est refusée dès sa création
try
{
    _ = new Note("Lovelace", "Ada", "Algo", 25, 3);
}
catch (ArgumentOutOfRangeException e)
{
    Console.WriteLine($"Note refusée : {e.ParamName} = {e.ActualValue}");
}

try
{
    _ = new Note("Lovelace", "Ada", "Algo", 15, 0);
}
catch (ArgumentOutOfRangeException e)
{
    Console.WriteLine($"Note refusée : {e.ParamName} = {e.ActualValue}");
}

// 1.2 Lecture stricte : chaque ligne rejetée est signalée avec son numéro et sa raison
List<Note> notes = Lecture.LireNotes("notes.csv");
List<Inscrit> inscrits = Lecture.LireInscrits("etudiants.csv");
Console.WriteLine($"{notes.Count} notes valides et {inscrits.Count} inscrits.");

// Expérience throw; ou throw e; : à supprimer une fois les deux piles d'appels comparées
Console.WriteLine("--- Avec throw; ---");
try
{
    Relancer(garderLaPile: true);
}
catch (ArgumentOutOfRangeException e)
{
    Console.WriteLine(e.StackTrace);
}

Console.WriteLine("--- Avec throw e; ---");
try
{
    Relancer(garderLaPile: false);
}
catch (ArgumentOutOfRangeException e)
{
    Console.WriteLine(e.StackTrace);
}

static void Relancer(bool garderLaPile)
{
    try
    {
        _ = new Note("Lovelace", "Ada", "Algo", 25, 3);
    }
    catch (ArgumentOutOfRangeException)
    {
        if (garderLaPile)
            throw;
        throw;
    }
}