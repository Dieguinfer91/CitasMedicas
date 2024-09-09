using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CitasMedicas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CitasMedicas.Data
{
    public class CitasMedicasContext : IdentityDbContext<Usuario>
    {
        public CitasMedicasContext(DbContextOptions<CitasMedicasContext> options)
            : base(options)
        {
        }

        
        public virtual DbSet<Consultorio> Consultorios { get; set; } = default!;
        
        public virtual DbSet<Especialidad> Especialidades { get; set; } = default!;
        public virtual DbSet<Medicamento> Medicamentos { get; set; } = default!;
        public virtual DbSet<Medico> Medicos { get; set; } = default!;
        public virtual DbSet<Paciente> Pacientes { get; set; } = default!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = default!;
        public virtual DbSet<Cita> Citas { get; set; } = default!;
        public virtual DbSet<Antecedente> Antecedentes { get; set; } = default!;
        public virtual DbSet<HistoriaClinica> HistoriasClinicas { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Paciente>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Enfermera>()
           .HasOne(p => p.Usuario)
           .WithMany()
           .HasForeignKey(p => p.UsuarioId)
           .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Medico>()
            //    .HasOne(m => m.Usuario)
            //    .WithMany()
            //    .HasForeignKey(m => m.UsuarioId)
            //    .OnDelete(DeleteBehavior.NoAction);



            modelBuilder.Entity<Medico>()//tabla implicita del modelo
           .HasMany(m => m.Especialidades)
           .WithMany(e => e.Medicos)
           .UsingEntity<Dictionary<string, object>>(
               "MedicoEspecialidad",
           r => r.HasOne<Especialidad>().WithMany().HasForeignKey("EspecialidadId"),
           l => l.HasOne<Medico>().WithMany().HasForeignKey("MedicoId"));

            //     modelBuilder.Entity<Medico>()
            //.HasOne(m => m.Consultorio)
            //.WithOne(c => c.Medico)
            //.HasForeignKey<Consultorio>(c => c.MedicoId);

            modelBuilder.Entity<Consultorio>()
        .HasOne(c => c.Medico)
        .WithOne(m => m.Consultorio)
        .HasForeignKey<Medico>(m => m.ConsultorioId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<CitasMedicas.Models.MedicamentoReceta>? MedicamentoReceta { get; set; }
        public DbSet<CitasMedicas.Models.MedicoEnfermera>? MedicoEnfermera { get; set; }
        public DbSet<CitasMedicas.Models.Enfermera>? Enfermera { get; set; }


    }
}
