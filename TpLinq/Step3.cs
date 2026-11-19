public static class Step3
{
    public static void Executer(List<Note> notes, List<Inscrit> inscrits)
    {
        Console.WriteLine("=== Step 3 ===");

        // 3.1.1 On range les notes dans des groupes, un groupe par matière
        // Chaque groupe est un IGrouping<string, Note> : une clé (la matière) et les notes qui vont avec
        var parMatiere = notes.GroupBy(n => n.Matiere);
        Console.WriteLine("Notes regroupées par matière :");
        foreach (var groupe in parMatiere)
            Console.WriteLine($"  {groupe.Key} : {string.Join(", ", groupe.Select(n => $"{n.Prenom} {n.Valeur}"))}");

        // 3.1.2 Pour chaque groupe, on calcule une ligne de statistiques, puis on trie par moyenne décroissante
        var stats = notes
            .GroupBy(n => n.Matiere)
            .Select(g => new
            {
                Matiere = g.Key,
                Nombre = g.Count(),
                Moyenne = g.Average(n => n.Valeur),
                Min = g.Min(n => n.Valeur),
                Max = g.Max(n => n.Valeur),
                // Un groupe n'est jamais vide, donc MaxBy trouve toujours une note : le ! le dit au compilateur
                Major = g.MaxBy(n => n.Valeur)!.Prenom
            })
            .OrderByDescending(s => s.Moyenne);
        Console.WriteLine("Statistiques par matière :");
        foreach (var s in stats)
            Console.WriteLine($"  {s.Matiere} : {s.Nombre} notes, moyenne {s.Moyenne:F2}, min {s.Min}, max {s.Max}, major {s.Major}");

        // 3.1.3 Un groupe par étudiant, puis un dictionnaire prénom -> moyenne pondérée
        Dictionary<string, double> moyennes = notes
            .GroupBy(n => n.Prenom)
            .ToDictionary(g => g.Key, g => g.Sum(n => n.Valeur * n.Coef) / g.Sum(n => n.Coef));
        Console.WriteLine("Moyennes pondérées par étudiant :");
        foreach (var (prenom, moyenne) in moyennes.OrderByDescending(p => p.Value))
            Console.WriteLine($"  {prenom} : {moyenne:F2}");

        // 3.1.4 CountBy compte directement le nombre de notes par prénom
        var parCountBy = notes.CountBy(n => n.Prenom);
        Console.WriteLine($"Nombre de notes avec CountBy : {string.Join(", ", parCountBy.Select(p => $"{p.Key} {p.Value}"))}");

        // Même résultat avec GroupBy, mais en construisant d'abord les groupes complets
        var parGroupBy = notes
            .GroupBy(n => n.Prenom)
            .Select(g => new { Prenom = g.Key, Nombre = g.Count() });
        Console.WriteLine($"Nombre de notes avec GroupBy : {string.Join(", ", parGroupBy.Select(p => $"{p.Prenom} {p.Nombre}"))}");

        // 3.2.1 On associe chaque note à son inscrit quand le nom et le prénom sont les mêmes
        // La clé est un type anonyme : deux clés sont égales si elles ont le même nom et le même prénom
        var releve = notes
            .Join(inscrits,
                n => new { n.Nom, n.Prenom },
                i => new { i.Nom, i.Prenom },
                (n, i) => new { n.Prenom, i.Promo, n.Matiere, n.Valeur })
            .ToList();
        Console.WriteLine("Relevé :");
        foreach (var r in releve)
            Console.WriteLine($"  {r.Prenom} · {r.Promo} · {r.Matiere} · {r.Valeur}");

        // 3.2.2 On regroupe les lignes du relevé par promo, puis on fait la moyenne de chaque groupe
        var moyennesPromo = releve
            .GroupBy(r => r.Promo)
            .Select(g => new { Promo = g.Key, Moyenne = g.Average(r => r.Valeur) });
        Console.WriteLine("Moyenne par promo :");
        foreach (var p in moyennesPromo)
            Console.WriteLine($"  {p.Promo} : {p.Moyenne:F2}");

        // 3.2.3 Première façon : on garde les inscrits pour qui aucune note ne correspond
        var sansNoteAny = inscrits
            .Where(i => !notes.Any(n => n.Nom == i.Nom && n.Prenom == i.Prenom))
            .Select(i => i.Prenom);
        Console.WriteLine($"Inscrits sans note (Where + Any) : {string.Join(", ", sansNoteAny)}");

        // Deuxième façon : GroupJoin donne à chaque inscrit la liste de ses notes, qui peut être vide
        var sansNoteGroupJoin = inscrits
            .GroupJoin(notes,
                i => new { i.Nom, i.Prenom },
                n => new { n.Nom, n.Prenom },
                (i, sesNotes) => new { i.Prenom, NbNotes = sesNotes.Count() })
            .Where(x => x.NbNotes == 0)
            .Select(x => x.Prenom);
        Console.WriteLine($"Inscrits sans note (GroupJoin) : {string.Join(", ", sansNoteGroupJoin)}");

        // Troisième façon (.NET 10) : LeftJoin garde chaque inscrit, avec une note à null s'il n'en a aucune
        var sansNoteLeftJoin = inscrits
            .LeftJoin(notes,
                i => new { i.Nom, i.Prenom },
                n => new { n.Nom, n.Prenom },
                (i, n) => new { i.Prenom, Note = n })
            .Where(x => x.Note is null)
            .Select(x => x.Prenom);
        Console.WriteLine($"Inscrits sans note (LeftJoin) : {string.Join(", ", sansNoteLeftJoin)}");

        // 3.2.4 ToLookup construit un index : pour chaque prénom, toutes ses notes
        ILookup<string, Note> index = notes.ToLookup(n => n.Prenom);
        Console.WriteLine($"Notes de Grace : {string.Join(", ", index["Grace"].Select(n => $"{n.Matiere} {n.Valeur}"))}");
        // Pour un prénom absent, l'index renvoie une séquence vide, sans exception
        Console.WriteLine($"Notes de Margaret dans l'index : {index["Margaret"].Count()}");

        // 3.3.1 Un bulletin par étudiant : son prénom et la liste de ses valeurs
        List<Bulletin> bulletins = notes
            .GroupBy(n => n.Prenom)
            .Select(g => new Bulletin(g.Key, g.Select(n => n.Valeur).ToList()))
            .ToList();
        Console.WriteLine("Bulletins :");
        foreach (var b in bulletins)
            Console.WriteLine($"  {b.Prenom} : {string.Join(", ", b.Valeurs)}");

        // 3.3.2 SelectMany met toutes les listes bout à bout : une seule séquence de valeurs
        IEnumerable<double> aPlat = bulletins.SelectMany(b => b.Valeurs);
        Console.WriteLine($"Avec SelectMany : {aPlat.Count()} valeurs -> {string.Join(", ", aPlat)}");

        // Select garde une liste par bulletin : on obtient une séquence de listes
        IEnumerable<List<double>> imbrique = bulletins.Select(b => b.Valeurs);
        Console.WriteLine($"Avec Select : {imbrique.Count()} listes");

        // 3.3.3 La seconde forme de SelectMany garde le bulletin à côté de chaque valeur
        var paires = bulletins.SelectMany(b => b.Valeurs, (b, v) => (b.Prenom, v));
        Console.WriteLine($"Paires (prénom, valeur) : {string.Join(", ", paires)}");
    }
}