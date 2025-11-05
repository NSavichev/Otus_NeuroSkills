using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TherapyService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciseDtos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    Instructions = table.Column<string[]>(type: "text[]", nullable: false),
                    RequiredMaterials = table.Column<string[]>(type: "text[]", nullable: false),
                    MinimumRecommendedAge = table.Column<int>(type: "integer", nullable: false),
                    ScientificBasis = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseDtos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseTest",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    Instructions = table.Column<string[]>(type: "text[]", nullable: false),
                    RequiredMaterials = table.Column<string[]>(type: "text[]", nullable: false),
                    MinimumRecommendedAge = table.Column<int>(type: "integer", nullable: false),
                    ScientificBasis = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseTest", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProgressDtos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    AverageDurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    PerceivedDifficulty = table.Column<int>(type: "integer", nullable: false),
                    CurrentBenefits = table.Column<string[]>(type: "text[]", nullable: false),
                    EffectivenessRating = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressDtos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProgressTest",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    AverageDurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    PerceivedDifficulty = table.Column<int>(type: "integer", nullable: false),
                    CurrentBenefits = table.Column<string[]>(type: "text[]", nullable: false),
                    EffectivenessRating = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressTest", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseDtos");

            migrationBuilder.DropTable(
                name: "ExerciseTest");

            migrationBuilder.DropTable(
                name: "ProgressDtos");

            migrationBuilder.DropTable(
                name: "ProgressTest");
        }
    }
}
