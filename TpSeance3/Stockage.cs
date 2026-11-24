using System.Text.Json; 
  
public static class Stockage 
{ 
    // Options partagées par toutes les sauvegardes : conventions du Web (camelCase) et JSON indenté
    private static readonly JsonSerializerOptions Options = 
        new(JsonSerializerDefaults.Web) { WriteIndented = true }; 
        
    // Transforme la liste en texte JSON, puis l'écrit dans le fichier
    public static void Sauver(string chemin, List<Note> notes) => 
    File.WriteAllText(chemin, JsonSerializer.Serialize(notes, Options)); 

    // Lit le fichier puis reconstruit la liste. Si le fichier contient seulement « null », on le considère comme invalide
    public static List<Note> Charger(string chemin) => 
    JsonSerializer.Deserialize<List<Note>>(File.ReadAllText(chemin), Options) ?? new List<Note>(); 
}