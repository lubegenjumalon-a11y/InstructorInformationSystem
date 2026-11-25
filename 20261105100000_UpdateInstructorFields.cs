using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InstructorInfoSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInstructorFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address", table: "Instructors", type: "TEXT",
                maxLength: 200, nullable: false, defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Age", table: "Instructors", type: "INTEGER",
                nullable: false, defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BachelorDegree", table: "Instructors", type: "TEXT",
                maxLength: 150, nullable: false, defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber", table: "Instructors", type: "TEXT",
                maxLength: 20, nullable: false, defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Designation", table: "Instructors", type: "TEXT",
                maxLength: 100, nullable: false, defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName", table: "Instructors", type: "TEXT",
                maxLength: 100, nullable: false, defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender", table: "Instructors", type: "TEXT",
                maxLength: 20, nullable: false, defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Majorship", table: "Instructors", type: "TEXT",
                maxLength: 100, nullable: false, defaultValue: "");

            // Keep the names of existing instructors.
            migrationBuilder.Sql(
                "UPDATE Instructors SET FullName = TRIM(FirstName || ' ' || LastName);");

            migrationBuilder.DropColumn(name: "FirstName", table: "Instructors");
            migrationBuilder.DropColumn(name: "LastName", table: "Instructors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName", table: "Instructors", type: "TEXT",
                maxLength: 50, nullable: false, defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName", table: "Instructors", type: "TEXT",
                maxLength: 50, nullable: false, defaultValue: "");

            migrationBuilder.Sql("UPDATE Instructors SET FirstName = FullName;");

            migrationBuilder.DropColumn(name: "Address", table: "Instructors");
            migrationBuilder.DropColumn(name: "Age", table: "Instructors");
            migrationBuilder.DropColumn(name: "BachelorDegree", table: "Instructors");
            migrationBuilder.DropColumn(name: "ContactNumber", table: "Instructors");
            migrationBuilder.DropColumn(name: "Designation", table: "Instructors");
            migrationBuilder.DropColumn(name: "FullName", table: "Instructors");
            migrationBuilder.DropColumn(name: "Gender", table: "Instructors");
            migrationBuilder.DropColumn(name: "Majorship", table: "Instructors");
        }
    }
}
