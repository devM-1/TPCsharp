# TP — Introduction à C# : réponses

## 1. Premier projet

### 1.2
Le projet vise .NET 10 (net10.0), et les options Nullable et ImplicitUsings sont activées. Avec Nullable, le compilateur signale les valeurs qui peuvent être null. ImplicitUsings ajoute automatiquement les using courants : c'est pour cela qu'on peut utiliser Console sans écrire using System;.

### 1.4
Le .dll contient du code intermédiaire (IL), pas du code machine. C'est l'équivalent d'un .jar en Java : la CLR l'exécute comme la JVM exécute le bytecode, en le compilant pendant l'exécution (JIT).

### 1.5
Un fichier seul s'exécute sans .csproj : .NET crée un projet virtuel et compile dans un dossier temporaire, donc aucun dossier bin/ ni obj/ n'apparaît. C'est l'équivalent de java Hello.java, pratique pour un petit script.

## 2. Types, chaînes et null

### 2.1
**c1.X et s1.X.** On obtient c1.X = 9 et s1.X = 1. Une classe est un type référence : c2 = c1 copie la référence, donc les deux variables désignent le même objet. Une structure est un type valeur : s2 = s1 copie les données, donc s2 est indépendante de s1.

**a == b.** En Java, == compare les références et donnerait false, car string.Concat crée un nouvel objet. En C#, string redéfinit == pour comparer le contenu, d'où True.

**Suffixe m.** Il désigne un decimal, stocké en base 10 : 0.1m + 0.2m vaut exactement 0.3m. Un double est stocké en base 2, ce qui provoque une erreur d'arrondi, d'où False. On choisit decimal pour les montants (banque, facturation) et double pour les calculs scientifiques.

### 2.2
**Avertissements.** On obtient CS8600, car on range un string? dans un string, et CS8602, car on lit saisie.Length alors que saisie peut valoir null.

**ReadLine() et null.** ReadLine() renvoie null quand l'entrée standard est fermée, c'est-à-dire quand il n'y a plus rien à lire : Ctrl+D sous Linux et macOS, Ctrl+Z sous Windows, ou fin d'un fichier redirigé. Une ligne vide donne "", pas null.

**TreatWarningsAsErrors.** Les avertissements deviennent des erreurs : si un problème de nullabilité réapparaît, le projet ne compile plus.

## 3. Contrôle et collections

### 3.4
dico["Inconnu"] lève une KeyNotFoundException, alors qu'en Java, get renverrait null. Avec TryGetValue, on obtient simplement false, sans exception, si la clé n'existe pas.

## 4. Classe Etudiant et propriétés

### 4.1
**override.** On redéfinit ToString avec override. Sans ce mot-clé, la méthode masque celle d'object au lieu de la redéfinir : on obtient l'avertissement CS0114, transformé en erreur ici. Sans TreatWarningsAsErrors, le programme compilerait, mais Console.WriteLine(ada) afficherait Ecole.Etudiant.

**ada._notes.** On obtient l'erreur CS0122 : le champ est private, donc inaccessible en dehors de la classe.

**Liste publique.** Exposer directement la List<double> permettrait d'ajouter ou de supprimer des notes sans passer par AjouterNote, donc sans contrôle des bornes. On propose public IReadOnlyList<double> Notes => _notes.AsReadOnly();, qui permet de lire les notes sans pouvoir les modifier.

### 4.2
Sans public, la classe est internal par défaut. Le programme compile toujours, car Program.cs est dans le même projet, mais un projet de tests séparé ne pourrait pas utiliser Promo.

## 5. Héritage et interfaces : les formes

### 5.1
Dériver une classe de Carre provoque l'erreur CS0509 : une classe sealed ne peut pas être héritée. C'est l'équivalent de final en Java.

### 5.2
Les formes s'affichent bien par aire croissante, car Sort() utilise CompareTo, qui compare les aires. Le texte des cercles contient le rayon, car Decrire est virtual et redéfinie dans Cercle. On obtient une aire totale de 41,78, et la forme de plus grand périmètre est le cercle de rayon 2,5 (15,71).

