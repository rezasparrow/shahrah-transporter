using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shahrah.Transporter.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeDriveInfoNullableForCompanyVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DriverNationalCode",
                table: "Vehicles",
                type: "char(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(10)");

            migrationBuilder.AlterColumn<string>(
                name: "DriverLastName",
                table: "Vehicles",
                type: "nvarchar(128)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)");

            migrationBuilder.AlterColumn<string>(
                name: "DriverFirstName",
                table: "Vehicles",
                type: "nvarchar(64)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)");

            migrationBuilder.AlterColumn<Guid>(
                name: "CorrelationId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("e43aaffe-f32c-4fce-9541-6a8b1ba53551"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("f7d2271b-8f09-4238-a636-be923bad042c"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DriverNationalCode",
                table: "Vehicles",
                type: "char(10)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "char(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DriverLastName",
                table: "Vehicles",
                type: "nvarchar(128)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DriverFirstName",
                table: "Vehicles",
                type: "nvarchar(64)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CorrelationId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("f7d2271b-8f09-4238-a636-be923bad042c"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValue: new Guid("e43aaffe-f32c-4fce-9541-6a8b1ba53551"));
        }
    }
}
