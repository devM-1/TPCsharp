using System.Diagnostics;

public static class DemoAsync
{
    public static async Task ExecuterAsync()
    {
        // 14. Trois appels attendus l'un après l'autre : chacun attend la fin du précédent
        var chrono = Stopwatch.StartNew();
        string a = await OperationLenteAsync("A");
        string b = await OperationLenteAsync("B");
        string c = await OperationLenteAsync("C");
        Console.WriteLine($"À la suite : {a}, {b}, {c} en {chrono.Elapsed.TotalSeconds:F2} s");

        // 15. On lance les trois appels d'abord, puis on attend qu'ils soient tous terminés
        chrono.Restart();
        Task<string> tacheA = OperationLenteAsync("A");
        Task<string> tacheB = OperationLenteAsync("B");
        Task<string> tacheC = OperationLenteAsync("C");
        string[] resultats = await Task.WhenAll(tacheA, tacheB, tacheC);
        Console.WriteLine($"Avec Task.WhenAll : {string.Join(", ", resultats)} en {chrono.Elapsed.TotalSeconds:F2} s");

        // 16. Expérience avec .Result : à supprimer une fois le résultat noté (interdit dans le rendu)
        chrono.Restart();
        string ra = OperationLenteAsync("A").Result;
        string rb = OperationLenteAsync("B").Result;
        string rc = OperationLenteAsync("C").Result;
        Console.WriteLine($"Avec .Result : {ra}, {rb}, {rc} en {chrono.Elapsed.TotalSeconds:F2} s");
    }

    // Simule une opération lente : Task.Delay attend 1 seconde sans bloquer le thread
    static async Task<string> OperationLenteAsync(string nom)
    {
        await Task.Delay(1000);
        return $"{nom} terminée";
    }
}