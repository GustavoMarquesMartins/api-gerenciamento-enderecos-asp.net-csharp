using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciamentoDeEndereco.Migrations
{
    /// <inheritdoc />
    public partial class atualizaDadosPasswordResetToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "PasswordResetTokens",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "Expiration",
                table: "PasswordResetTokens",
                newName: "expiration");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PasswordResetTokens",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PasswordResetTokens",
                newName: "email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "token",
                table: "PasswordResetTokens",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "expiration",
                table: "PasswordResetTokens",
                newName: "Expiration");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "PasswordResetTokens",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "PasswordResetTokens",
                newName: "UserId");
        }
    }
}
