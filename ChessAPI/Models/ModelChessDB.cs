namespace ChessAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelChessDB : DbContext
    {
        public ModelChessDB()
            : base("name=ModelChessDB")
        {
        }

        public virtual DbSet<Games> Games { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Games>()
                .Property(e => e.FEN)
                .IsUnicode(false);

            modelBuilder.Entity<Games>()
                .Property(e => e.Statuse)
                .IsUnicode(false);

            modelBuilder.Entity<Games>()
                .Property(e => e.ColorMove)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
               .Property(e => e.Email)
               .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Games)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.White);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Games1)
                .WithOptional(e => e.Users1)
                .HasForeignKey(e => e.Black);
        }
    }
}
