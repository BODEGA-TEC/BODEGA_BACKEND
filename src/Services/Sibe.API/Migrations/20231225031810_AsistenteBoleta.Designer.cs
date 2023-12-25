﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sibe.API.Data;

#nullable disable

namespace Sibe.API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231225031810_AsistenteBoleta")]
    partial class AsistenteBoleta
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Sibe.API.Models.Boletas.Boleta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CarneAsistente")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CarneSolicitante")
                        .HasColumnType("longtext");

                    b.Property<string>("Consecutivo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CorreoSolicitante")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaEmision")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NombreAsistente")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NombreSolicitante")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TipoBoleta")
                        .HasColumnType("int");

                    b.Property<int>("TipoSolicitante")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Boleta");
                });

            modelBuilder.Entity("Sibe.API.Models.Boletas.BoletaComponente", b =>
                {
                    b.Property<int>("BoletaId")
                        .HasColumnType("int");

                    b.Property<int>("ComponenteId")
                        .HasColumnType("int");

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<string>("Observaciones")
                        .HasColumnType("longtext");

                    b.HasKey("BoletaId", "ComponenteId");

                    b.HasIndex("ComponenteId");

                    b.ToTable("BoletaComponente");
                });

            modelBuilder.Entity("Sibe.API.Models.Boletas.BoletaEquipo", b =>
                {
                    b.Property<int>("BoletaId")
                        .HasColumnType("int");

                    b.Property<int>("EquipoId")
                        .HasColumnType("int");

                    b.Property<string>("Observaciones")
                        .HasColumnType("longtext");

                    b.HasKey("BoletaId", "EquipoId");

                    b.HasIndex("EquipoId");

                    b.ToTable("BoletaEquipo");
                });

            modelBuilder.Entity("Sibe.API.Models.Entidades.Asistente", b =>
                {
                    b.Property<string>("Carne")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("HuellaDigital")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Carne");

                    b.ToTable("Asistente");
                });

            modelBuilder.Entity("Sibe.API.Models.Entidades.HistoricoClave", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<byte[]>("ClaveHash")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<byte[]>("ClaveSalt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("HistoricoClave");
                });

            modelBuilder.Entity("Sibe.API.Models.Entidades.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaveTemporal")
                        .HasColumnType("longtext");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Rol")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("Sibe.API.Models.Historicos.HistoricoComponente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CantidadDisponible")
                        .HasColumnType("int");

                    b.Property<int>("CantidadModificada")
                        .HasColumnType("int");

                    b.Property<int>("ComponenteId")
                        .HasColumnType("int");

                    b.Property<int?>("ComprobanteId")
                        .HasColumnType("int");

                    b.Property<string>("Detalle")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ComponenteId");

                    b.HasIndex("ComprobanteId");

                    b.ToTable("HistoricoComponente");
                });

            modelBuilder.Entity("Sibe.API.Models.Historicos.HistoricoEquipo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ComprobanteId")
                        .HasColumnType("int");

                    b.Property<string>("Detalle")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("EquipoId")
                        .HasColumnType("int");

                    b.Property<int>("EstadoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ComprobanteId");

                    b.HasIndex("EquipoId");

                    b.HasIndex("EstadoId");

                    b.ToTable("HistoricoEquipo");
                });

            modelBuilder.Entity("Sibe.API.Models.Historicos.HistoricoRefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaExpiracion")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("HistoricoRefreshToken");
                });

            modelBuilder.Entity("Sibe.API.Models.Inventario.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Tipo", "Nombre")
                        .IsUnique();

                    b.ToTable("Categoria");
                });

            modelBuilder.Entity("Sibe.API.Models.Inventario.Componente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ActivoBodega")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("CantidadDisponible")
                        .HasColumnType("int");

                    b.Property<int>("CantidadTotal")
                        .HasColumnType("int");

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<int>("Condicion")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("EstadoId")
                        .HasColumnType("int");

                    b.Property<string>("Estante")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NoParte")
                        .HasColumnType("longtext");

                    b.Property<string>("Observaciones")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ActivoBodega")
                        .IsUnique();

                    b.HasIndex("CategoriaId");

                    b.HasIndex("EstadoId");

                    b.ToTable("Componente");
                });

            modelBuilder.Entity("Sibe.API.Models.Inventario.Equipo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ActivoBodega")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ActivoTec")
                        .HasColumnType("longtext");

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<int>("Condicion")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("EstadoId")
                        .HasColumnType("int");

                    b.Property<string>("Estante")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Marca")
                        .HasColumnType("longtext");

                    b.Property<string>("Modelo")
                        .HasColumnType("longtext");

                    b.Property<string>("Observaciones")
                        .HasColumnType("longtext");

                    b.Property<string>("Serie")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ActivoBodega")
                        .IsUnique();

                    b.HasIndex("CategoriaId");

                    b.HasIndex("EstadoId");

                    b.ToTable("Equipo");
                });

            modelBuilder.Entity("Sibe.API.Models.Inventario.Estado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Estado");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Descripcion = "El item está disponible en bodega y listo para ser prestado a los solicitantes.",
                            Nombre = "DISPONIBLE"
                        },
                        new
                        {
                            Id = 2,
                            Descripcion = "El item ha sido entregado a un solicitante y está a la espera de ser devuelto.",
                            Nombre = "PRESTADO"
                        },
                        new
                        {
                            Id = 3,
                            Descripcion = "No quedan componentes disponibles en bodega ya que todos han sido prestados.",
                            Nombre = "AGOTADO"
                        },
                        new
                        {
                            Id = 4,
                            Descripcion = "El item está dañado y no puede ser prestado en su estado actual.",
                            Nombre = "DAÑADO"
                        },
                        new
                        {
                            Id = 5,
                            Descripcion = "El item se encuentra en mantenimiento y reparación.",
                            Nombre = "EN REPARACION"
                        },
                        new
                        {
                            Id = 6,
                            Descripcion = "El item ha sido retirado de la bodega, donado o descontinuado su uso.",
                            Nombre = "RETIRADO"
                        },
                        new
                        {
                            Id = 7,
                            Descripcion = "Un funcionario ha apartado el item y está reservado para su préstamo futuro.",
                            Nombre = "APARTADO"
                        });
                });

            modelBuilder.Entity("Sibe.API.Models.Boletas.BoletaComponente", b =>
                {
                    b.HasOne("Sibe.API.Models.Boletas.Boleta", "Boleta")
                        .WithMany("BoletaComponentes")
                        .HasForeignKey("BoletaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sibe.API.Models.Inventario.Componente", "Componente")
                        .WithMany("BoletasComponente")
                        .HasForeignKey("ComponenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Boleta");

                    b.Navigation("Componente");
                });

            modelBuilder.Entity("Sibe.API.Models.Boletas.BoletaEquipo", b =>
                {
                    b.HasOne("Sibe.API.Models.Boletas.Boleta", "Boleta")
                        .WithMany("BoletaEquipo")
                        .HasForeignKey("BoletaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sibe.API.Models.Inventario.Equipo", "Equipo")
                        .WithMany("BoletasEquipo")
                        .HasForeignKey("EquipoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Boleta");

                    b.Navigation("Equipo");
                });

            modelBuilder.Entity("Sibe.API.Models.Entidades.HistoricoClave", b =>
                {
                    b.HasOne("Sibe.API.Models.Entidades.Usuario", null)
                        .WithMany("HistoricoClaves")
                        .HasForeignKey("UsuarioId");
                });

            modelBuilder.Entity("Sibe.API.Models.Historicos.HistoricoComponente", b =>
                {
                    b.HasOne("Sibe.API.Models.Inventario.Componente", "Componente")
                        .WithMany("HistoricoComponente")
                        .HasForeignKey("ComponenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sibe.API.Models.Boletas.Boleta", "Comprobante")
                        .WithMany()
                        .HasForeignKey("ComprobanteId");

                    b.Navigation("Componente");

                    b.Navigation("Comprobante");
                });

            modelBuilder.Entity("Sibe.API.Models.Historicos.HistoricoEquipo", b =>
                {
                    b.HasOne("Sibe.API.Models.Boletas.Boleta", "Comprobante")
                        .WithMany()
                        .HasForeignKey("ComprobanteId");

                    b.HasOne("Sibe.API.Models.Inventario.Equipo", "Equipo")
                        .WithMany("HistoricoEquipo")
                        .HasForeignKey("EquipoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sibe.API.Models.Inventario.Estado", "Estado")
                        .WithMany()
                        .HasForeignKey("EstadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comprobante");

                    b.Navigation("Equipo");

                    b.Navigation("Estado");
                });

            modelBuilder.Entity("Sibe.API.Models.Historicos.HistoricoRefreshToken", b =>
                {
                    b.HasOne("Sibe.API.Models.Entidades.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Sibe.API.Models.Inventario.Componente", b =>
                {
                    b.HasOne("Sibe.API.Models.Inventario.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sibe.API.Models.Inventario.Estado", "Estado")
                        .WithMany()
                        .HasForeignKey("EstadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("Estado");
                });

            modelBuilder.Entity("Sibe.API.Models.Inventario.Equipo", b =>
                {
                    b.HasOne("Sibe.API.Models.Inventario.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sibe.API.Models.Inventario.Estado", "Estado")
                        .WithMany()
                        .HasForeignKey("EstadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("Estado");
                });

            modelBuilder.Entity("Sibe.API.Models.Boletas.Boleta", b =>
                {
                    b.Navigation("BoletaComponentes");

                    b.Navigation("BoletaEquipo");
                });

            modelBuilder.Entity("Sibe.API.Models.Entidades.Usuario", b =>
                {
                    b.Navigation("HistoricoClaves");
                });

            modelBuilder.Entity("Sibe.API.Models.Inventario.Componente", b =>
                {
                    b.Navigation("BoletasComponente");

                    b.Navigation("HistoricoComponente");
                });

            modelBuilder.Entity("Sibe.API.Models.Inventario.Equipo", b =>
                {
                    b.Navigation("BoletasEquipo");

                    b.Navigation("HistoricoEquipo");
                });
#pragma warning restore 612, 618
        }
    }
}
