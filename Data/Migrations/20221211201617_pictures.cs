using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class pictures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Movie_MovieId",
                table: "Image");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Image",
                table: "Image");

            migrationBuilder.RenameTable(
                name: "Image",
                newName: "Picture");

            migrationBuilder.RenameIndex(
                name: "IX_Image_MovieId",
                table: "Picture",
                newName: "IX_Picture_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Picture",
                table: "Picture",
                column: "Path");

            migrationBuilder.AddForeignKey(
                name: "FK_Picture_Movie_MovieId",
                table: "Picture",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Picture_Movie_MovieId",
                table: "Picture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Picture",
                table: "Picture");

            migrationBuilder.RenameTable(
                name: "Picture",
                newName: "Image");

            migrationBuilder.RenameIndex(
                name: "IX_Picture_MovieId",
                table: "Image",
                newName: "IX_Image_MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Image",
                table: "Image",
                column: "Path");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Movie_MovieId",
                table: "Image",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
