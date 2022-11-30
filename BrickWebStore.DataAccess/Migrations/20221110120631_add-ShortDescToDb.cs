using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrickWebStore.DataAccess.Migrations
{
    public partial class addShortDescToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_BrickWebStoreModel_ShopAddressId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ShopAddressId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ShopAddressId",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "ShortDesk",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_StoreAddressId",
                table: "Product",
                column: "StoreAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_BrickWebStoreModel_StoreAddressId",
                table: "Product",
                column: "StoreAddressId",
                principalTable: "BrickWebStoreModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_BrickWebStoreModel_StoreAddressId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_StoreAddressId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ShortDesk",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "ShopAddressId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ShopAddressId",
                table: "Product",
                column: "ShopAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_BrickWebStoreModel_ShopAddressId",
                table: "Product",
                column: "ShopAddressId",
                principalTable: "BrickWebStoreModel",
                principalColumn: "Id");
        }
    }
}
