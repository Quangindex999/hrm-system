using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace payroll_service.Migrations
{
    /// <inheritdoc />
    public partial class AddDependentCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "dependent_count",
                table: "payrolls",
                type: "INT",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "dependent_deduction",
                table: "payrolls",
                type: "DECIMAL(12,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dependent_count",
                table: "payrolls");

            migrationBuilder.DropColumn(
                name: "dependent_deduction",
                table: "payrolls");
        }
    }
}
