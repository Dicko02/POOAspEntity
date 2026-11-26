using Microsoft.EntityFrameworkCore;

namespace PizzaStore.Models
{
    // Classe d'entite : un objet metier, mappe a la table "Pizzas".
    public class Pizza
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    // Classe de contexte : represente une session avec la base de donnees.
    // Elle interroge et enregistre les donnees et gere la connexion.
    public class PizzaDb : DbContext
    {
        public PizzaDb(DbContextOptions options) : base(options) { }

        public DbSet<Pizza> Pizzas { get; set; } = null!;
    }
}
