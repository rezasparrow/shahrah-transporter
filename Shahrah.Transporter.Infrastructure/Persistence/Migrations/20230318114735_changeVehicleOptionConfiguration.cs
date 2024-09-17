using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shahrah.Transporter.Infrastructure.Persistence.Migrations
{
    public partial class changeVehicleOptionConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VehicleOptionItems_VehicleId_OptionItemId",
                table: "VehicleOptionItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "CorrelationId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("7623e122-8dd3-4823-93ae-6d861b25179d"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("df84e2f6-80e1-4b26-9d75-52858375d6ac"));

            migrationBuilder.CreateIndex(
                name: "IX_VehicleOptionItems_VehicleId_OptionItemId",
                table: "VehicleOptionItems",
                columns: new[] { "VehicleId", "OptionItemId" },
                unique: true,
                filter: "[IsDeleted]=(0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VehicleOptionItems_VehicleId_OptionItemId",
                table: "VehicleOptionItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "CorrelationId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("df84e2f6-80e1-4b26-9d75-52858375d6ac"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("7623e122-8dd3-4823-93ae-6d861b25179d"));

            migrationBuilder.CreateIndex(
                name: "IX_VehicleOptionItems_VehicleId_OptionItemId",
                table: "VehicleOptionItems",
                columns: new[] { "VehicleId", "OptionItemId" },
                unique: true);
        }
    }
}
