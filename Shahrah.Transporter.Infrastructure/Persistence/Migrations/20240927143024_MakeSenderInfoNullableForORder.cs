using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shahrah.Transporter.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeSenderInfoNullableForORder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SenderName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SenderMobileNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Orders",
                type: "nvarchar(1024)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)");

            migrationBuilder.AlterColumn<Guid>(
                name: "CorrelationId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("c95dc008-43da-45c6-8acc-a2b252cf4899"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("e43aaffe-f32c-4fce-9541-6a8b1ba53551"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SenderName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SenderMobileNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Orders",
                type: "nvarchar(1024)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CorrelationId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("e43aaffe-f32c-4fce-9541-6a8b1ba53551"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("c95dc008-43da-45c6-8acc-a2b252cf4899"));
        }
    }
}
