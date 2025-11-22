using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookVerseApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedTimestampToBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add columns
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Categories",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Books",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Authors",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Authors",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()");

            // Update existing rows to have current UTC timestamp
            migrationBuilder.Sql("UPDATE Authors SET CreatedAtUtc = GETUTCDATE(), UpdatedAtUtc = GETUTCDATE()");
            migrationBuilder.Sql("UPDATE Books SET CreatedAtUtc = GETUTCDATE(), UpdatedAtUtc = GETUTCDATE()");
            migrationBuilder.Sql("UPDATE Categories SET CreatedAtUtc = GETUTCDATE(), UpdatedAtUtc = GETUTCDATE()");
            migrationBuilder.Sql("UPDATE AspNetUsers SET CreatedAtUtc = GETUTCDATE(), UpdatedAtUtc = GETUTCDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "AspNetUsers");
        }
    }
}
