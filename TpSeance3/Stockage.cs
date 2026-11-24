using System.Text.Json;

public static class Stockage
{
    // Options partagées par toutes les sauvegardes : conventions du Web (camelCase) et JSON indenté
    private static readonly JsonSerializerOptions Options =
        new(JsonSerializerDefaults.Web) { WriteIndented = true };

    // Versions synchrones de l'étape 2
    public static void Sauver(string chemin, List<Note> notes) =>
        File.WriteAllText(chemin, JsonSerializer.Serialize(notes, Options));

    public static List<Note> Charger(string chemin) =>
        JsonSerializer.Deserialize<List<Note>>(File.ReadAllText(chemin), Options)
        ?? throw new JsonException($"{Path.GetFileName(chemin)} ne contient pas de liste de notes.");

    // Versions asynchrones : le programme n'est pas bloqué pendant l'accès au disque
    public static async Task SauverAsync(string chemin, List<Note> notes)
    {
        // await using ferme le fichier à la fin de la méthode, lui aussi sans bloquer
        await using var flux = File.Create(chemin);
        await JsonSerializer.SerializeAsync(flux, notes, Options);
    }

    public static async Task<List<Note>> ChargerAsync(string chemin)
    {
        await using var flux = File.OpenRead(chemin);
        return await JsonSerializer.DeserializeAsync<List<Note>>(flux, Options)
            ?? throw new JsonException($"{Path.GetFileName(chemin)} ne contient pas de liste de notes.");
    }
}