### 5.3
**Sans virtual.** On obtient l'erreur CS0506 : on ne peut redéfinir qu'une méthode virtual, abstract ou override.

**new ou override.** Avec new, c.Decrire() affiche le rayon mais pas f.Decrire(), alors que les deux variables désignent le même objet : la méthode est choisie selon le type déclaré de la variable (masquage). Avec override, elle est choisie selon le type réel de l'objet (redéfinition), et les deux lignes affichent le rayon. En Java, les méthodes sont virtuelles par défaut : les deux lignes afficheraient aussi le rayon.

## 6. Records et exceptions : un fichier de notes

### 6.1
Deux records identiques sont égaux (True), car un record compare ses valeurs. Deux objets Etudiant avec les mêmes données ne le sont pas (False), car pour une classe, == compare les références. with crée une copie modifiée sans changer l'original.

### 6.6
On place le calcul dans Etudiant, puisque c'est cette classe qui possède les notes. Il faut y conserver le coefficient de chaque note : on ajoute un paramètre coef à AjouterNote, et une méthode MoyennePonderee() qui fait le calcul.


# TP — C# séance 2 : LINQ — réponses

## 0. Mise en place

### 0.2
Avec Where(n => n is not null), le résultat reste de type IEnumerable<Note?> : Where retire des éléments mais ne change pas leur type, donc le compilateur considère toujours que chaque note peut être null. Avec OfType<Note>(), on obtient un IEnumerable<Note>. Les null sont écartés et le type n'est plus nullable, ce qui permet d'utiliser les notes sans avertissement ni vérification supplémentaire.

## 1. Délégués et lambdas

### 1.1
Avec les trois critères, on obtient 4 notes d'Algo, 1 note inférieure à 10 (Alan en Algo) et 2 notes d'Ada supérieures ou égales à 15.

La lambda stockée dans une variable est de type Func<Note, bool> : elle reçoit une Note et renvoie un booléen. On ne peut pas écrire var c = n => n.Valeur >= 15; : le compilateur affiche l'erreur CS8917, car il ne connaît pas le type de n et ne peut donc pas déduire le type de la lambda. Il faut préciser soit le type de la variable (Func<Note, bool>), soit celui du paramètre ((Note n) => n.Valeur >= 15).

### 1.3
On obtient 10 puis 6. La lambda capture la variable seuil elle-même, et non sa valeur au moment de sa création : elle relit seuil à chaque appel, donc le passage à 15 est pris en compte.

En Java, ce code ne compile pas : une lambda ne peut utiliser qu'une variable locale finale ou effectivement finale, et seuil est modifiée après la création de la lambda.

Pour garder le seuil 12, on copie sa valeur dans une variable qui ne change plus (double seuilFige = seuil;), et la lambda utilise cette copie. On peut aussi passer par une méthode comme AuDessusDe : son paramètre est une copie de la valeur, donc la lambda renvoyée ne dépend plus de la variable d'origine.

## 2. Premières requêtes

### 2.1
Les notes d'Algo par valeur décroissante sont Grace 18, Ada 15, Linus 11 et Alan 9. Les matières, sans doublon et triées, sont Algo, BD et Reseau. La seule note sous 10 est celle d'Alan en Algo (9) : le type anonyme s'affiche { Prenom = Alan, Valeur = 9 } et le tuple (Alan, 9).

Avec deux OrderBy successifs, le second refait entièrement le tri : les notes sont triées par valeur uniquement et les matières sont mélangées. Le premier tri ne sert plus qu'à départager les égalités. Avec ThenByDescending puis ThenBy, la valeur et le prénom deviennent des critères secondaires, et le tri par matière est conservé.

Le top 3 est 18 (Grace, Algo), 17 (Grace, BD) et 16 (Ada, BD). Les places 4 à 6 sont 16 (Linus, Reseau), 15 (Ada, Algo) et 15 (Barbara, Reseau). Le tri est stable : à valeur égale, c'est l'ordre du fichier qui départage.

