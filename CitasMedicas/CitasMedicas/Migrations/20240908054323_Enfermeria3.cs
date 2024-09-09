using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CitasMedicas.Migrations
{
    public partial class Enfermeria3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TelefonoPaciente",
                table: "Pacientes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CedulaPaciente",
                table: "Pacientes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CedulaMedico",
                table: "Medicos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Enfermera",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CedulaEnfermera = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NombreEnfermera = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApellidoEnfermera = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorreoEnfermera = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelefonoEnfermera = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FechaNacimientoEnfermera = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enfermera", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enfermera_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MedicoEnfermera",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicoId = table.Column<int>(type: "int", nullable: false),
                    EnfermeraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicoEnfermera", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicoEnfermera_Enfermera_EnfermeraId",
                        column: x => x.EnfermeraId,
                        principalTable: "Enfermera",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicoEnfermera_Medicos_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "Medicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enfermera_UsuarioId",
                table: "Enfermera",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicoEnfermera_EnfermeraId",
                table: "MedicoEnfermera",
                column: "EnfermeraId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicoEnfermera_MedicoId",
                table: "MedicoEnfermera",
                column: "MedicoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicoEnfermera");

            migrationBuilder.DropTable(
                name: "Enfermera");

            migrationBuilder.AlterColumn<string>(
                name: "TelefonoPaciente",
                table: "Pacientes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CedulaPaciente",
                table: "Pacientes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "CedulaMedico",
                table: "Medicos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }
    }
}
