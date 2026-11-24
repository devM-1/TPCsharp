using System.Diagnostics;

// Étape 4 : interroger l'API, puis comparer les requêtes en parallèle et l'une après l'autre
public static class DemoGeo
{
    // 24. Délai maximal de chaque série de requêtes (mettre TimeSpan.FromMilliseconds(1) pour tester l'annulation)
    private static readonly TimeSpan DelaiMax = TimeSpan.FromSeconds(3);

    public static async Task ExecuterAsync(List<Inscrit> inscrits)
    {
        Console.WriteLine("=== API geo.api.gouv.fr ===");
        try
        {
            // 18-19. Quelques codes postaux, dont un qui n'existe pas : chaque requête a son propre délai
            foreach (var codePostal in new[] { "35000", "29200", "99999" })
            {
                using var delai = new CancellationTokenSource(DelaiMax);
                List<Commune> communes = await Geo.CommunesAsync(codePostal, delai.Token);
                string texte = communes.Count == 0
                    ? "aucune commune"
                    : string.Join(", ", communes.Select(c => $"{c.Nom} (INSEE {c.Code}, {c.Population:N0} habitants)"));
                Console.WriteLine($"{codePostal} : {texte}");
            }

            // 20-21. Toutes les requêtes lancées en même temps : le délai vaut pour l'ensemble
            var chrono = Stopwatch.StartNew();
            string[] lignes;
            using (var delai = new CancellationTokenSource(DelaiMax))
            {
                lignes = await Task.WhenAll(inscrits.Select(i => Geo.DecrireInscritAsync(i, delai.Token)));
            }
            chrono.Stop();
            foreach (var ligne in lignes)
                Console.WriteLine($"  {ligne}");
            Console.WriteLine($"En parallèle : {chrono.ElapsedMilliseconds} ms");

            // 22. Les mêmes requêtes, l'une après l'autre : chaque requête a son propre délai
            chrono.Restart();
            foreach (var inscrit in inscrits)
            {
                using var delai = new CancellationTokenSource(DelaiMax);
                _ = await Geo.DecrireInscritAsync(inscrit, delai.Token);
            }
            chrono.Stop();
            Console.WriteLine($"L'une après l'autre : {chrono.ElapsedMilliseconds} ms");
        }
        catch (HttpRequestException e)
        {
            // 23. Le serveur est introuvable (mauvaise adresse, pas de réseau...)
            Console.Error.WriteLine($"Impossible de joindre l'API : {e.Message}");
        }
        catch (OperationCanceledException)
        {
            // 24. Le délai est dépassé : les requêtes en cours sont annulées
            Console.Error.WriteLine($"Délai dépassé : l'API n'a pas répondu en moins de {DelaiMax.TotalSeconds} s.");
        }
    }
}