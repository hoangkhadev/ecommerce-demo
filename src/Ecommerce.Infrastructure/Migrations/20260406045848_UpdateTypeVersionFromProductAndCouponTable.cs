using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTypeVersionFromProductAndCouponTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "row_version",
                table: "products");

            migrationBuilder.DropColumn(
                name: "row_version",
                table: "coupons");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "products",
                type: "xid",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "coupons",
                type: "xid",
                rowVersion: true,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "products");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "coupons");

            migrationBuilder.AddColumn<byte[]>(
                name: "row_version",
                table: "products",
                type: "bytea",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "row_version",
                table: "coupons",
                type: "bytea",
                rowVersion: true,
                nullable: true);
        }
    }
}
