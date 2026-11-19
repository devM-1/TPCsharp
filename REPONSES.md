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