using Microsoft.EntityFrameworkCore;
using ScreenSound.Modelos;
using System.Collections.Generic;

namespace ScreenSound.Banco
{
    internal class ScreenSoundContext : DbContext
    {
        public DbSet<Artista> Artistas {  get; set; }
        public DbSet<Musica> Musicas { get; set; }

        private string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ScreenSoundv3;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(_connectionString)
                .UseLazyLoadingProxies();
        }
    }
}
