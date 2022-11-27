using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MVCAppData.Migrations
{
    /// <inheritdoc />
    public partial class initialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Houses1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    GeoLat = table.Column<double>(name: "Geo_Lat", type: "double precision", nullable: false),
                    GeoLon = table.Column<double>(name: "Geo_Lon", type: "double precision", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    file = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Houses1", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Houses1");
        }
    }
}
