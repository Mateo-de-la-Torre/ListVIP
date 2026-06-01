using Application.Auth.Interfaces;
using Domain.Enums;
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

    public async Task<bool> SendInvitationAsync(string email, string token, Role role)
    {
        var apiKey = _configuration["Resend:ApiKey"]!;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var body = new
        {
            from = "onboarding@resend.dev",
            to = new[] { email },
            subject = "Invitación a ListVIP",
            html = $"<p>Fuiste invitado como <b>{role}</b>. Tu token de registro es: <b>{token}</b></p>"
        };

        var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://api.resend.com/emails", content);

        return response.IsSuccessStatusCode;
    }
}