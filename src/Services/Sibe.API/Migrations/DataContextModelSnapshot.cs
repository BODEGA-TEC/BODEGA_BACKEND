﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sibe.API.Data;

#nullable disable

namespace Sibe.API.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ComponentePrestamoEstudiante", b =>
                {
                    b.Property<int>("PrestamoEstudianteId")
                        .HasColumnType("int");

                    b.Property<int>("ComponenteId")
                        .HasColumnType("int");

                    b.HasKey("PrestamoEstudianteId", "ComponenteId");

                    b.HasIndex("ComponenteId");

                    b.ToTable("PrestamoEstudianteComponente", (string)null);
                });

            modelBuilder.Entity("ComponentePrestamoProfesor", b =>
                {
                    b.Property<int>("PrestamoProfesorId")
                        .HasColumnType("int");

                    b.Property<int>("ComponenteId")
                        .HasColumnType("int");

                    b.HasKey("PrestamoProfesorId", "ComponenteId");

                    b.HasIndex("ComponenteId");

                    b.ToTable("PrestamoProfesorComponente", (string)null);
                });

            modelBuilder.Entity("EquipoPrestamoEstudiante", b =>
                {
                    b.Property<int>("PrestamoEstudianteId")
                        .HasColumnType("int");

                    b.Property<int>("EquipoId")
                        .HasColumnType("int");

                    b.HasKey("PrestamoEstudianteId", "EquipoId");

                    b.HasIndex("EquipoId");

                    b.ToTable("PrestamoEstudianteEquipo", (string)null);
                });

            modelBuilder.Entity("EquipoPrestamoProfesor", b =>
                {
                    b.Property<int>("PrestamoProfesorId")
                        .HasColumnType("int");

                    b.Property<int>("EquipoId")
                        .HasColumnType("int");

                    b.HasKey("PrestamoProfesorId", "EquipoId");

                    b.HasIndex("EquipoId");

                    b.ToTable("PrestamoProfesorEquipo", (string)null);
                });

            modelBuilder.Entity("Sibe.API.Models.Categoria", b =>
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

                    b.HasIndex("Nombre")
                        .IsUnique();

                    b.ToTable("Categoria");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Nombre = "TTL",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 2,
                            Nombre = "OPERACIONALES",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 3,
                            Nombre = "RESISTENCIAS",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 4,
                            Nombre = "POTENCIOMETROS",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 5,
                            Nombre = "CAPACITORES",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 6,
                            Nombre = "PRECISION",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 7,
                            Nombre = "CRISTALES",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 8,
                            Nombre = "CMOS",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 9,
                            Nombre = "BASES",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 10,
                            Nombre = "TECLADO HEXADECIMAL",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 11,
                            Nombre = "LCD",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 12,
                            Nombre = "OSCILOSCOPIO MINI",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 13,
                            Nombre = "CABLE WIRE WRAP",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 14,
                            Nombre = "PUERTO SERIAL",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 15,
                            Nombre = "DISIPADOR",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 16,
                            Nombre = "SENSORES",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 17,
                            Nombre = "TRANSISTORES/DIODOS",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 18,
                            Nombre = "ADC/DAC",
                            Tipo = 2
                        },
                        new
                        {
                            Id = 19,
                            Nombre = "MULTIMETRO",
                            Tipo = 1
                        },
                        new
                        {
                            Id = 20,
                            Nombre = "GENERADOR FUNCIONES",
                            Tipo = 1
                        },
                        new
                        {
                            Id = 21,
                            Nombre = "FUENTE",
                            Tipo = 1
                        },
                        new
                        {
                            Id = 22,
                            Nombre = "MONITOR",
                            Tipo = 1
                        });
                });

            modelBuilder.Entity("Sibe.API.Models.Componente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ActivoBodega")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ActivoTec")
                        .HasColumnType("longtext");

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("EstadoId")
                        .HasColumnType("int");

                    b.Property<string>("Observaciones")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("EstadoId");

                    b.ToTable("Componente");
                });

            modelBuilder.Entity("Sibe.API.Models.Departamento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Departamento");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Nombre = "ESCUELA DE ELETRONICA"
                        },
                        new
                        {
                            Id = 2,
                            Nombre = "ESCUELA DE MECATRONICA"
                        },
                        new
                        {
                            Id = 3,
                            Nombre = "AREA ACADEMICA DE INGENIERIA EN COMPUTADORES"
                        });
                });

            modelBuilder.Entity("Sibe.API.Models.Equipo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ActivoBodega")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ActivoTec")
                        .HasColumnType("longtext");

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("EstadoId")
                        .HasColumnType("int");

                    b.Property<string>("Marca")
                        .HasColumnType("longtext");

                    b.Property<string>("Modelo")
                        .HasColumnType("longtext");

                    b.Property<string>("Observaciones")
                        .HasColumnType("longtext");

                    b.Property<string>("Serie")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("EstadoId");

                    b.ToTable("Equipo");
                });

            modelBuilder.Entity("Sibe.API.Models.Estado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Estado");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Descripcion = "DISPONIBLE"
                        },
                        new
                        {
                            Id = 2,
                            Descripcion = "PRESTADO"
                        },
                        new
                        {
                            Id = 3,
                            Descripcion = "AGOTADO"
                        },
                        new
                        {
                            Id = 4,
                            Descripcion = "DAÑADO"
                        },
                        new
                        {
                            Id = 5,
                            Descripcion = "EN REPARACION"
                        },
                        new
                        {
                            Id = 6,
                            Descripcion = "RETIRADO"
                        },
                        new
                        {
                            Id = 7,
                            Descripcion = "APARTADO"
                        });
                });

            modelBuilder.Entity("Sibe.API.Models.HistorialEquipo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ComprobantePrestamoId")
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

                    b.HasIndex("ComprobantePrestamoId");

                    b.HasIndex("EquipoId");

                    b.HasIndex("EstadoId");

                    b.ToTable("HistorialEquipo");
                });

            modelBuilder.Entity("Sibe.API.Models.PrestamoEstudiante", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AsistenteCarne")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("EstudianteCarne")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("ProfesorAutorizadorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AsistenteCarne");

                    b.HasIndex("ProfesorAutorizadorId");

                    b.ToTable("PrestamoEstudiante");
                });

            modelBuilder.Entity("Sibe.API.Models.PrestamoProfesor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AsistenteCarne")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ProfesorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AsistenteCarne");

                    b.HasIndex("ProfesorId");

                    b.ToTable("PrestamoProfesor");
                });

            modelBuilder.Entity("Sibe.API.Models.Profesor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DepartamentoId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PrimerApellido")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("SegundoApellido")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DepartamentoId");

                    b.ToTable("Profesor");
                });

            modelBuilder.Entity("Sibe.API.Models.Rol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Nombre")
                        .IsUnique();

                    b.ToTable("Rol");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Descripcion = "Administra usuarios y gestiona información sensible.",
                            Nombre = "ADMINISTRADOR"
                        },
                        new
                        {
                            Id = 2,
                            Descripcion = "Desarrollo y mantenimiento del sistema.",
                            Nombre = "DESARROLLADOR"
                        },
                        new
                        {
                            Id = 3,
                            Descripcion = "Asistencia y tareas administrativas.",
                            Nombre = "ASISTENTE"
                        });
                });

            modelBuilder.Entity("Sibe.API.Models.Usuario", b =>
                {
                    b.Property<string>("Carne")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("CorreoTec")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<int>("RolId")
                        .HasColumnType("int");

                    b.HasKey("Carne");

                    b.HasIndex("RolId");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("ComponentePrestamoEstudiante", b =>
                {
                    b.HasOne("Sibe.API.Models.Componente", null)
                        .WithMany()
                        .HasForeignKey("ComponenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sibe.API.Models.PrestamoEstudiante", null)
                        .WithMany()
                        .HasForeignKey("PrestamoEstudianteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ComponentePrestamoProfesor", b =>
                {
                    b.HasOne("Sibe.API.Models.Componente", null)
                        .WithMany()
                        .HasForeignKey("ComponenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sibe.API.Models.PrestamoProfesor", null)
                        .WithMany()
                        .HasForeignKey("PrestamoProfesorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EquipoPrestamoEstudiante", b =>
                {
                    b.HasOne("Sibe.API.Models.Equipo", null)
                        .WithMany()
                        .HasForeignKey("EquipoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sibe.API.Models.PrestamoEstudiante", null)
                        .WithMany()
                        .HasForeignKey("PrestamoEstudianteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EquipoPrestamoProfesor", b =>
                {
                    b.HasOne("Sibe.API.Models.Equipo", null)
                        .WithMany()
                        .HasForeignKey("EquipoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sibe.API.Models.PrestamoProfesor", null)
                        .WithMany()
                        .HasForeignKey("PrestamoProfesorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Sibe.API.Models.Componente", b =>
                {
                    b.HasOne("Sibe.API.Models.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sibe.API.Models.Estado", "Estado")
                        .WithMany()
                        .HasForeignKey("EstadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("Estado");
                });

            modelBuilder.Entity("Sibe.API.Models.Equipo", b =>
                {
                    b.HasOne("Sibe.API.Models.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sibe.API.Models.Estado", "Estado")
                        .WithMany()
                        .HasForeignKey("EstadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("Estado");
                });

            modelBuilder.Entity("Sibe.API.Models.HistorialEquipo", b =>
                {
                    b.HasOne("Sibe.API.Models.PrestamoEstudiante", "PrestamoEstudiante")
                        .WithMany()
                        .HasForeignKey("ComprobantePrestamoId");

                    b.HasOne("Sibe.API.Models.PrestamoProfesor", "PrestamoProfesor")
                        .WithMany()
                        .HasForeignKey("ComprobantePrestamoId");

                    b.HasOne("Sibe.API.Models.Equipo", "Equipo")
                        .WithMany()
                        .HasForeignKey("EquipoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sibe.API.Models.Estado", "Estado")
                        .WithMany()
                        .HasForeignKey("EstadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipo");

                    b.Navigation("Estado");

                    b.Navigation("PrestamoEstudiante");

                    b.Navigation("PrestamoProfesor");
                });

            modelBuilder.Entity("Sibe.API.Models.PrestamoEstudiante", b =>
                {
                    b.HasOne("Sibe.API.Models.Usuario", "Asistente")
                        .WithMany()
                        .HasForeignKey("AsistenteCarne")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sibe.API.Models.Profesor", "ProfesorAutorizador")
                        .WithMany()
                        .HasForeignKey("ProfesorAutorizadorId");

                    b.Navigation("Asistente");

                    b.Navigation("ProfesorAutorizador");
                });

            modelBuilder.Entity("Sibe.API.Models.PrestamoProfesor", b =>
                {
                    b.HasOne("Sibe.API.Models.Usuario", "Asistente")
                        .WithMany()
                        .HasForeignKey("AsistenteCarne")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sibe.API.Models.Profesor", "Profesor")
                        .WithMany()
                        .HasForeignKey("ProfesorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Asistente");

                    b.Navigation("Profesor");
                });

            modelBuilder.Entity("Sibe.API.Models.Profesor", b =>
                {
                    b.HasOne("Sibe.API.Models.Departamento", "Departamento")
                        .WithMany()
                        .HasForeignKey("DepartamentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Departamento");
                });

            modelBuilder.Entity("Sibe.API.Models.Usuario", b =>
                {
                    b.HasOne("Sibe.API.Models.Rol", "Rol")
                        .WithMany()
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rol");
                });
#pragma warning restore 612, 618
        }
    }
}
