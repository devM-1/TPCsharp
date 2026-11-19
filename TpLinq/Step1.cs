public static class Step1
{
    public static void Executer(List<Note> notes)
    {
        Console.WriteLine("=== Delegue & Lambda ===");

        Console.WriteLine($"Notes d'Algo : {Filtrer(notes, EstEnAlgo).Count}");
        Console.WriteLine($"Notes sous 10 : {Filtrer(notes, n => n.Valeur < 10).Count}");

        Func<Note, bool> adaAuMoins15 = n => n.Prenom == "Ada" && n.Valeur >= 15;
        Console.WriteLine($"Notes d'Ada >= 15 : {Filtrer(notes, adaAuMoins15).Count}");

        var auMoins15 = (Note n) => n.Valeur >= 15;
        Console.WriteLine($"Notes >= 15 : {Filtrer(notes, auMoins15).Count}");

        Func<Note, double> pointsPonderes = n => n.Valeur * n.Coef;
        Action<Note> afficher = n => Console.WriteLine($"{n.Prenom} · {n.Matiere} · {n.Valeur}");
        Console.WriteLine($"Points pondérés de la première note : {pointsPonderes(notes[0])}");
        afficher(notes[0]);

        Console.WriteLine($"Notes au-dessus de 14 : {Filtrer(notes, AuDessusDe(14)).Count}");

        var copie = new List<Note>(notes);
        copie.Sort((a, b) => a.Matiere != b.Matiere
            ? string.CompareOrdinal(a.Matiere, b.Matiere)
            : b.Valeur.CompareTo(a.Valeur));
        Console.WriteLine("Copie triée par matière puis valeur décroissante :");
        copie.ForEach(afficher);
        int supprimees = copie.RemoveAll(n => n.Matiere == "Reseau");
        Console.WriteLine($"{supprimees} notes de Réseau supprimées, il en reste {copie.Count}.");

        double seuil = 12;
        Func<Note, bool> admis = n => n.Valeur >= seuil;
        Console.WriteLine($"Admis avec seuil = 12 : {Filtrer(notes, admis).Count}");
        seuil = 15;
        Console.WriteLine($"Admis avec seuil = 15 : {Filtrer(notes, admis).Count}");

        seuil = 12;
        double seuilFige = seuil;
        Func<Note, bool> admisFige = n => n.Valeur >= seuilFige;
        seuil = 15;
        Console.WriteLine($"Admis avec la copie du seuil : {Filtrer(notes, admisFige).Count}");
    }

    public static List<Note> Filtrer(List<Note> source, Func<Note, bool> critere)
    {
        var resultat = new List<Note>();
        foreach (var n in source)
        {
            if (critere(n))
                resultat.Add(n);
        }
        return resultat;
    }

    static bool EstEnAlgo(Note n) => n.Matiere == "Algo";

    static Func<Note, bool> AuDessusDe(double seuil) => n => n.Valeur > seuil;
}