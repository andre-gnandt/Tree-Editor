using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalTreeData.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryRegionColumns_NodesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Nodes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Nodes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Nodes");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Nodes");
        }
    }
}
