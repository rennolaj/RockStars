using Microsoft.EntityFrameworkCore.Migrations;

namespace RockStars.API.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 1000, nullable: false),
                    Year = table.Column<uint>(nullable: true),
                    ArtistId = table.Column<int>(nullable: false),
                    Artist = table.Column<string>(maxLength: 150, nullable: true),
                    Bpm = table.Column<uint>(nullable: true),
                    Duration = table.Column<uint>(nullable: false),
                    Genre = table.Column<string>(maxLength: 1500, nullable: false),
                    SpotifyId = table.Column<string>(maxLength: 1500, nullable: true),
                    Album = table.Column<string>(maxLength: 1500, nullable: true),
                    ShortName = table.Column<string>(maxLength: 1500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Songs_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Songs_ArtistId",
                table: "Songs",
                column: "ArtistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Songs");
        }
    }
}
