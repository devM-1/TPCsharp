using Microsoft.EntityFrameworkCore; 
  
using (var db = new PromoContext()) 
{ 
    // On n'insère la promo que si la table est vide
    if (!db.Etudiants.Any()) 
    { 
        var promo = Donnees.Promo(); 
        db.Etudiants.AddRange(promo); 
        Console.WriteLine($"Avant : {db.Entry(promo[0]).State}"); 
        // SaveChanges écrit en base tout ce qui a été ajouté, et renvoie le nombre de lignes écrites
        int n = db.SaveChanges();
        Console.WriteLine($"{n} lignes écrites"); 
        Console.WriteLine($"Après : {db.Entry(promo[0]).State}, Id = {promo[0].Id}"); 
    } 
}

using (var db = new PromoContext()) 
{ 
    //9. les etudiants de M2 triés par nom
    var m2 = db.Etudiants
        .Where(e => e.Promo == "M2")
        .OrderBy(e => e.Nom);
    foreach (var e in m2) 
        Console.WriteLine($"{e.Nom} {e.Prenom} ");
    Console.WriteLine($"SQL : {m2.ToQueryString()}"); 

    //10. une projection : le nom et le nombre de notes de chaque étudiant
    var nbNotes = db.Etudiants
        .Select(e => new { e.Nom, NbNotes = e.Notes.Count }) ; 
    foreach (var e in nbNotes.ToList()) 
        Console.WriteLine($"{e.Nom} a {e.NbNotes} notes");
    
    Console.WriteLine($"SQL : {nbNotes.ToQueryString()}"); 

    //11. la moyenne de chaque matière, calculée sur la base
    var parMatiere = db.Notes
        .GroupBy(n => n.Matiere)
        .Select(g => new { Matiere = g.Key, Moyenne = g.Average(n => n.Valeur) })
        .OrderBy(m => m.Matiere);
    foreach (var m in parMatiere.ToList()) 
        Console.WriteLine($"{m.Matiere} : {m.Moyenne}");
    Console.WriteLine($"SQL : {parMatiere.ToQueryString()}"); 

    //12. Sans Include
    Console.WriteLine("Sans Include :");
    foreach (var e in db.Etudiants.ToList()) 
        Console.WriteLine($"{e.Nom} a {e.Notes.Count} notes");

    // Avec Include
    Console.WriteLine("Avec Include :");
    foreach (var e in db.Etudiants.Include(e => e.Notes).ToList()) 
        Console.WriteLine($"{e.Nom} a {e.Notes.Count} notes");

    //13. la moyenne pondérée des etudiants qui ont au moins une note, de la meilleure à la moins bonne
    var moyennes = db.Etudiants
        .Where(e => e.Notes.Any())
        .Select(e => new 
        { 
            e.Nom, 
            Moyenne = e.Notes.Sum(n => n.Valeur * n.Coef) / e.Notes.Sum(n => n.Coef) 
            })
        .OrderByDescending(p => p.Moyenne);  
    Console.WriteLine("Moyennes pondérées :");
    foreach (var m in moyennes) 
        Console.WriteLine($"{m.Nom} : {m.Moyenne:F2}");

    Console.WriteLine($"SQL : {moyennes.ToQueryString()}");

}


using (var db = new PromoContext())
{
    // 14. Alan passe en M2 : on change la propriété, et le contexte repère tout seul la modification
    var alan = db.Etudiants.FirstOrDefault(e => e.Nom == "Turing");
    if (alan is not null)
    {
        alan.Promo = "M2";
        Console.WriteLine($"État d'Alan avant SaveChanges : {db.Entry(alan).State}");
        db.SaveChanges();
    }

    // 15. Suppression de Linus : la base supprime aussi ses notes (cascade)
    var linus = db.Etudiants.FirstOrDefault(e => e.Nom == "Torvalds");
    if (linus is not null)
    {
        db.Etudiants.Remove(linus);
        db.SaveChanges();
    }
    Console.WriteLine($"Notes restantes : {db.Notes.Count()}");

    // 16. On charge d'abord la note de Réseau d'Ada, pour voir ce que devient un objet déjà en mémoire
    var reseauAda = db.Notes.FirstOrDefault(n => n.Matiere == "Reseau" && n.Etudiant.Prenom == "Ada");

    // ExecuteUpdate envoie directement un UPDATE à la base : rien n'est chargé, et pas besoin de SaveChanges
    int nb = db.Notes.Where(n => n.Matiere == "Reseau")
                     .ExecuteUpdate(s => s.SetProperty(n => n.Coef, 2));
    Console.WriteLine($"{nb} notes de Réseau modifiées");

    if (reseauAda is not null)
    {
        // L'objet déjà chargé n'a pas été prévenu : il garde l'ancien coefficient jusqu'à ce qu'on le relise
        Console.WriteLine($"Coef de la note d'Ada en mémoire : {reseauAda.Coef}");
        db.Entry(reseauAda).Reload();
        Console.WriteLine($"Après Reload : {reseauAda.Coef}");
    }
}