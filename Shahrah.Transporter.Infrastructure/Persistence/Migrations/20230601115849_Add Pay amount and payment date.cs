using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shahrah.Transporter.Infrastructure.Persistence.Migrations
{
    public partial class AddPayamountandpaymentdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "CorrelationId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("f30cd555-97dc-4997-adbb-aee487bdba67"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("7623e122-8dd3-4823-93ae-6d861b25179d"));

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "OrderItems",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "OrderItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "CorrelationId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("7623e122-8dd3-4823-93ae-6d861b25179d"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("f30cd555-97dc-4997-adbb-aee487bdba67"));
        }
    }
}
