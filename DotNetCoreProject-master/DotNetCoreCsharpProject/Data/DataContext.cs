using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotNetCoreCsharpProject.Entities
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Filieres> Filieres { get; set; }
        public virtual DbSet<Groupes> Groupes { get; set; }
        public virtual DbSet<Levels> Levels { get; set; }
        public virtual DbSet<PlanningDates> PlanningDates { get; set; }
        public virtual DbSet<Professors> Professors { get; set; }
        public virtual DbSet<Reports> Reports { get; set; }
        public virtual DbSet<Societes> Societes { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<TypeReports> TypeReports { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Filieres>(entity =>
            {
                entity.HasKey(e => e.IdFiliere)
                    .HasName("PK__Filieres__D38100FF6B81F551");

                entity.Property(e => e.IdFiliere).HasColumnName("Id_filiere");

                entity.Property(e => e.NomFiliere)
                    .IsRequired()
                    .HasColumnName("Nom_filiere")
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Groupes>(entity =>
            {
                entity.Property(e => e.DateDepotRapport1).HasColumnType("datetime");

                entity.Property(e => e.DateDepotRapport2).HasColumnType("datetime");

                entity.Property(e => e.DateDepotRapport3).HasColumnType("datetime");

                entity.Property(e => e.DateDepotRapport4).HasColumnType("datetime");

                entity.Property(e => e.DateSoutenance).HasColumnType("datetime");

                entity.Property(e => e.DesciptionFile).HasMaxLength(100);

                entity.Property(e => e.DesciptionSujet).HasMaxLength(1000);

                entity.Property(e => e.RapportAvt1).HasMaxLength(200);

                entity.Property(e => e.RapportAvt2).HasMaxLength(200);

                entity.Property(e => e.RapportAvt3).HasMaxLength(200);

                entity.Property(e => e.RapportAvt4).HasMaxLength(200);

                entity.Property(e => e.Sujet)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdSocieteNavigation)
                    .WithMany(p => p.Groupes)
                    .HasForeignKey(d => d.IdSociete)
                    .HasConstraintName("FK__Groupes__IdSocie__6EF57B66");

                entity.HasOne(d => d.IdprofNavigation)
                    .WithMany(p => p.Groupes)
                    .HasForeignKey(d => d.Idprof)
                    .HasConstraintName("FK__Groupes__Idprof__6E01572D");
            });

            modelBuilder.Entity<Levels>(entity =>
            {
                entity.HasKey(e => e.IdNiveau)
                    .HasName("PK__Levels__C7F2C51AA000CB3A");

                entity.Property(e => e.IdNiveau).HasColumnName("Id_niveau");

                entity.Property(e => e.NomNiveau)
                    .IsRequired()
                    .HasColumnName("Nom_niveau")
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<PlanningDates>(entity =>
            {
                entity.Property(e => e.DateAttributionEnc).HasColumnType("datetime");

                entity.Property(e => e.DateFinEnrGrp).HasColumnType("datetime");

                entity.Property(e => e.DateLimitInsSujet).HasColumnType("datetime");

                entity.Property(e => e.DateLimitRapportAvt1).HasColumnType("datetime");

                entity.Property(e => e.DateLimitRapportAvt2).HasColumnType("datetime");

                entity.Property(e => e.DateLimitRapportAvt3).HasColumnType("datetime");

                entity.Property(e => e.DateLimitRapportAvt4).HasColumnType("datetime");

                entity.Property(e => e.DateLimitRapportFinal).HasColumnType("datetime");

                entity.Property(e => e.DateOuvertureEnrGrp).HasColumnType("datetime");

                entity.Property(e => e.PlanningSoutenanceFile).HasMaxLength(100);
            });

            modelBuilder.Entity<Professors>(entity =>
            {
                entity.Property(e => e.IdFil).HasColumnName("Id_fil");

                entity.Property(e => e.IdUser)
                    .IsRequired()
                    .HasColumnName("Id_user")
                    .HasMaxLength(450);

                entity.HasOne(d => d.IdFilNavigation)
                    .WithMany(p => p.Professors)
                    .HasForeignKey(d => d.IdFil)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Professor__Id_fi__6FE99F9F");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Professors)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Professor__Id_us__70DDC3D8");
            });

            modelBuilder.Entity<Reports>(entity =>
            {
                entity.Property(e => e.DateDepot).HasColumnType("datetime");

                entity.Property(e => e.PathReport).HasMaxLength(200);

                entity.HasOne(d => d.IdGroupNavigation)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.IdGroup)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reports__IdGroup__71D1E811");

                entity.HasOne(d => d.IdTypeNavigation)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.IdType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reports__IdType__72C60C4A");
            });

            modelBuilder.Entity<Societes>(entity =>
            {
                entity.Property(e => e.MailEncadrant).HasMaxLength(50);

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NomEncadrant).HasMaxLength(50);

                entity.Property(e => e.Tele).HasMaxLength(1000);

                entity.Property(e => e.TeleEncadrant).HasMaxLength(50);

                entity.Property(e => e.Ville)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.Property(e => e.Cin)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Cne)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.IdFil).HasColumnName("Id_fil");

                entity.Property(e => e.IdNiv).HasColumnName("Id_niv");

                entity.Property(e => e.IdUser)
                    .IsRequired()
                    .HasColumnName("Id_user")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK__Students__GroupI__73BA3083");

                entity.HasOne(d => d.IdFilNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.IdFil)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Students__Id_fil__74AE54BC");

                entity.HasOne(d => d.IdNivNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.IdNiv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Students__Id_niv__75A278F5");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Students__Id_use__76969D2E");
            });

            modelBuilder.Entity<TypeReports>(entity =>
            {
                entity.HasKey(e => e.IdType)
                    .HasName("PK__Type_Rep__74C1FDF6D30A0588");

                entity.ToTable("Type_Reports");

                entity.Property(e => e.IdType).HasColumnName("Id_type");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
