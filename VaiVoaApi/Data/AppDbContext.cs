using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VaiVoaApi.Models;

namespace VaiVoaApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Cartao> Cartoes { get; set; }
        public string GenerateCardNumber()
        {
            Random rand = new Random();
            String sRandomResult = "";
            for (int i = 0; i < 16; i++)
            {
                sRandomResult += rand.Next(9).ToString();
            }
            return sRandomResult;
        }
    }
}
