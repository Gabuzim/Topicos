using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class NovaTabelaModelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModeloId",
                table: "Carros",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Modelos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modelos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carros_ModeloId",
                table: "Carros",
                column: "ModeloId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carros_Modelos_ModeloId",
                table: "Carros",
                column: "ModeloId",
                principalTable: "Modelos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carros_Modelos_ModeloId",
                table: "Carros");

            migrationBuilder.DropTable(
                name: "Modelos");

            migrationBuilder.DropIndex(
                name: "IX_Carros_ModeloId",
                table: "Carros");

            migrationBuilder.DropColumn(
                name: "ModeloId",
                table: "Carros");
        }
    }
}
