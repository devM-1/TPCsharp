public static class Step4
{
    static int _tests;
    static int _passagesTri;

    public static void Executer(List<Note> source)
    {
        Console.WriteLine("=== Step 4 ===");

        // On travaille sur une copie pour ne pas ajouter Margaret aux notes des autres exercices
        var notes = new List<Note>(source);

        // 4.1 Where ne calcule rien tout de suite : il prépare seulement la requête
        var bonnes = notes.Where(n => n.Valeur >= 15);
        // La requête s'exécute ici, sur les 13 notes de départ
        Console.WriteLine(bonnes.Count());
        notes.Add(new Note("Hamilton", "Margaret", "Algo", 19, 3));
        // La requête s'exécute de nouveau et voit la note ajoutée
        Console.WriteLine(bonnes.Count());
        // ToList exécute la requête tout de suite et garde le résultat dans une liste
        var figees = notes.Where(n => n.Valeur >= 15).ToList();
        notes.Add(new Note("Hamilton", "Margaret", "BD", 20, 2));
        // bonnes est recalculée et voit la seconde note, alors que figees ne change plus
        Console.WriteLine(bonnes.Count() + " / " + figees.Count);

        // 4.2 Sans ToList, chaque Count() relance le filtre sur toutes les notes
        _tests = 0;
        Console.WriteLine("Count() appelé deux fois sur la requête :");
        var tracees = source.Where(EstBonneAvecTrace);
        _ = tracees.Count();
        _ = tracees.Count();
        Console.WriteLine($"  -> {_tests} test(s)");

        // Avec ToList, le filtre ne tourne qu'une fois, puis on lit simplement la liste
        _tests = 0;
        Console.WriteLine("ToList() d'abord, puis Count deux fois sur la liste :");
        var liste = source.Where(EstBonneAvecTrace).ToList();
        _ = liste.Count;
        _ = liste.Count;
        Console.WriteLine($"  -> {_tests} test(s)");

        // 4.3 Count() > 0 doit tout compter, donc tester toutes les notes
        _tests = 0;
        Console.WriteLine("Count() > 0 :");
        _ = source.Where(EstBonneAvecTrace).Count() > 0;
        Console.WriteLine($"  -> {_tests} test(s)");

        // Any() s'arrête dès qu'une note convient
        _tests = 0;
        Console.WriteLine("Any() :");
        _ = source.Where(EstBonneAvecTrace).Any();
        Console.WriteLine($"  -> {_tests} test(s)");

        // 4.4 Where avant OrderBy : seules les notes gardées passent par le tri
        _tests = 0;
        _passagesTri = 0;
        Console.WriteLine("Where puis OrderBy :");
        var whereAvant = source.Where(EstBonneAvecTrace).OrderBy(CleDeTri).ToList();
        Console.WriteLine($"  -> {_tests} test(s), {_passagesTri} notes triées : {Resume(whereAvant)}");

        // OrderBy avant Where : toutes les notes passent par le tri, puis on filtre
        _tests = 0;
        _passagesTri = 0;
        Console.WriteLine("OrderBy puis Where :");
        var whereApres = source.OrderBy(CleDeTri).Where(EstBonneAvecTrace).ToList();
        Console.WriteLine($"  -> {_tests} test(s), {_passagesTri} notes triées : {Resume(whereApres)}");
    }

    // Le filtre affiche la note testée avant de répondre : c'est l'effet de bord de l'expérience
    static bool EstBonneAvecTrace(Note n)
    {
        _tests++;
        Console.WriteLine($"    test de {n.Prenom} en {n.Matiere}");
        return n.Valeur >= 15;
    }

    // Clé de tri qui compte ses appels : elle est appelée une fois pour chaque note qui passe par le tri
    static double CleDeTri(Note n)
    {
        _passagesTri++;
        return n.Valeur;
    }

    static string Resume(IEnumerable<Note> notes) => string.Join(", ", notes.Select(n => $"{n.Prenom} {n.Valeur}"));
}