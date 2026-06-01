using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Infrastructure.Data;

public class ListVIPContext : DbContext
{
    public ListVIPContext(DbContextOptions<ListVIPContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Organizador> Organizadores { get; set; }
    public DbSet<Promotor> Promotores { get; set; }
    public DbSet<Portero> Porteros { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<PromotorEvento> PromotorEventos { get; set; }
    public DbSet<Invitado> Invitados { get; set; }
    public DbSet<RegistroIngreso> RegistrosIngreso { get; set; }
    public DbSet<Invitacion> Invitaciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Herencia: tabla única para Usuario, Organizador, Promotor y Portero
        modelBuilder.Entity<Usuario>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<Usuario>("Usuario")
            .HasValue<Organizador>("Organizador")
            .HasValue<Promotor>("Promotor")
            .HasValue<Portero>("Portero");

        // Organizador → Evento (1 a N)
        modelBuilder.Entity<Evento>()
            .HasOne(e => e.Organizador)
            .WithMany(o => o.Eventos)
            .HasForeignKey(e => e.OrganizadorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Promotor → PromotorEvento (1 a N)
        modelBuilder.Entity<PromotorEvento>()
            .HasOne(pe => pe.Promotor)
            .WithMany(p => p.PromotorEventos)
            .HasForeignKey(pe => pe.PromotorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Evento → PromotorEvento (1 a N)
        modelBuilder.Entity<PromotorEvento>()
            .HasOne(pe => pe.Evento)
            .WithMany(e => e.PromotorEventos)
            .HasForeignKey(pe => pe.EventoId)
            .OnDelete(DeleteBehavior.Restrict);

        // PromotorEvento → Invitado (1 a N)
        modelBuilder.Entity<Invitado>()
            .HasOne(i => i.PromotorEvento)
            .WithMany(pe => pe.Invitados)
            .HasForeignKey(i => i.PromotorEventoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Invitado → RegistroIngreso (1 a 0..1)
        modelBuilder.Entity<RegistroIngreso>()
            .HasOne(r => r.Invitado)
            .WithOne(i => i.RegistroIngreso)
            .HasForeignKey<RegistroIngreso>(r => r.InvitadoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Portero → RegistroIngreso (1 a N)
        modelBuilder.Entity<RegistroIngreso>()
            .HasOne(r => r.Portero)
            .WithMany(p => p.RegistrosIngreso)
            .HasForeignKey(r => r.PorteroId)
            .OnDelete(DeleteBehavior.Restrict);

        // Evento → RegistroIngreso (1 a N)
        modelBuilder.Entity<RegistroIngreso>()
            .HasOne(r => r.Evento)
            .WithMany(e => e.RegistrosIngreso)
            .HasForeignKey(r => r.EventoId)
            .OnDelete(DeleteBehavior.Restrict);

        // Organizador → Invitacion (1 a N)
        modelBuilder.Entity<Invitacion>()
            .HasOne<Organizador>()
            .WithMany()
            .HasForeignKey(i => i.OrganizadorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
