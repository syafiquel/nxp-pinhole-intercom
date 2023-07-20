using Microsoft.EntityFrameworkCore;
using IntercomModel = IntercomCameraWebApi.Models.Intercom;

namespace IntercomCameraWebApi.Data
{
    public class Intercom : DbContext
    {
        public Intercom(DbContextOptions<Intercom> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntercomModel>()
                .ToTable("intercom", schema: "trx");

            modelBuilder.Entity<IntercomModel>().HasData(
                new IntercomModel { Id = 1, URL = "http://192.168.0.10", Status = "Online" } ,
                new IntercomModel { Id = 2, URL = "http://192.168.0.11", Status = "Online" } ,
                new IntercomModel { Id = 3, URL = "http://192.168.0.12", Status = "Online" } ,
                new IntercomModel { Id = 4, URL = "http://192.168.0.13", Status = "Online" } ,
                new IntercomModel { Id = 5, URL = "http://192.168.0.14", Status = "Online" } ,
                new IntercomModel { Id = 6, URL = "http://192.168.0.14", Status = "Online" } ,
                new IntercomModel { Id = 7, URL = "http://192.168.0.15", Status = "Online" } ,
                new IntercomModel { Id = 8, URL = "http://192.168.0.16", Status = "Online" } ,
                new IntercomModel { Id = 9, URL = "http://192.168.0.17", Status = "Online" } ,
                new IntercomModel { Id = 10, URL = "http://192.168.0.18", Status = "Online" } ,
                new IntercomModel { Id = 11, URL = "http://192.168.0.19", Status = "Online" } ,
                new IntercomModel { Id = 12, URL = "http://192.168.0.20", Status = "Online" } ,
                new IntercomModel { Id = 13, URL = "http://192.168.0.21", Status = "Online" } ,
                new IntercomModel { Id = 14, URL = "http://192.168.0.22", Status = "Online" } ,
                new IntercomModel { Id = 15, URL = "http://192.168.0.23", Status = "Online" } ,
                new IntercomModel { Id = 16, URL = "http://192.168.0.24", Status = "Online" } ,
                new IntercomModel { Id = 17, URL = "http://192.168.0.25", Status = "Online" } ,
                new IntercomModel { Id = 18, URL = "http://192.168.0.26", Status = "Online" } ,
                new IntercomModel { Id = 19, URL = "http://192.168.0.27", Status = "Online" } ,
                new IntercomModel { Id = 20, URL = "http://192.168.0.28", Status = "Online" } ,
                new IntercomModel { Id = 21, URL = "http://192.168.0.29", Status = "Online" } ,
                new IntercomModel { Id = 22, URL = "http://192.168.0.30", Status = "Online" } ,
                new IntercomModel { Id = 23, URL = "http://192.168.0.31", Status = "Online" } ,
                new IntercomModel { Id = 24, URL = "http://192.168.0.32", Status = "Online" } ,
                new IntercomModel { Id = 25, URL = "http://192.168.0.33", Status = "Online" } ,
                new IntercomModel { Id = 26, URL = "http://192.168.0.34", Status = "Online" } ,
                new IntercomModel { Id = 27, URL = "http://192.168.0.35", Status = "Online" } ,
                new IntercomModel { Id = 28, URL = "http://192.168.0.36", Status = "Online" } ,
                new IntercomModel { Id = 29, URL = "http://192.168.0.37", Status = "Online" } ,
                new IntercomModel { Id = 30, URL = "http://192.168.0.38", Status = "Online" } ,
                new IntercomModel { Id = 31, URL = "http://192.168.0.39", Status = "Online" } 
            );
        }

        public DbSet<IntercomModel> IntercomDataModel { get; set; }
    }
}
