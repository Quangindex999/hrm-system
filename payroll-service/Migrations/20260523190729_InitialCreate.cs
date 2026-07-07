using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace payroll_service.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payrolls",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<string>(type: "VARCHAR(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    employee_name = table.Column<string>(type: "VARCHAR(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    base_salary = table.Column<decimal>(type: "DECIMAL(12,2)", nullable: false),
                    working_days = table.Column<int>(type: "INT", nullable: false),
                    leave_days = table.Column<int>(type: "INT", nullable: false),
                    bonus = table.Column<decimal>(type: "DECIMAL(12,2)", nullable: false),
                    deduction = table.Column<decimal>(type: "DECIMAL(12,2)", nullable: false),
                    final_salary = table.Column<decimal>(type: "DECIMAL(12,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    updated_at = table.Column<DateTime>(type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payrolls", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_payrolls_created_at",
                table: "payrolls",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_payrolls_employee_id",
                table: "payrolls",
                column: "employee_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payrolls");
        }
    }
}