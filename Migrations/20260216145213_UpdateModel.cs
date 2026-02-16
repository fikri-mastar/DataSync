using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataSync.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlatformName",
                table: "Platforms",
                newName: "UniqueName");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Wells",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Wells",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Platforms",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Platforms",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Platforms");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Platforms");

            migrationBuilder.RenameColumn(
                name: "UniqueName",
                table: "Platforms",
                newName: "PlatformName");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Wells",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
