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