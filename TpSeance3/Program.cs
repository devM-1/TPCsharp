using System.Text.Json;

// Path.Combine assemble les chemins avec le bon séparateur (\ sous Windows, / sous Linux et dans Docker)
string dossier = Directory.GetCurrentDirectory();
string cheminJson = Path.Combine(dossier, "promo.json");
string cheminCsv = Path.Combine(dossier, "notes.csv");
string cheminInscrits = Path.Combine(dossier, "etudiants.csv");

// await : on attend le chargement sans bloquer le programme
List<Note> notes = await ChargerPromoAsync(cheminJson, cheminCsv);
List<Inscrit> inscrits = Lecture.LireInscrits(cheminInscrits);
Console.WriteLine($"{notes.Count} notes et {inscrits.Count} inscrits.");

// Démonstrations de l'étape 3 (asynchrone) et de l'étape 4 (API)
await DemoAsync.ExecuterAsync();
await DemoGeo.ExecuterAsync(inscrits);

// Charge promo.json s'il existe et qu'il est valide ; sinon importe notes.csv et crée promo.json
static async Task<List<Note>> ChargerPromoAsync(string cheminJson, string cheminCsv)
{
    if (File.Exists(cheminJson))
    {
        try
        {
            var notes = await Stockage.ChargerAsync(cheminJson);
            Console.WriteLine("Promo chargée depuis promo.json.");
            return notes;
        }
        catch (JsonException e)
        {
            // Le fichier n'est pas un JSON correct (accolade manquante, contenu null...)
            Console.Error.WriteLine($"promo.json est abîmé : {e.Message}");
            Console.Error.WriteLine("On repart de notes.csv.");
        }
        catch (ArgumentOutOfRangeException e)
        {
            // Le JSON est correct, mais une note est refusée par le record (par exemple une valeur de 30)
            Console.Error.WriteLine($"promo.json contient une valeur invalide : {e.ParamName} = {e.ActualValue}.");
            Console.Error.WriteLine("On repart de notes.csv.");
        }
    }

    // Pas de JSON utilisable : on lit le CSV, puis on crée (ou on répare) promo.json
    var depuisCsv = Lecture.LireNotes(cheminCsv);
    await Stockage.SauverAsync(cheminJson, depuisCsv);
    Console.WriteLine("Promo importée depuis notes.csv et enregistrée dans promo.json.");
    return depuisCsv;
}
