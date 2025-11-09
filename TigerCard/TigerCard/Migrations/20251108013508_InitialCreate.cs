using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TigerCard.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntentosFallidos = table.Column<int>(type: "int", nullable: false),
                    Bloqueado = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tarjetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CVV = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LimiteCredito = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SaldoActual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaCorte = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarjetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tarjetas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TarjetaId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacciones_Tarjetas_TarjetaId",
                        column: x => x.TarjetaId,
                        principalTable: "Tarjetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tarjetas_Numero",
                table: "Tarjetas",
                column: "Numero",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tarjetas_UsuarioId",
                table: "Tarjetas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_TarjetaId",
                table: "Transacciones",
                column: "TarjetaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transacciones");

            migrationBuilder.DropTable(
                name: "Tarjetas");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
