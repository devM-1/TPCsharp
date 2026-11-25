# TP C# — ESIEE-IT 2026-2027

TP du cours de programmation C# / .NET 10, réalisés par Maiva Payon.

## Contenu du dépôt

| Dossier ou fichier | Séance | Contenu |
|---|---|---|
| `TpCsharp/` | 1 | Bases du langage, classes `Etudiant` et `Promo`, formes géométriques, records et exceptions |
| `TpCsharp.Tests/` | 1 | Tests unitaires xUnit de `Etudiant` et `Bareme` |
| `TpLinq/` | 2 | Délégués, lambdas et requêtes LINQ |
| `TpSeance3/` | 3 | Exceptions, sauvegarde JSON, asynchrone et API geo.api.gouv.fr |
| `hello.cs` | 1 | Programme d'un seul fichier, sans projet |
| `REPONSES.md` | 1 à 3 | Réponses aux questions des trois séances |
| `Tp.slnx` | — | Solution qui regroupe tous les projets |

## Prérequis

- Le **SDK .NET 10**. Pour vérifier qu'il est installé :
  ```bash
  dotnet --version
  ```
  La commande doit afficher une version `10.0.x`.
- Un **accès Internet** pour l'étape 4 de la séance 3, qui interroge l'API publique geo.api.gouv.fr. Cette API ne demande ni clé ni inscription.

## Compiler et tester tout le dépôt

Depuis la racine du dépôt :
```bash
dotnet build
dotnet test
```
- `dotnet build` compile tous les projets de la solution. Il ne doit afficher **aucun avertissement**, car chaque projet transforme les avertissements en erreurs.
- `dotnet test` lance les tests xUnit. Les **13 tests** doivent réussir.

## Lancer chaque projet

Les programmes lisent leurs fichiers CSV dans le **dossier courant**. Il faut donc toujours se placer dans le dossier du projet avant de faire `dotnet run`.

### Séance 1 : `TpCsharp`
```bash
cd TpCsharp
dotnet run
```
Le programme montre la copie d'une `readonly struct Point2D` et l'égalité entre étudiants dans un `HashSet`, de la partie « Pour aller plus loin ». Les programmes des parties précédentes se trouvent dans l'historique Git.

### Séance 2 : `TpLinq`
```bash
cd TpLinq
dotnet run
```
Le programme lit `notes.csv` et `etudiants.csv`, signale la ligne invalide, puis exécute les exercices 1 à 5, du fichier `Step1.cs` au fichier `Step5.cs`.

### Séance 3 : `TpSeance3`
```bash
cd TpSeance3
dotnet run
```
- **Au premier lancement**, le programme importe `notes.csv` et signale les 3 lignes invalides, puis crée `promo.json`.
- **Aux lancements suivants**, il charge directement `promo.json`.
- **Pour repartir du CSV**, il suffit de supprimer `promo.json`. Le programme repart aussi du CSV, avec un message clair, si `promo.json` est abîmé ou contient une note invalide.
- La démonstration de l'asynchrone dure environ 4 secondes, puis viennent les requêtes à l'API.
- **Sans réseau**, le programme affiche « Impossible de joindre l'API » et se termine normalement, sans planter.

### Programme d'un seul fichier : `hello.cs`
Depuis la racine du dépôt :
```bash
dotnet run hello.cs
```

## Avec Docker, sans installer .NET

Depuis la racine du dépôt, ouvre un terminal dans un conteneur qui contient le SDK .NET 10 :
```bash
# Linux / macOS
docker run --rm -it -v "$PWD":/src -w /src mcr.microsoft.com/dotnet/sdk:10.0 bash

# Windows PowerShell
docker run --rm -it -v "${PWD}:/src" -w /src mcr.microsoft.com/dotnet/sdk:10.0 bash
```
Ensuite, utilise dans ce terminal les mêmes commandes que ci-dessus (`dotnet build`, `dotnet test`, `cd TpLinq`, `dotnet run`…).

## Fichiers générés

Les dossiers `bin/` et `obj/`, ainsi que le fichier `promo.json`, sont produits par la compilation ou par le programme. Ils sont exclus du dépôt par le `.gitignore`.

## 2. Insérer la promo