### 2.2
On obtient 13 notes, dont 1 inférieure à 10, et une moyenne simple de 13,77.

Max renvoie seulement la meilleure valeur (18), alors que MaxBy renvoie la note entière qui l'atteint (Grace, Algo, 18, coefficient 3).

La moyenne pondérée de toutes les notes vaut 13,72.

Aggregate et string.Join donnent tous les deux « Ada, Alan, Grace, Linus, Barbara ». On préfère string.Join : il est plus lisible, il construit la chaîne en une seule fois au lieu de créer une chaîne intermédiaire à chaque étape, et il fonctionne aussi sur une liste vide, alors qu'Aggregate sans valeur initiale lève une exception.

### 2.3
FirstOrDefault renvoie null, car Linus n'a pas de note en BD. Avec First, on obtient une InvalidOperationException, car aucune note ne correspond.

Single sur Ada en Algo renvoie sa note de 15. Single sur toutes les notes d'Ada lève une InvalidOperationException : Single exige exactement un élément, et Ada a trois notes.

Il existe bien une note inférieure à 10 (Any renvoie True). Toutes les notes de BD valent au moins 12 (All renvoie True) ; Linus n'a pas de note en BD, il n'est donc pas pris en compte. All renvoie True sur une séquence vide, puisqu'aucun élément ne contredit la condition.

## 3. Regrouper et joindre

### 3.1
Les groupes sont Algo (Ada 15, Alan 9, Grace 18, Linus 11), BD (Ada 16, Alan 12, Grace 17, Barbara 13) et Reseau (Ada 14, Alan 11, Grace 12, Linus 16, Barbara 15). Un groupe est de type IGrouping<string, Note> : il possède une clé (Key, ici la matière) et c'est aussi une séquence qui contient les notes du groupe.

Statistiques par matière, par moyenne décroissante : BD, 4 notes, moyenne 14,50 (min 12, max 17) ; Reseau, 5 notes, moyenne 13,60 (min 11, max 16) ; Algo, 4 notes, moyenne 13,25 (min 9, max 18). Les majors sont Grace en Algo et en BD, et Linus en Reseau.

Moyennes pondérées : Grace 16,67, Ada 15,17, Barbara 13,67, Linus 12,25 et Alan 10,33.

CountBy et GroupBy suivi de Count donnent le même résultat (Ada 3, Alan 3, Grace 3, Linus 2, Barbara 2). CountBy est plus court et plus efficace : il se contente de compter, alors que GroupBy construit d'abord des groupes qui gardent toutes les notes en mémoire.

### 3.2
Moyenne simple par promo : 13,00 pour M1 et 15,00 pour M2.

Margaret Hamilton n'apparaît pas dans le relevé, car Join est une jointure interne : il ne garde que les inscrits qui ont au moins une note correspondante, et Margaret n'en a aucune. On la retrouve comme seule inscrite sans note avec Where et Any, avec GroupJoin (sa liste de notes est vide) ou avec LeftJoin (sa note associée vaut null).

L'index construit avec ToLookup donne les trois notes de Grace : Algo 18, BD 17 et Reseau 12. Pour un prénom absent, il renvoie une séquence vide au lieu de lever une exception comme un dictionnaire.

### 3.3
SelectMany renvoie une seule séquence à plat de 13 valeurs, de type IEnumerable<double>. Avec Select à la place, on obtiendrait une séquence de 5 listes, de type IEnumerable<List<double>>, soit une liste par bulletin. La seconde forme de SelectMany garde le prénom à côté de chaque valeur et produit 13 paires, de (Ada, 15) à (Barbara, 15).

## 4. L'évaluation différée

### 4.1
Le programme affiche 6, puis 7, puis 8 / 7. La ligne Where ne calcule rien : elle prépare seulement la requête. Le premier Count() l'exécute sur les 13 notes de départ et trouve 6 notes d'au moins 15. Après l'ajout de la note de Margaret en Algo (19), le deuxième Count() relance la requête sur la liste modifiée et trouve 7 notes. ToList() exécute la requête immédiatement et fige le résultat : figees garde ses 7 notes, alors que bonnes, recalculée après l'ajout de la note de BD (20), en trouve 8.

