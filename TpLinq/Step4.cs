public static class Step4
{
    public static void Executer(List<Note> source)
    {
        Console.WriteLine("=== Exercice 4 : évaluation différée ===");

        // On travaille sur une copie pour ne pas ajouter Margaret aux notes des autres exercices
        var notes = new List<Note>(source);

        // 4.1 Where prépare la requête, chaque Count() l'exécute sur l'état actuel de la liste
        var bonnes = notes.Where(n => n.Valeur >= 15);
        Console.WriteLine(bonnes.Count());
        notes.Add(new Note("Hamilton", "Margaret", "Algo", 19, 3));
        Console.WriteLine(bonnes.Count());
        // ToList fige le résultat au moment de l'appel
        var figees = notes.Where(n => n.Valeur >= 15).ToList();
        notes.Add(new Note("Hamilton", "Margaret", "BD", 20, 2));
        Console.WriteLine(bonnes.Count() + " / " + figees.Count);

        // 4.2 Sans ToList, chaque Count() relance le filtre (26 tests au total) ; avec ToList, un seul passage (13 tests)
        var requete = source.Where(n => n.Valeur >= 15);
        Console.WriteLine($"{requete.Count()} puis {requete.Count()}");
        var liste = source.Where(n => n.Valeur >= 15).ToList();
        Console.WriteLine($"{liste.Count} puis {liste.Count}");

        // 4.3 Any() s'arrête à la première note qui convient (1 seul test ici)
        Console.WriteLine(source.Any(n => n.Valeur >= 15));

        // 4.4 Même résultat dans les deux ordres, mais filtrer d'abord ne trie que 6 notes au lieu de 13
        var whereAvant = source.Where(n => n.Valeur >= 15).OrderBy(n => n.Valeur).ToList();
        var whereApres = source.OrderBy(n => n.Valeur).Where(n => n.Valeur >= 15).ToList();
        Console.WriteLine(whereAvant.SequenceEqual(whereApres));
    }
}
