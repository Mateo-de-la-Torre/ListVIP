using Application.Common.Interfaces;
using QRCoder;

namespace Infrastructure.Services;

public class QrService : IQrService
{
    public string GenerateBase64(string content)
    {
        using var qrGenerator = new QRCodeGenerator();
        var qrData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrData);
        var bytes = qrCode.GetGraphic(10);
        return Convert.ToBase64String(bytes);
    }
}
