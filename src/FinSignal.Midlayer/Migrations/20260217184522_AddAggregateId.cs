using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinSignal.Midlayer.Migrations
{
    /// <inheritdoc />
    public partial class AddAggregateId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AggregateId",
                table: "SignalEvents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AggregateId",
                table: "SignalEvents");
        }
    }
}
