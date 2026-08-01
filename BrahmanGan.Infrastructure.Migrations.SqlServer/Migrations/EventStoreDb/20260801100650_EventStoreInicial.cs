using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrahmanGan.Infrastructure.Migrations.SqlServer.Migrations.EventStoreDb
{
    /// <inheritdoc />
    public partial class EventStoreInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DomainEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AggregateType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AggregateId = table.Column<int>(type: "int", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EventData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OccurredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainEvents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DomainEvents_AggregateType_AggregateId",
                table: "DomainEvents",
                columns: new[] { "AggregateType", "AggregateId" });

            migrationBuilder.CreateIndex(
                name: "IX_DomainEvents_EventId",
                table: "DomainEvents",
                column: "EventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DomainEvents_EventType",
                table: "DomainEvents",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_DomainEvents_OccurredOn",
                table: "DomainEvents",
                column: "OccurredOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DomainEvents");
        }
    }
}
