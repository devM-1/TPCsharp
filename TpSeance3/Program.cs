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

await DemoAsync.ExecuterAsync();

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
            Console.Error.WriteLine($"promo.json est abîmé : {e.Message}");
            Console.Error.WriteLine("On repart de notes.csv.");
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.Error.WriteLine($"promo.json contient une valeur invalide : {e.ParamName} = {e.ActualValue}.");
            Console.Error.WriteLine("On repart de notes.csv.");
        }
    }

    var depuisCsv = Lecture.LireNotes(cheminCsv);
    await Stockage.SauverAsync(cheminJson, depuisCsv);
    Console.WriteLine("Promo importée depuis notes.csv et enregistrée dans promo.json.");
    return depuisCsv;
}