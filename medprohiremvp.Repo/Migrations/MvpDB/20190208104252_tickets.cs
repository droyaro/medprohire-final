using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace medprohiremvp.Repo.Migrations.MvpDB
{
    public partial class tickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketCategories",
                columns: table => new
                {
                    TicketCategory_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TicketCategoryName = table.Column<string>(nullable: true),
                    IsClient = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketCategories", x => x.TicketCategory_ID);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Ticket_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Applicant_ID = table.Column<int>(nullable: false),
                    TicketCategory_ID = table.Column<int>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    TicketType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Ticket_ID);
                    table.ForeignKey(
                        name: "FK_Tickets_Applicants_Applicant_ID",
                        column: x => x.Applicant_ID,
                        principalTable: "Applicants",
                        principalColumn: "Applicant_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketCategories_TicketCategory_ID",
                        column: x => x.TicketCategory_ID,
                        principalTable: "TicketCategories",
                        principalColumn: "TicketCategory_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketContents",
                columns: table => new
                {
                    TicketContent_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ticket_ID = table.Column<int>(nullable: false),
                    User_ID = table.Column<Guid>(nullable: false),
                    TicketContent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketContents", x => x.TicketContent_ID);
                    table.ForeignKey(
                        name: "FK_TicketContents_Tickets_Ticket_ID",
                        column: x => x.Ticket_ID,
                        principalTable: "Tickets",
                        principalColumn: "Ticket_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketContents_Ticket_ID",
                table: "TicketContents",
                column: "Ticket_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Applicant_ID",
                table: "Tickets",
                column: "Applicant_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketCategory_ID",
                table: "Tickets",
                column: "TicketCategory_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketContents");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "TicketCategories");
        }
    }
}
