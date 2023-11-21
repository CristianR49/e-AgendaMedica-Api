using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_AgendaMedica.Infra.Orm.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoAtividade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "TBMedico",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataConclusao",
                table: "TBAtividade",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "TBMedico");

            migrationBuilder.DropColumn(
                name: "DataConclusao",
                table: "TBAtividade");
        }
    }
}
