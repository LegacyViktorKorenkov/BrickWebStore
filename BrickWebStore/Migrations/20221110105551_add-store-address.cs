using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrickWebStore.Migrations
{
    public partial class addstoreaddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductImage",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProductDescription",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ShopAddressId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreAddressId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "StoreAddressId",
                table: "Product");

            migrationBuilder.AlterColumn<string>(
                name: "ProductImage",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductDescription",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
