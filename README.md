# POOAspEntity — P5AspEntity

TP .NET (ESP/UCAD, 2026/2027) : **API minimale ASP.NET Core + Entity Framework Core + SQLite**.
Module Microsoft Learn : *Utiliser une base de données avec une API minimale, Entity Framework Core et ASP.NET Core*.

```
POOAspEntity/
├── POOAspEntity.sln
├── .gitignore
└── P5AspEntity/
    ├── P5AspEntity.csproj     (packages : Swashbuckle, EF Core InMemory, Sqlite, Design)
    ├── Pizza.cs               (entité Pizza + contexte PizzaDb)
    ├── Program.cs             (connexion SQLite, Swagger, endpoints CRUD)
    ├── appsettings*.json
    └── Properties/launchSettings.json   (http://localhost:5100, ouvre /swagger)
```

## Prérequis

```bash
dotnet --list-sdks          # doit afficher une version 8.x
dotnet tool install --global dotnet-ef --version 8.*
```

> Si `dotnet-ef` est déjà installé : `dotnet tool update --global dotnet-ef --version 8.*`.
> Un `dotnet-ef` en version 10 exige le runtime .NET 10 : restez en 8.x avec un SDK 8.

## Lancer le projet (une seule fois, dans cet ordre)

Toutes les commandes se tapent **dans le dossier du projet** :

```bash
cd POOAspEntity/P5AspEntity

dotnet restore
dotnet ef migrations add InitialCreate    # crée le dossier Migrations (2 fichiers + snapshot)
dotnet ef database update                 # crée Pizzas.db et la table Pizzas
dotnet run --launch-profile http          # écoute sur http://localhost:5100
```

Ouvrir **http://localhost:5100/swagger** (le navigateur s'ouvre seul avec le profil `http`).

Pour le profil HTTPS (`https://localhost:7200/swagger`) : `dotnet dev-certs https --trust`
puis `dotnet run --launch-profile https`.

## Tester le CRUD

Dans Swagger (bouton **Try it out** puis **Execute**) :

| Action | Endpoint | Corps JSON |
| --- | --- | --- |
| Créer | `POST /pizza` | `{ "name": "Pepperoni", "description": "A classic pepperoni pizza" }` |
| Lire tout | `GET /pizzas` | |
| Lire un | `GET /pizza/1` | |
| Modifier | `PUT /pizza/1` | `{ "id": 1, "name": "Pineapple" }` |
| Supprimer | `DELETE /pizza/1` | |

Ou en ligne de commande :

```bash
curl -X POST   http://localhost:5100/pizza   -H "Content-Type: application/json" -d '{"name":"Pepperoni","description":"A classic pepperoni pizza"}'
curl           http://localhost:5100/pizzas
curl           http://localhost:5100/pizza/1
curl -X PUT    http://localhost:5100/pizza/1 -H "Content-Type: application/json" -d '{"id":1,"name":"Pineapple"}'
curl -X DELETE http://localhost:5100/pizza/1
```

Résultats attendus : `POST` → 201 avec la pizza (id 1) · `GET /pizzas` → liste JSON · `PUT` → 204 (la description devient `null`
puisqu'elle n'est pas envoyée, c'est le comportement du cours) · `DELETE` → 200, puis 404 si l'id n'existe plus.

**Test de persistance** : créez une pizza, arrêtez l'application (`Ctrl+C`), relancez `dotnet run --launch-profile http` :
`GET /pizzas` retourne toujours la pizza, car elle est stockée dans `Pizzas.db`.

## Passer à la base en mémoire (étape 1 du cours)

Dans `Program.cs`, commenter `AddSqlite<PizzaDb>(connectionString)` et décommenter la ligne `UseInMemoryDatabase("items")`.
Les données sont alors perdues à chaque redémarrage (aucune migration nécessaire).

## Livrable du TP : lien vers le code source (GitHub)

Après avoir généré les migrations (le dossier `Migrations` doit être dans le dépôt) :

```bash
cd POOAspEntity
git init
git add .
git commit -m "P5AspEntity : API minimale, EF Core et SQLite"
git branch -M main
git remote add origin https://github.com/<votre-compte>/POOAspEntity.git
git push -u origin main
```

Créez d'abord le dépôt vide `POOAspEntity` sur GitHub, puis donnez son lien au professeur.
Pour ajouter plus tard le projet MAUI à la solution :
`dotnet new maui -o P8Maui` puis `dotnet sln POOAspEntity.sln add P8Maui/P8Maui.csproj`.

## Dépannage

| Message | Cause et solution |
| --- | --- |
| `Could not execute because the specified command or file was not found` (`dotnet ef`) | Outil non installé ou PATH non rechargé : réinstaller `dotnet-ef`, rouvrir le terminal |
| `Unable to create an object of type 'PizzaDb'` | Lancer la commande depuis `P5AspEntity` (là où se trouve le `.csproj`), après `dotnet restore` |
| `no such table: Pizzas` | Migration non appliquée : relancer `dotnet ef database update` depuis `P5AspEntity` |
| `Address already in use` | Un autre processus utilise le port 5100 : changer `applicationUrl` dans `launchSettings.json` |
| `Pizzas.db` introuvable | Il est créé dans le dossier d'où l'on lance `dotnet ef`/`dotnet run` : toujours `cd P5AspEntity` |