### 4.2
En appelant Count() deux fois sur la requête, chaque note est testée deux fois, soit 26 tests, car la requête est réexécutée à chaque parcours. Avec ToList() d'abord, chaque note n'est testée qu'une fois (13 tests), et les appels suivants lisent simplement la liste.

### 4.3
Avec Count() > 0, les 13 notes sont testées. Avec Any(), une seule note est testée : Any s'arrête dès qu'il trouve un élément qui convient, et la première note (Ada en Algo, 15) convient déjà.

### 4.4
Le résultat est le même dans les deux cas : Ada 15, Barbara 15, Ada 16, Linus 16, Grace 17, Grace 18. Avec Where avant OrderBy, seules les 6 notes gardées passent par le tri. Avec OrderBy avant Where, les 13 notes sont triées avant d'être filtrées. Il vaut donc mieux filtrer avant de trier.

## 5. Réécrire la classe Promo

### 5.1
On obtient les étudiants Ada, Alan, Grace, Linus et Barbara, une moyenne générale de 13,62 et Grace comme major. Le nombre de notes au moins égales à 10 est de 3 en Algo, 4 en BD et 5 en Reseau.

Pour un étudiant sans aucune note, les deux Sum renvoient 0. La division 0 / 0 entre nombres décimaux ne lève pas d'exception : elle donne NaN (« pas un nombre »), qui s'afficherait comme moyenne. On l'évite en vérifiant d'abord que l'étudiant a au moins une note, et en renvoyant 0 sinon.

### 5.2
Le relevé donne, dans l'ordre du classement : 1. Grace 16,67 Très bien ; 2. Ada 15,17 Bien ; 3. Barbara 13,67 Assez bien ; 4. Linus 12,25 Assez bien ; 5. Alan 10,33 Passable.

### 5.3
La version LINQ est nettement plus courte : une trentaine de lignes, contre une soixantaine pour la classe Promo de la séance 1, et la plupart des méthodes tiennent en une ou deux lignes. Elle est aussi plus lisible, car chaque méthode décrit ce qu'on veut obtenir (filtrer, regrouper, trier) plutôt que la façon de le calculer avec des boucles et des variables intermédiaires. Elle est plus facile à modifier : changer un critère revient à changer une lambda. En revanche, il faut faire attention à l'évaluation différée, et le débogage pas à pas est moins direct qu'avec une boucle.

### 5.4
Oui, Classement() renvoie une requête différée, qui est recalculée à chaque parcours, et chaque calcul refait la moyenne de chaque étudiant en reparcourant toutes les notes. Ce n'est pas un problème ici : il n'y a que 13 notes et 5 étudiants, et comme la liste des notes est privée et ne change jamais, le résultat est toujours le même. Il faudrait mettre le classement en cache, par exemple avec ToList() dans un champ calculé une seule fois, si la promo contenait beaucoup d'étudiants et de notes, ou si le classement était demandé très souvent, comme à chaque affichage d'une page. Il faudrait alors penser à recalculer ce cache si les notes pouvaient changer.

# TP — C# séance 3 : réponses

## 1. Exceptions

**Pourquoi intercepter FormatException et ArgumentOutOfRangeException plutôt qu'Exception.** On n'intercepte que les erreurs qu'on attend et qu'on sait traiter : une ligne mal formée ou une note hors limites. Dans ces deux cas, on sait quoi faire : signaler la ligne et passer à la suivante. Intercepter Exception attraperait aussi les erreurs imprévues, comme un bug (NullReferenceException) ou un problème d'accès au fichier, qui seraient alors présentées à tort comme une simple ligne invalide et passeraient inaperçues. Intercepter des types précis permet aussi d'afficher un message adapté à chaque cas.

