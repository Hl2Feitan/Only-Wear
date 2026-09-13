using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OnlyWear.Api.Migrations
{
    /// <inheritdoc />
    public partial class InicialOnlyWear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    ImagenUrl = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Rol = table.Column<string>(type: "text", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreciosPorTalle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductoId = table.Column<int>(type: "integer", nullable: false),
                    Talle = table.Column<string>(type: "text", nullable: false),
                    Precio = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreciosPorTalle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreciosPorTalle_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisenosPersonalizados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    ImagenClienteUrl = table.Column<string>(type: "text", nullable: false),
                    PosicionX = table.Column<float>(type: "real", nullable: false),
                    PosicionY = table.Column<float>(type: "real", nullable: false),
                    Escala = table.Column<float>(type: "real", nullable: false),
                    TalleElegido = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisenosPersonalizados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisenosPersonalizados_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    MetodoPago = table.Column<string>(type: "text", nullable: false),
                    MetodoEntrega = table.Column<string>(type: "text", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesPedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PedidoId = table.Column<int>(type: "integer", nullable: false),
                    ProductoId = table.Column<int>(type: "integer", nullable: true),
                    DisenoPersonalizadoId = table.Column<int>(type: "integer", nullable: true),
                    Talle = table.Column<string>(type: "text", nullable: false),
                    Cantidad = table.Column<int>(type: "integer", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesPedido", x => x.Id);
                    table.CheckConstraint("CK_DetallePedido_ProductoODiseno", "(\"ProductoId\" IS NOT NULL AND \"DisenoPersonalizadoId\" IS NULL) OR (\"ProductoId\" IS NULL AND \"DisenoPersonalizadoId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_DetallesPedido_DisenosPersonalizados_DisenoPersonalizadoId",
                        column: x => x.DisenoPersonalizadoId,
                        principalTable: "DisenosPersonalizados",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DetallesPedido_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesPedido_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedido_DisenoPersonalizadoId",
                table: "DetallesPedido",
                column: "DisenoPersonalizadoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedido_PedidoId",
                table: "DetallesPedido",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedido_ProductoId",
                table: "DetallesPedido",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DisenosPersonalizados_UsuarioId",
                table: "DisenosPersonalizados",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UsuarioId",
                table: "Pedidos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PreciosPorTalle_ProductoId",
                table: "PreciosPorTalle",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesPedido");

            migrationBuilder.DropTable(
                name: "PreciosPorTalle");

            migrationBuilder.DropTable(
                name: "DisenosPersonalizados");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
