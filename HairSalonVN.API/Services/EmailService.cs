using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HairSalonVN.API.Services.Repositories.Interfaces;
using HairSalonVN.Database.Entities;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;

namespace HairSalonVN.API.Services
{
    public class EmailService
    {
        private readonly IRepository<Appointment> _apptRepo;
        private readonly IConfiguration _cfg;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IRepository<Appointment> apptRepo,
            IConfiguration cfg,
            ILogger<EmailService> logger)
        {
            _apptRepo = apptRepo;
            _cfg = cfg;
            _logger = logger;
        }

        public Task SendBookingConfirmationAsync(Guid appointmentId)
            => Task.Run(() => SendBookingConfirmationInternal(appointmentId));

        public Task SendStatusUpdateAsync(Guid appointmentId, string status)
            => Task.Run(() => SendStatusUpdateInternal(appointmentId, status));

        private void SendBookingConfirmationInternal(Guid appointmentId)
        {
            try
            {
                var appt = _apptRepo.FindAsync(a => a.Id == appointmentId).Result.FirstOrDefault();
                if (appt == null) return;

                var toEmail = appt.IsGuestBooking ? appt.GuestEmail : null;
                if (string.IsNullOrWhiteSpace(toEmail)) return;

                var serviceName = appt.Service?.Name ?? "N/A";
                var dateStr = appt.AppointmentDate.ToString("dd/MM/yyyy");
                var timeStr = appt.AppointmentDate.ToString("HH:mm");
                var customerName = appt.IsGuestBooking ? (appt.GuestName ?? "Quý khách") : "Quý khách";
                var subject = "Xac nhan dat lich #" + appt.BookingCode;
                var body = "Xin chao " + customerName + ",\n\n"
                    + "Ban da dat lich thanh cong tai LUXE Hair Salon!\n\n"
                    + "Ma dat lich: " + appt.BookingCode + "\n"
                    + "Dich vu: " + serviceName + "\n"
                    + "Ngay: " + dateStr + "\n"
                    + "Gio: " + timeStr + "\n\n"
                    + "Cam on ban da tin tuong LUXE Hair Salon!\n\n"
                    + "Tran trong,\nLUXE Hair Salon";

                SendEmail(toEmail, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send booking confirmation for {AppointmentId}", appointmentId);
            }
        }

        private void SendStatusUpdateInternal(Guid appointmentId, string status)
        {
            try
            {
                var appt = _apptRepo.FindAsync(a => a.Id == appointmentId).Result.FirstOrDefault();
                if (appt == null) return;

                var toEmail = appt.IsGuestBooking ? appt.GuestEmail : null;
                if (string.IsNullOrWhiteSpace(toEmail)) return;

                var statusText = status == "Confirmed" ? "da duoc xac nhan"
                    : status == "Completed" ? "da hoan thanh"
                    : status == "Cancelled" ? "da bi huy"
                    : "co trang thai: " + status;

                var customerName = appt.IsGuestBooking ? (appt.GuestName ?? "Quy khach") : "Quy khach";
                var dateStr = appt.AppointmentDate.ToString("dd/MM/yyyy");
                var timeStr = appt.AppointmentDate.ToString("HH:mm");
                var subject = "Cap nhat lich hen #" + appt.BookingCode;
                var body = "Xin chao " + customerName + ",\n\n"
                    + "Lich hen #" + appt.BookingCode + " cua ban " + statusText + ".\n\n"
                    + "Ngay: " + dateStr + "\n"
                    + "Gio: " + timeStr + "\n\n"
                    + "Neu co thac mac, vui long lien he voi chung toi.\n\n"
                    + "Tran trong,\nLUXE Hair Salon";

                SendEmail(toEmail, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send status update for {AppointmentId}", appointmentId);
            }
        }

        private void SendEmail(string to, string subject, string body)
        {
            var smtpHost = _cfg["MailSettings:Host"];
            var smtpPort = int.TryParse(_cfg["MailSettings:Port"], out var port) ? port : 587;
            var smtpUser = _cfg["MailSettings:Username"];
            var smtpPass = _cfg["MailSettings:Password"];
            var fromEmail = _cfg["MailSettings:FromEmail"] ?? smtpUser;
            var fromName = _cfg["MailSettings:FromName"] ?? "LUXE Hair Salon";

            if (string.IsNullOrWhiteSpace(smtpHost) || string.IsNullOrWhiteSpace(smtpUser))
            {
                _logger.LogWarning("Email not configured - skipping send to {To}", to);
                return;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            try
            {
                using var client = new SmtpClient();
                client.Connect(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate(smtpUser, smtpPass);
                client.Send(message);
                client.Disconnect(true);
                _logger.LogInformation("Email sent to {To}: {Subject}", to, subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", to);
            }
        }
    }

    public class EmailBackgroundHandler : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
