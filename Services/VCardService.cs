using System.Text;
using QRCoder;
using VCard.Models;

namespace VCard.Services;

public abstract class VCardService
{
    
    // save vcard and qrcode to desktop
    public static void SaveVCardAndQrCodeToFolder(CardInfo vcard)
    {
        var folderName = $"{vcard.Firstname} {vcard.Surname} {vcard.Id}";
        var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), folderName);
        Directory.CreateDirectory(folderPath);
        SaveVCardToFile(vcard, folderPath);
        SaveQrCodeToFile(vcard, folderPath);
    }

    // save vcard as vcf file to desktop
    private static void SaveVCardToFile(CardInfo vcard, string folderPath)
    {
        var vcardTextBuilder = new StringBuilder();
        vcardTextBuilder.AppendLine("BEGIN:VCARD");
        vcardTextBuilder.AppendLine("VERSION:3.0");
        vcardTextBuilder.AppendLine($"FN:{vcard.Firstname} {vcard.Surname}");
        vcardTextBuilder.AppendLine($"EMAIL:{vcard.Email}");
        vcardTextBuilder.AppendLine($"TEL:{vcard.Phone}");
        vcardTextBuilder.AppendLine($"ADR:{vcard.Country} {vcard.City}");
        vcardTextBuilder.AppendLine("END:VCARD");

        var filePath = Path.Combine(folderPath, $"{vcard.Firstname} {vcard.Surname} {vcard.Id}.vcf");
        File.WriteAllText(filePath, vcardTextBuilder.ToString());
    }

    
    // save qrcode as png file to desktop
    private static void SaveQrCodeToFile(CardInfo vcard, string folderPath)
    {
        var qrGenerator = new QRCodeGenerator();
        var vcardText =
            $"BEGIN:VCARD\n" +
            $"VERSION:3.0\n" +
            $"FN:{vcard.Firstname} {vcard.Surname}\n" +
            $"EMAIL:{vcard.Email}\n" +
            $"TEL:{vcard.Phone}\n" +
            $"ADR:{vcard.Country} {vcard.City}\n" +
            $"END:VCARD";
        var qrCodeData = qrGenerator.CreateQrCode(vcardText, QRCodeGenerator.ECCLevel.Q);

        var qrCode = new PngByteQRCode(qrCodeData);
        var qrCodeFilePath = Path.Combine(folderPath, $"{vcard.Firstname}  {vcard.Surname} {vcard.Id}_qrcode.png");
        File.WriteAllBytes(qrCodeFilePath, qrCode.GetGraphic(20));
    }
}