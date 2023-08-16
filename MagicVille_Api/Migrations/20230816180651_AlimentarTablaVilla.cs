using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVille_Api.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[] { 1, "si", "Default", new DateTime(2023, 8, 16, 12, 6, 51, 740, DateTimeKind.Local).AddTicks(2751), new DateTime(2023, 8, 16, 12, 6, 51, 740, DateTimeKind.Local).AddTicks(2739), "/images/1.jpg", 10.0, "Inicial", 1, 1.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
