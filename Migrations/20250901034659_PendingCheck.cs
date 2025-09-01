using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerceAPI.Migrations
{
    /// <inheritdoc />
    public partial class PendingCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_SellerId1",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SellerId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SellerId1",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "SellerId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SellerId",
                table: "Products",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_SellerId",
                table: "Products",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AspNetUsers_SellerId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SellerId",
                table: "Products");

            migrationBuilder.AlterColumn<Guid>(
                name: "SellerId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "SellerId1",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SellerId1",
                table: "Products",
                column: "SellerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AspNetUsers_SellerId1",
                table: "Products",
                column: "SellerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
