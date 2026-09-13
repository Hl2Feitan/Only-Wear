using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlyWear.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarProductoBaseADiseno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductoBaseId",
                table: "DisenosPersonalizados",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DisenosPersonalizados_ProductoBaseId",
                table: "DisenosPersonalizados",
                column: "ProductoBaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_DisenosPersonalizados_Productos_ProductoBaseId",
                table: "DisenosPersonalizados",
                column: "ProductoBaseId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DisenosPersonalizados_Productos_ProductoBaseId",
                table: "DisenosPersonalizados");

            migrationBuilder.DropIndex(
                name: "IX_DisenosPersonalizados_ProductoBaseId",
                table: "DisenosPersonalizados");

            migrationBuilder.DropColumn(
                name: "ProductoBaseId",
                table: "DisenosPersonalizados");
        }
    }
}
