namespace MajeBug.Data
{
    using Domain;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    public class DataContext : DbContext
    {
        // El contexto se ha configurado para usar una cadena de conexión 'DataContext' del archivo 
        // de configuración de la aplicación (App.config o Web.config). De forma predeterminada, 
        // esta cadena de conexión tiene como destino la base de datos 'MajeBug.Data.DataContext' de la instancia LocalDb. 
        // 
        // Si desea tener como destino una base de datos y/o un proveedor de base de datos diferente, 
        // modifique la cadena de conexión 'DataContext'  en el archivo de configuración de la aplicación.
        public DataContext() : base("name=DataContext")
        {
                


        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var bug = modelBuilder.Entity<Bug>();
            bug.HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            bug.Property(x => x.IsFixed).IsRequired();
            bug.Property(x => x.Title).HasMaxLength(120).IsRequired();
            bug.Property(x => x.Body).HasMaxLength(500).IsRequired();
            bug.Property(x => x.StepsToReproduce).HasMaxLength(250).IsOptional();
            bug.Property(x => x.Severity).IsRequired();
            //relationships
            //tracking
            bug.HasRequired(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById);
            bug.HasOptional(x => x.ModifiedBy).WithMany().HasForeignKey(x => x.ModifiedById);
            //concurrency management
            bug.Property(x => x.RowVersion).IsConcurrencyToken();
            

            var user = modelBuilder.Entity<User>();
            user.HasKey(x => x.Id);
            user.Property(x => x.DisplayName).HasMaxLength(100);
            user.Property(x => x.CreatedAt).IsRequired();

            //**modelBuilder.Configurations.Add(new UserMap());

        }



        // Agregue un DbSet para cada tipo de entidad que desee incluir en el modelo. Para obtener más información 
        // sobre cómo configurar y usar un modelo Code First, vea http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Bug> Bugs { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }

    //**Another way to configure
    public class UserMap : EntityTypeConfiguration<User> {
        public UserMap()
        {
            HasKey(x => x.Id);
            Property(x => x.DisplayName).HasMaxLength(100);
            Property(x => x.CreatedAt).IsRequired();
        }
    }

}