**TryParse ou Parse dans un try.** On préfère TryParse quand l'échec est un cas normal et prévisible, comme une donnée saisie par un utilisateur ou lue dans un fichier. Parse dans un try convient quand l'échec est vraiment exceptionnel. Dans ParserStrict, une note non numérique dans le CSV est un cas attendu : on utilise donc TryParse, qui évite le coût d'une exception et permet de lever ensuite notre propre FormatException avec un message précis (numéro de ligne et raison), au lieu du message générique de double.Parse.

**throw; et throw e;.** throw; relance l'exception en conservant sa pile d'appels d'origine : on voit toujours qu'elle a été levée dans le constructeur de Note. throw e; relance la même exception, mais remet sa pile d'appels à zéro à l'endroit du catch : la ligne d'origine disparaît, ce qui rend le débogage plus difficile. Il faut donc utiliser throw;, et l'analyseur du compilateur signale d'ailleurs throw e; par l'avertissement CA2200.

## 2. Sauvegarder la promo en JSON

**Noms des propriétés.** Dans promo.json, les noms des propriétés s'écrivent en camelCase (nom, prenom, matiere, valeur, coef), alors qu'ils commencent par une majuscule en C#. C'est l'effet de JsonSerializerDefaults.Web, qui applique les conventions du Web : en JavaScript et dans la plupart des API, les noms de propriétés JSON commencent par une minuscule.

**promo.json abîmé.** Quand on supprime une accolade, le fichier n'est plus un JSON valide : Deserialize lève une JsonException, dont le message indique la ligne et la position de l'erreur. On l'intercepte, on affiche un message clair, puis le programme repart de notes.csv et réécrit un promo.json correct.

**Note à 30.** Le JSON reste valide, mais le désérialiseur crée chaque note en appelant le constructeur du record Note, qui vérifie la valeur : il lève donc une ArgumentOutOfRangeException. C'est le comportement souhaité, car une note invalide ne peut jamais entrer dans le programme, quelle que soit sa provenance. Il faut seulement intercepter cette exception, comme pour un fichier abîmé, pour que le programme ne s'arrête pas.

**Quand Deserialize renvoie null.** Deserialize renvoie null quand le fichier contient simplement le mot null : c'est un JSON valide, mais il ne représente aucune liste. Dans Charger, on traite ce cas comme un fichier invalide : l'opérateur ?? lève une JsonException, et le programme repart de notes.csv comme pour un fichier abîmé. On évite ainsi de travailler ensuite avec une liste null.

**Path.Combine.** Le séparateur de dossiers n'est pas le même selon le système : \ sous Windows, / sous Linux et macOS. Un chemin écrit à la main pour Windows ne fonctionnerait donc pas dans un conteneur Docker, qui tourne sous Linux. Path.Combine assemble les morceaux avec le bon séparateur pour le système où tourne le programme, et gère les séparateurs en trop ou manquants.

## 3. Passer en asynchrone

**Await oublié.** En supprimant le await devant Stockage.SauverAsync, le compilateur affiche l'avertissement CS4014 : l'appel n'est pas attendu, donc la méthode continue avant que la sauvegarde soit terminée. À l'exécution, le programme afficherait que la promo est enregistrée alors que l'écriture est peut-être encore en cours, et il pourrait même se terminer avant la fin de l'écriture, en laissant un promo.json incomplet. Une erreur pendant la sauvegarde serait aussi perdue, puisque personne n'attend la tâche.

**Task.WhenAll.** Les trois appels attendus l'un après l'autre prennent environ 3 secondes, car chacun attend la fin du précédent. Lancés d'abord puis attendus ensemble avec Task.WhenAll, ils prennent environ 1 seconde, car les trois attentes se déroulent en même temps.

**.Result.** Avec .Result, le programme fonctionne et prend environ 3 secondes. C'est tout de même déconseillé : .Result bloque le thread pendant toute l'attente au lieu de le libérer comme await, ce qui gaspille des ressources et fige une application graphique. Dans certains contextes, comme une application graphique ou l'ancien ASP.NET, il peut même provoquer un interblocage où le programme reste figé pour toujours. Enfin, en cas d'erreur, l'exception arrive enveloppée dans une AggregateException au lieu de l'exception d'origine.