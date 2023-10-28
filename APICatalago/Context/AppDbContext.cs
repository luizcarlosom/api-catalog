using APICatalago.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalago.Context
{
    public class AppDbContext : DbContext
    {
        //Configurando context
        public AppDbContext(DbContextOptions<AppDbContext> options ) : base( options )
        { }
        //Setando as duas tabelas
        public DbSet<Categoria>? Categorias { get; set; }
        public DbSet<Produto>? Produtos { get; set; }
    }
}
