using Application.Auth.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Auth;

public class ResendEmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ResendEmailService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<bool> SendInvitationAsync(string email, string token, decimal? commission, string eventoName, DateTime eventoDate, string eventoLocation, decimal eventoTicketPrice)
    {
        var apiKey = _configuration["Resend:ApiKey"]!;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var commissionText = commission.HasValue
            ? $"<p>Tu comisión es del <b>{commission.Value}%</b> por persona — ${eventoTicketPrice * commission.Value / 100:F2} por cada entrada.</p>"
            : string.Empty;

        var html = $@"
            <h2>¡Fuiste invitado como promotor a un evento en ListVIP!</h2>
            <h3>{eventoName}</h3>
            <p>📅 <b>Fecha:</b> {eventoDate:dd/MM/yyyy HH:mm}</p>
            <p>📍 <b>Lugar:</b> {eventoLocation}</p>
            <p>🎟️ <b>Precio de entrada:</b> ${eventoTicketPrice:F2}</p>
            {commissionText}
            <hr/>
            <p>Para registrarte usá el siguiente token (válido por 48 horas):</p>
            <p style='font-size:18px'><b>{token}</b></p>";

        var body = new
        {
            from = "onboarding@resend.dev",
            to = new[] { email },
            subject = $"Invitación a ListVIP — {eventoName}",
            html
        };

        var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://api.resend.com/emails", content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Resend error {(int)response.StatusCode}: {responseBody}");

        return true;
    }

    public async Task<bool> SendGuestQrAsync(string email, string guestName, string qrBase64, string eventoName, DateTime eventoDate, string eventoLocation)
    {
        var apiKey = _configuration["Resend:ApiKey"]!;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var html = $@"
            <h2>¡Tu acceso a {eventoName} está confirmado!</h2>
            <p>Hola <b>{guestName}</b>, presentá este QR en la puerta del evento.</p>
            <p>📅 <b>Fecha:</b> {eventoDate:dd/MM/yyyy HH:mm}</p>
            <p>📍 <b>Lugar:</b> {eventoLocation}</p>
            <br/>
            <img src='cid:qr_code' width='250' height='250' />";

        var body = new
        {
            from = "onboarding@resend.dev",
            to = new[] { email },
            subject = $"Tu QR de acceso — {eventoName}",
            html,
            attachments = new[]
            {
                new
                {
                    filename = "qr.png",
                    content = qrBase64,
                    content_id = "qr_code"
                }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://api.resend.com/emails", content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Resend error {(int)response.StatusCode}: {responseBody}");

        return true;
    }
}