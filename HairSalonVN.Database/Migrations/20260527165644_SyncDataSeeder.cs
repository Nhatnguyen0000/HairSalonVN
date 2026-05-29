using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairSalonVN.Database.Migrations
{
    /// <inheritdoc />
    public partial class SyncDataSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "FullName", "PasswordHash" },
                values: new object[] { "Nguyễn Minh Nhật", "$2a$11$C3jnmGxfQ8Rg7QGiC0x.fuD9x25cWsQtgtdfJ2FcZnEXLDTw/hVF6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "$2a$11$zmwVfwVZssmoFteDSp7HjejFACAp4mZyMTeZcNzKcftW.CwetP/Ku");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                column: "PasswordHash",
                value: "$2a$11$ISs4K54igroR4GALtz7IJuqKTV7fiz4WhXpRLAoqJE24.74RakBvW");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"),
                column: "PasswordHash",
                value: "$2a$11$nLQ.M5AfGALEkB0qXXvsUO0Nvf4Rjkh1xW06Wnjkb2nmsI1ZVInQe");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000020"),
                column: "PasswordHash",
                value: "$2a$11$9K8Qzdk1gGWoCXKbgm869uIpKH57Q5JJ7gcAcHhvlF8ZGUPO0mnxO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000021"),
                column: "PasswordHash",
                value: "$2a$11$18j0KR9hdXKopvLq3uiDie.qErlQxBzgW52BHdeyATihA44IgBkCu");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000022"),
                column: "PasswordHash",
                value: "$2a$11$XiNZWUIdUrStaSJUjS1Z5uFe3H0.n4Vkf40QR0S2Q.AdXf42HE7gu");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000023"),
                column: "PasswordHash",
                value: "$2a$11$OrVCd1CNl8dZY/P55ndQh.wA2X./H3SjGVTMhMH2OdIzhAMpL.xL2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000024"),
                column: "PasswordHash",
                value: "$2a$11$OctWat.BwgILDxgnZbsizuTEGe/1u/PhK.1TU.wR4aOy7hxODJgKG");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000025"),
                column: "PasswordHash",
                value: "$2a$11$7OF0JCPNGfJXZUIZomMdn.0cTlRDhmigNQz9.fsMJdtjo53u92OGS");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000026"),
                column: "PasswordHash",
                value: "$2a$11$2pICBzJquua7bPLvacWLkeH4l/1rAVALuJi7KFs5nF7iZxCZVwwly");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000027"),
                column: "PasswordHash",
                value: "$2a$11$cJWYMNh3EpHHQpkZcclJYOnv5y7.hl42LoGHLi57zd1CGzai.K9qq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000028"),
                column: "PasswordHash",
                value: "$2a$11$z14/slHDPLGYOGKubX1k3.U4UI.wLQkaEXSPSDE7AFEkVMmsi7ahe");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000029"),
                column: "PasswordHash",
                value: "$2a$11$cJrsfjHAXUnpqVP7zIbr2emxxiYPYynjUfo51BHpC5vZGktooJgBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "FullName", "PasswordHash" },
                values: new object[] { "Admin — Trần Văn Quản", "$2a$11$GZWzw9RdwFx8YXOnMUjnculZMCm.NegH54GZ6BYq/oyCqt0qM0JjS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"),
                column: "PasswordHash",
                value: "$2a$11$zkpFyZBVOnBblBaSu51xVOb04i8TAEx0tnDLHQac7x6C6bJfk5B96");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"),
                column: "PasswordHash",
                value: "$2a$11$ddvqpWXmCheKT.GBDmpsluNWdPinaSF.twXe8wTN1TXLLp6vDfVyK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"),
                column: "PasswordHash",
                value: "$2a$11$NL69sNdqnweriVabkq7ALeFq6VgOmeYCZ.TV/cgm1JVVv1YOUcC62");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000020"),
                column: "PasswordHash",
                value: "$2a$11$b0CpDl.qFSYYxc01cEA0TOacHUkRQU7OW23RGpuNrGmAKKTgthuc.");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000021"),
                column: "PasswordHash",
                value: "$2a$11$SATwcUDIC2BaJHA5FTPeKOF7A27APXARUEiU6obaNjTCKMz6081MK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000022"),
                column: "PasswordHash",
                value: "$2a$11$ZzmRlcYO7JOPEB1vknwADOg/BPR5YZdSDfJosnPlxafAe3bjGEl4S");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000023"),
                column: "PasswordHash",
                value: "$2a$11$b8qZoxwSI72WB2Oc9NG3wub7gpWw2ESkwl1yLgR6nMNl2JIOVH2Tm");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000024"),
                column: "PasswordHash",
                value: "$2a$11$vg5xH8JWAT2g.GqFKVFtlufbLcZGGdO/PieZuAWtHOzr20PRt3t/u");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000025"),
                column: "PasswordHash",
                value: "$2a$11$pY1G1j9xUDao2qizKldTVeMpyQRg0j4PDuRQmCHRQwq9fcw7RB5R2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000026"),
                column: "PasswordHash",
                value: "$2a$11$.6o38CO8ZPzWsI.iSFyOjuo0XqZlXWG0cMCznn0tvlGSK679GaNPC");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000027"),
                column: "PasswordHash",
                value: "$2a$11$x9CDVlyjk5q6dQMFmOvF6.XEuIcjp/HrlwjYtjUfy9nOOeA3j4asi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000028"),
                column: "PasswordHash",
                value: "$2a$11$gJTweringooQ5kTr5CIwAeel63JR9/FwbnzmX6rkRIeRuGqKp1JOC");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000029"),
                column: "PasswordHash",
                value: "$2a$11$Zy0kY12Pb7.0kLnxSgfoEOiaF1KHR6RJfUx.rkun0AhGXYDKl7tkm");
        }
    }
}