**Premier lancement.** Avant SaveChanges, Ada est dans l'état Added : AddRange l'a seulement marquée comme à ajouter, et rien n'est encore écrit. SaveChanges écrit 19 lignes (6 étudiants et 13 notes) dans une seule transaction et renvoie ce nombre. Ada passe ensuite à l'état Unchanged, car elle correspond maintenant à ce qui est en base, et son Id vaut 1 : la clé générée par la base a été recopiée dans l'objet. Les notes ont été ajoutées avec leur étudiant, et EF Core a rempli leur EtudiantId tout seul.

**Second lancement.** Rien n'est inséré : les données sont désormais enregistrées dans le fichier promo.db, donc db.Etudiants.Any() renvoie true et le bloc d'insertion est sauté. Le journal ne montre que la requête SELECT EXISTS qui vérifie s'il y a déjà des étudiants.

## 3. Interroger la base

**Résultats.** Les étudiants de M2, triés par nom, sont Hamilton Margaret, Hopper Grace et Liskov Barbara. Le nombre de notes par étudiant est de 3 pour Lovelace, Turing et Hopper, 2 pour Torvalds et Liskov, et 0 pour Hamilton : la base compte elle-même les notes dans une sous-requête. Les moyennes par matière sont 13,25 en Algo, 14,5 en BD et 13,6 en Reseau. Le SQL contient GROUP BY et AVG : le calcul est fait par la base, et seules trois lignes reviennent dans le programme. Les moyennes pondérées sont Hopper 16,67, Lovelace 15,17, Liskov 13,67, Torvalds 12,25 et Turing 10,33.

**Pourquoi 0 note sans Include.** Par défaut, EF Core ne charge pas les propriétés de navigation : la requête ne lit que la table Etudiants, et la liste Notes de chaque étudiant reste vide. Avec Include, EF Core ajoute une jointure (LEFT JOIN) vers la table Notes et remplit les listes en une seule requête. Si l'on chargeait les notes de chaque étudiant dans la boucle, on ferait une requête pour la liste des étudiants, puis une requête par étudiant : c'est le problème N+1, qui multiplie les allers-retours avec la base.

**Garder Hamilton.** Hamilton n'a aucune note : les deux sommes valent 0, et la moyenne demanderait de diviser 0 par 0. Le calcul étant fait par SQLite, la division par zéro donne NULL au lieu d'un nombre, et EF Core ne peut pas ranger NULL dans une propriété de type double : la requête échoue avec une exception à la lecture du résultat. Le filtre Where(e => e.Notes.Any()) évite ce cas en écartant les étudiants sans note avant le calcul.

## 4. Modifier, supprimer, faire évoluer

**Résultats.** Avant SaveChanges, Alan est dans l'état Modified, et le journal montre UPDATE "Etudiants" SET "Promo" = @p0 WHERE "Id" = @p1 : seule la colonne modifiée est écrite. Après la suppression de Linus, il reste 11 notes : ses 2 notes ont été supprimées en cascade par la base. ExecuteUpdate modifie 4 lignes, les notes de Reseau d'Ada, Alan, Grace et Barbara, et s'exécute immédiatement, sans SaveChanges. La migration AjoutEmail ne contient qu'un AddColumn : les 5 étudiants sont conservés, et leur Email vaut null, car la nouvelle colonne accepte NULL.

**Pourquoi aucune méthode Update.** Alan a été chargé par le contexte, qui le suit depuis et garde une copie de ses valeurs d'origine. Au moment de SaveChanges, le contexte compare les valeurs actuelles à cette copie, voit que Promo a changé, passe Alan à l'état Modified et génère l'UPDATE tout seul. Update ne sert que pour un objet que le contexte ne suit pas, par exemple un objet créé ou reçu ailleurs.

**ExecuteUpdate et objets déjà chargés.** Non : la note de Reseau d'Ada, chargée avant l'ExecuteUpdate, garde son coefficient de 1. ExecuteUpdate envoie directement un UPDATE à la base sans passer par le suivi des modifications, donc les objets en mémoire ne sont pas prévenus. Il faut les relire, par exemple avec Reload ou avec un nouveau contexte, pour voir le nouveau coefficient de 2.
