using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eAgenda.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class Add_TBContato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contatos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Empresa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Cargo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contatos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Compromisso",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Assunto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraTermino = table.Column<TimeSpan>(type: "time", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Local = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContatoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compromisso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compromisso_Contatos_ContatoId",
                        column: x => x.ContatoId,
                        principalTable: "Contatos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compromisso_ContatoId",
                table: "Compromisso",
                column: "ContatoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Compromisso");

            migrationBuilder.DropTable(
                name: "Contatos");
        }
    }
}
