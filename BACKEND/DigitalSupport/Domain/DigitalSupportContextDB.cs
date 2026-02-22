using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApiDigitalSupport.Domain.Entities;

namespace WebApiDigitalSupport.Domain;

public partial class DigitalSupportContextDB : DbContext
{
    public DigitalSupportContextDB()
    {
    }

    public DigitalSupportContextDB(DbContextOptions<DigitalSupportContextDB> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAplicativo> TblAplicativos { get; set; }

    public virtual DbSet<TblAsignacionSolicitud> TblAsignacionSolicituds { get; set; }

    public virtual DbSet<TblCliente> TblClientes { get; set; }

    public virtual DbSet<TblColaborador> TblColaboradors { get; set; }

    public virtual DbSet<TblNotificacion> TblNotificacions { get; set; }

    public virtual DbSet<TblRegistroTrabajo> TblRegistroTrabajos { get; set; }

    public virtual DbSet<TblRolColaborador> TblRolColaboradors { get; set; }

    public virtual DbSet<TblRolUsuario> TblRolUsuarios { get; set; }

    public virtual DbSet<TblSolicitud> TblSolicituds { get; set; }

    public virtual DbSet<TblTipoCliente> TblTipoClientes { get; set; }

    public virtual DbSet<TblTipoSolicitud> TblTipoSolicituds { get; set; }

    public virtual DbSet<TblUsuarioCliente> TblUsuarioClientes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=BD_SOPORTE_DIGITAL;User Id=Cuno;Password=Picosa22;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAplicativo>(entity =>
        {
            entity.HasKey(e => e.NIdAplicativo).HasName("PK__TBL_APLI__555F9223D4B668D6");

            entity.ToTable("TBL_APLICATIVO", "DigitalSuport");

            entity.Property(e => e.NIdAplicativo).HasColumnName("nIdAplicativo");
            entity.Property(e => e.BEstado).HasColumnName("bEstado");
            entity.Property(e => e.DFechaLanzamiento).HasColumnName("dFechaLanzamiento");
            entity.Property(e => e.DFechaModificacion).HasColumnName("dFechaModificacion");
            entity.Property(e => e.NIdCliente).HasColumnName("nIdCliente");
            entity.Property(e => e.SDescripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sDescripcion");
            entity.Property(e => e.SNombreAplicativo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sNombreAplicativo");
            entity.Property(e => e.SVersion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sVersion");

            entity.HasOne(d => d.NIdClienteNavigation).WithMany(p => p.TblAplicativos)
                .HasForeignKey(d => d.NIdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Aplicativo_Cliente");
        });

        modelBuilder.Entity<TblAsignacionSolicitud>(entity =>
        {
            entity.HasKey(e => e.NIdAsignacionSolicitud).HasName("PK__TBL_ASIG__E56EF0DF23E72DC3");

            entity.ToTable("TBL_ASIGNACION_SOLICITUD", "DigitalSuport");

            entity.Property(e => e.NIdAsignacionSolicitud).HasColumnName("nIdAsignacionSolicitud");
            entity.Property(e => e.BEsCoordinador)
                .HasDefaultValue(false)
                .HasColumnName("bEsCoordinador");
            entity.Property(e => e.BEstado).HasColumnName("bEstado");
            entity.Property(e => e.NIdColaborador).HasColumnName("nIdColaborador");
            entity.Property(e => e.NIdSolicitud).HasColumnName("nIdSolicitud");

            entity.HasOne(d => d.NIdColaboradorNavigation).WithMany(p => p.TblAsignacionSolicituds)
                .HasForeignKey(d => d.NIdColaborador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignacion_Colaborador");

            entity.HasOne(d => d.NIdSolicitudNavigation).WithMany(p => p.TblAsignacionSolicituds)
                .HasForeignKey(d => d.NIdSolicitud)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignacion_Solicitud");
        });

        modelBuilder.Entity<TblCliente>(entity =>
        {
            entity.HasKey(e => e.NIdCliente).HasName("PK__TBL_CLIE__8D5EB14CA8E21A3C");

            entity.ToTable("TBL_CLIENTE", "DigitalSuport");

            entity.HasIndex(e => e.SEmail, "UC_TBL_CLIENTE_Email").IsUnique();

            entity.Property(e => e.NIdCliente).HasColumnName("nIdCliente");
            entity.Property(e => e.BEstado).HasColumnName("bEstado");
            entity.Property(e => e.DFechaNacimiento).HasColumnName("dFechaNacimiento");
            entity.Property(e => e.NEdad).HasColumnName("nEdad");
            entity.Property(e => e.NIdTipoCliente).HasColumnName("nIdTipoCliente");
            entity.Property(e => e.SApellido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sApellido");
            entity.Property(e => e.SContrasena)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sContrasena");
            entity.Property(e => e.SEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sEmail");
            entity.Property(e => e.SNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sNombre");

            entity.HasOne(d => d.NIdTipoClienteNavigation).WithMany(p => p.TblClientes)
                .HasForeignKey(d => d.NIdTipoCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cliente_TipoCliente");
        });

        modelBuilder.Entity<TblColaborador>(entity =>
        {
            entity.HasKey(e => e.NIdColaborador).HasName("PK__TBL_COLA__E317286F29BEC026");

            entity.ToTable("TBL_COLABORADOR", "DigitalSuport");

            entity.HasIndex(e => e.SEmail, "UC_TBL_COLABORADOR_Email").IsUnique();

            entity.Property(e => e.NIdColaborador).HasColumnName("nIdColaborador");
            entity.Property(e => e.BEstado).HasColumnName("bEstado");
            entity.Property(e => e.NIdRolColaborador).HasColumnName("nIdRolColaborador");
            entity.Property(e => e.NIdRolUsuario).HasColumnName("nIdRolUsuario");
            entity.Property(e => e.SApellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sApellido");
            entity.Property(e => e.SContrasena)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sContrasena");
            entity.Property(e => e.SEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sEmail");
            entity.Property(e => e.SNombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sNombre");

            entity.HasOne(d => d.NIdRolColaboradorNavigation).WithMany(p => p.TblColaboradors)
                .HasForeignKey(d => d.NIdRolColaborador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Colaborador_Rol");

            entity.HasOne(d => d.NIdRolUsuarioNavigation).WithMany(p => p.TblColaboradors)
                .HasForeignKey(d => d.NIdRolUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolUsuario_Colaborador");
        });

        modelBuilder.Entity<TblNotificacion>(entity =>
        {
            entity.HasKey(e => e.NIdNotificacion).HasName("PK__TBL_NOTI__3A157B25409AF9D3");

            entity.ToTable("TBL_NOTIFICACION", "DigitalSuport");

            entity.Property(e => e.NIdNotificacion).HasColumnName("nIdNotificacion");
            entity.Property(e => e.BEstado).HasColumnName("bEstado");
            entity.Property(e => e.BLeido)
                .HasDefaultValue(false)
                .HasColumnName("bLeido");
            entity.Property(e => e.DFechaEnvio)
                .HasColumnType("datetime")
                .HasColumnName("dFechaEnvio");
            entity.Property(e => e.NIdSolicitud).HasColumnName("nIdSolicitud");
            entity.Property(e => e.SDescripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sDescripcion");

            entity.HasOne(d => d.NIdSolicitudNavigation).WithMany(p => p.TblNotificacions)
                .HasForeignKey(d => d.NIdSolicitud)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificacion_Solicitud");
        });

        modelBuilder.Entity<TblRegistroTrabajo>(entity =>
        {
            entity.HasKey(e => e.NIdRegistroTrabajo).HasName("PK__TBL_REGI__4E1EEAD59DD1FFC7");

            entity.ToTable("TBL_REGISTRO_TRABAJO", "DigitalSuport");

            entity.Property(e => e.NIdRegistroTrabajo).HasColumnName("nIdRegistroTrabajo");
            entity.Property(e => e.BEstado).HasColumnName("bEstado");
            entity.Property(e => e.DFechaRegistro).HasColumnName("dFechaRegistro");
            entity.Property(e => e.NHorasTrabajadas)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("nHorasTrabajadas");
            entity.Property(e => e.NIdColaborador).HasColumnName("nIdColaborador");
            entity.Property(e => e.NIdSolicitud).HasColumnName("nIdSolicitud");
            entity.Property(e => e.SDescripcion)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("sDescripcion");
            entity.Property(e => e.SObservacion)
                .IsUnicode(false)
                .HasColumnName("sObservacion");

            entity.HasOne(d => d.NIdColaboradorNavigation).WithMany(p => p.TblRegistroTrabajos)
                .HasForeignKey(d => d.NIdColaborador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RegistroTrabajo_Colaborador");

            entity.HasOne(d => d.NIdSolicitudNavigation).WithMany(p => p.TblRegistroTrabajos)
                .HasForeignKey(d => d.NIdSolicitud)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RegistroTrabajo_Solicitud");
        });

        modelBuilder.Entity<TblRolColaborador>(entity =>
        {
            entity.HasKey(e => e.NIdRolColaborador).HasName("PK__TBL_ROL___9B65DBD4FB09632E");

            entity.ToTable("TBL_ROL_COLABORADOR", "DigitalSuport");

            entity.Property(e => e.NIdRolColaborador).HasColumnName("nIdRolColaborador");
            entity.Property(e => e.BEstado).HasColumnName("bEstado");
            entity.Property(e => e.SDescripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sDescripcion");
        });

        modelBuilder.Entity<TblRolUsuario>(entity =>
        {
            entity.HasKey(e => e.NIdRolUsuario).HasName("PK__TBL_ROL___70DAE4D7185FAD74");

            entity.ToTable("TBL_ROL_USUARIO", "DigitalSuport");

            entity.Property(e => e.NIdRolUsuario).HasColumnName("nIdRolUsuario");
            entity.Property(e => e.SNombreRol)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sNombreRol");
        });

        modelBuilder.Entity<TblSolicitud>(entity =>
        {
            entity.HasKey(e => e.NIdSolicitud).HasName("PK__TBL_SOLI__5A47F2F3FEBA660A");

            entity.ToTable("TBL_SOLICITUD", "DigitalSuport");

            entity.Property(e => e.NIdSolicitud).HasColumnName("nIdSolicitud");
            entity.Property(e => e.BEstado).HasColumnName("bEstado");
            entity.Property(e => e.DFechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("dFechaCreacion");
            entity.Property(e => e.DFechaFinalizacion)
                .HasColumnType("datetime")
                .HasColumnName("dFechaFinalizacion");
            entity.Property(e => e.NIdAplicativo).HasColumnName("nIdAplicativo");
            entity.Property(e => e.NIdTipoSolicitud).HasColumnName("nIdTipoSolicitud");
            entity.Property(e => e.NIdUsuarioCliente).HasColumnName("nIdUsuarioCliente");
            entity.Property(e => e.SEstado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pendiente")
                .HasColumnName("sEstado");
            entity.Property(e => e.SMotivo)
                .IsUnicode(false)
                .HasColumnName("sMotivo");

            entity.HasOne(d => d.NIdAplicativoNavigation).WithMany(p => p.TblSolicituds)
                .HasForeignKey(d => d.NIdAplicativo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_Aplicativo");

            entity.HasOne(d => d.NIdTipoSolicitudNavigation).WithMany(p => p.TblSolicituds)
                .HasForeignKey(d => d.NIdTipoSolicitud)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_TipoSolicitud");

            entity.HasOne(d => d.NIdUsuarioClienteNavigation).WithMany(p => p.TblSolicituds)
                .HasForeignKey(d => d.NIdUsuarioCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_UsuarioCliente");
        });

        modelBuilder.Entity<TblTipoCliente>(entity =>
        {
            entity.HasKey(e => e.NIdTipoCliente).HasName("PK__TBL_TIPO__E0910A2B976FD61B");

            entity.ToTable("TBL_TIPO_CLIENTE", "DigitalSuport");

            entity.Property(e => e.NIdTipoCliente).HasColumnName("nIdTipoCliente");
            entity.Property(e => e.BEstado).HasColumnName("bEstado");
            entity.Property(e => e.SDescripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sDescripcion");
        });

        modelBuilder.Entity<TblTipoSolicitud>(entity =>
        {
            entity.HasKey(e => e.NIdTipoSolicitud).HasName("PK__TBL_TIPO__3804CC10C1679878");

            entity.ToTable("TBL_TIPO_SOLICITUD", "DigitalSuport");

            entity.Property(e => e.NIdTipoSolicitud).HasColumnName("nIdTipoSolicitud");
            entity.Property(e => e.BEstado).HasColumnName("bEstado");
            entity.Property(e => e.SDescripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sDescripcion");
        });

        modelBuilder.Entity<TblUsuarioCliente>(entity =>
        {
            entity.HasKey(e => e.NIdUsuarioCliente).HasName("PK__TBL_USUA__86461EE3E9BAEFB1");

            entity.ToTable("TBL_USUARIO_CLIENTE", "DigitalSuport");

            entity.Property(e => e.NIdUsuarioCliente).HasColumnName("nIdUsuarioCliente");
            entity.Property(e => e.BEstado).HasColumnName("bEstado");
            entity.Property(e => e.NIdCliente).HasColumnName("nIdCliente");
            entity.Property(e => e.NIdRolUsuario).HasColumnName("nIdRolUsuario");
            entity.Property(e => e.SApellido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sApellido");
            entity.Property(e => e.SContrasena)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sContrasena");
            entity.Property(e => e.SEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sEmail");
            entity.Property(e => e.SNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sNombre");

            entity.HasOne(d => d.NIdClienteNavigation).WithMany(p => p.TblUsuarioClientes)
                .HasForeignKey(d => d.NIdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioCliente_Cliente");

            entity.HasOne(d => d.NIdRolUsuarioNavigation).WithMany(p => p.TblUsuarioClientes)
                .HasForeignKey(d => d.NIdRolUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolUsuario_UsuarioCliente");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
