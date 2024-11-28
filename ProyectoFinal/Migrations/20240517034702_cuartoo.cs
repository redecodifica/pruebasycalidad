using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoFinal.Migrations
{
    /// <inheritdoc />
    public partial class cuartoo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AnalisisC",
                table: "AnalisisC");

            migrationBuilder.RenameTable(
                name: "AnalisisC",
                newName: "Analisis");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Analisis",
                table: "Analisis",
                column: "idAnalisis");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Analisis",
                table: "Analisis");

            migrationBuilder.RenameTable(
                name: "Analisis",
                newName: "AnalisisC");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnalisisC",
                table: "AnalisisC",
                column: "idAnalisis");
        }
    }
}
