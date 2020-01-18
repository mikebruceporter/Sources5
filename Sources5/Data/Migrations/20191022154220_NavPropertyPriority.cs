using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Sources5.Data.Migrations
{
    public partial class NavPropertyPriority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_PriorityId",
                table: "ProjectTasks",
                column: "PriorityId");

            // remove foreign key creation before updating the database!
            // leave this:  public Priority Priority { get; set; } in 
            // ProjectTask model! We need it.
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_PriorityId",
                table: "ProjectTasks");
        }
    }
}
