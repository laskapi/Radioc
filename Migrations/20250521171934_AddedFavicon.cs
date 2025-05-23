using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Radioc.Migrations
{
    /// <inheritdoc />
    public partial class AddedFavicon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Favicon",
                table: "FavoriteStations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Favicon",
                table: "FavoriteStations");
        }
    }
}
