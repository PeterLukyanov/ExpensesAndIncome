using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpensesAndIncome.Migrations
{
    /// <inheritdoc />
    public partial class RemakeModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TypesOfExpenses");

            migrationBuilder.DropTable(
                name: "TypesOfIncomes");

            migrationBuilder.RenameColumn(
                name: "TypeOfIncomes",
                table: "Incomes",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "TypeOfExpenses",
                table: "Expenses",
                newName: "Type");

            migrationBuilder.CreateTable(
                name: "NamesTypesOfExpenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NamesTypesOfExpenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NamesTypesOfIncomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NamesTypesOfIncomes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NamesTypesOfExpenses");

            migrationBuilder.DropTable(
                name: "NamesTypesOfIncomes");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Incomes",
                newName: "TypeOfIncomes");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Expenses",
                newName: "TypeOfExpenses");

            migrationBuilder.CreateTable(
                name: "TypesOfExpenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesOfExpenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypesOfIncomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesOfIncomes", x => x.Id);
                });
        }
    }
}
