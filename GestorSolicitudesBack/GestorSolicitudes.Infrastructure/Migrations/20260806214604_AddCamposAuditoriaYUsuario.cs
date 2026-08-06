using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorSolicitudes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCamposAuditoriaYUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NombreUsuario",
                table: "Usuarios",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "NombreCompleto",
                table: "Usuarios",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCierre",
                table: "Solicitudes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Solicitudes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "NombreCompleto", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 6, 21, 46, 3, 779, DateTimeKind.Utc).AddTicks(6656), "Administrador del Sistema", "$2a$11$tzEQ99S72St3kleeUH3e8e86EVkHwow8HaHgDUctfxNBBVxmJesey" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "NombreCompleto", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 6, 21, 46, 3, 904, DateTimeKind.Utc).AddTicks(1732), "Agente de Soporte 1", "$2a$11$d7v8yhoM0hdDOL0nXR4zh.VhqEbnuvgejQa8KN.xRKma.FTSreX0." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreCompleto",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaCierre",
                table: "Solicitudes");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Solicitudes");

            migrationBuilder.AlterColumn<string>(
                name: "NombreUsuario",
                table: "Usuarios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaCreacion", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 6, 20, 56, 1, 416, DateTimeKind.Utc).AddTicks(1513), "$2a$11$dPEiMtm3r1hIij3RMwIZBORz18yO9.qRP62RsQSzDgA38ORpch88y" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaCreacion", "PasswordHash" },
                values: new object[] { new DateTime(2026, 8, 6, 20, 56, 1, 538, DateTimeKind.Utc).AddTicks(2235), "$2a$11$K.7.AXPu74B/oE4Vl.vZW.E6kKPC4JA2Xxnae.bUyvfKt2pYwl0ym" });
        }
    }
}
