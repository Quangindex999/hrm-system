using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace payroll_service.Migrations
{
    /// <inheritdoc />
    public partial class AddInsuranceAndTax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "bhtn_employee",
                table: "payrolls",
                type: "DECIMAL(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "bhtn_employer",
                table: "payrolls",
                type: "DECIMAL(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "bhxh_employee",
                table: "payrolls",
                type: "DECIMAL(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "bhxh_employer",
                table: "payrolls",
                type: "DECIMAL(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "bhyt_employee",
                table: "payrolls",
                type: "DECIMAL(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "bhyt_employer",
                table: "payrolls",
                type: "DECIMAL(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "gross_income",
                table: "payrolls",
                type: "DECIMAL(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "overtime_pay",
                table: "payrolls",
                type: "DECIMAL(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "personal_tax",
                table: "payrolls",
                type: "DECIMAL(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "taxable_income",
                table: "payrolls",
                type: "DECIMAL(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "total_deduction",
                table: "payrolls",
                type: "DECIMAL(12,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bhtn_employee",
                table: "payrolls");

            migrationBuilder.DropColumn(
                name: "bhtn_employer",
                table: "payrolls");

            migrationBuilder.DropColumn(
                name: "bhxh_employee",
                table: "payrolls");

            migrationBuilder.DropColumn(
                name: "bhxh_employer",
                table: "payrolls");

            migrationBuilder.DropColumn(
                name: "bhyt_employee",
                table: "payrolls");

            migrationBuilder.DropColumn(
                name: "bhyt_employer",
                table: "payrolls");

            migrationBuilder.DropColumn(
                name: "gross_income",
                table: "payrolls");

            migrationBuilder.DropColumn(
                name: "overtime_pay",
                table: "payrolls");

            migrationBuilder.DropColumn(
                name: "personal_tax",
                table: "payrolls");

            migrationBuilder.DropColumn(
                name: "taxable_income",
                table: "payrolls");

            migrationBuilder.DropColumn(
                name: "total_deduction",
                table: "payrolls");
        }
    }
}
