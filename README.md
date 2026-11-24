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