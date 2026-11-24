using System.Net;
using System.Net.Http.Json;

// Accès à l'API geo.api.gouv.fr
public static class Geo
{
    // Un seul HttpClient pour tout le programme : il réutilise ses connexions d'une requête à l'autre
    private static readonly HttpClient Client = new()
    {
        BaseAddress = new Uri("https://geo.api.gouv.fr/"),
        // HTTP/2 fait passer plusieurs requêtes simultanées sur une seule connexion ;
        // si le serveur ne le connaît pas, le client revient automatiquement à HTTP/1.1
        DefaultRequestVersion = HttpVersion.Version20,
        DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
    };

    // Renvoie les communes d'un code postal : une liste vide s'il n'y en a aucune, jamais null
    public static async Task<List<Commune>> CommunesAsync(string codePostal, CancellationToken jeton = default)
    {
        try
        {
            var communes = await Client.GetFromJsonAsync<List<Commune>>(
                $"communes?codePostal={codePostal}&fields=nom,code,population", jeton);
            return communes ?? [];
        }
        catch (HttpRequestException e) when (e.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.NotFound)
        {
            // Le serveur a répondu, mais il refuse ce code postal : on considère qu'il n'y a aucune commune
            return [];
        }
    }

    // Une ligne de texte pour un inscrit ; une erreur sur cette requête ne concerne que lui
    public static async Task<string> DecrireInscritAsync(Inscrit inscrit, CancellationToken jeton)
    {
        string debut = $"{inscrit.Prenom} {inscrit.Nom} ({inscrit.Promo})";
        try
        {
            List<Commune> communes = await CommunesAsync(inscrit.CodePostal, jeton);
            return $"{debut} : {communes.FirstOrDefault()?.Nom ?? "commune inconnue"}";
        }
        catch (HttpRequestException e)
        {
            return $"{debut} : commune inconnue (erreur réseau : {e.Message})";
        }
    }
}