using System.Text.Json;
using System.Text.RegularExpressions;
using VCard.Exceptions;
using VCard.Models;
using VCard.Services;

namespace VCard
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            var continueProgram = true;

            while (continueProgram)
            {
                try
                {
                    Console.WriteLine("Seçim edin:");
                    Console.WriteLine("1 - Yeni VCard yarad");
                    Console.WriteLine("2 - Hazır VCard yüklə");
                    Console.WriteLine("0 - Proqramı bağla");

                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            // manual data entry and save to file
                            PerformManualDataEntry();
                            break;

                        case "2":
                            // random data usage and save to file
                            await PerformRandomDataUsage();
                            break;

                        case "0":
                            // exit program
                            continueProgram = false;
                            break;

                        default:
                            Console.WriteLine("Düzgün seçim edin!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Xəta baş verdi: {ex.Message}");
                }
            }
        }
        
        
        // manual data entry and save to file
        private static void PerformManualDataEntry()
        {
            var firstName = GetUserInput("Adı");
            var surname = GetUserInput("Soyadı");
            var email = GetValidatedEmail();
            var phone = GetUserInput("Telefon");
            var country = GetUserInput("Ölkə");
            var city = GetUserInput("Şəhər");

            var user = new CardInfo
            {
                Firstname = firstName,
                Surname = surname,
                Email = email,
                Phone = phone,
                Country = country,
                City = city
            };
            
            VCardService.SaveVCardAndQrCodeToFolder(user);
            Console.WriteLine($"{firstName} {surname} adlı şəxs üçün VCard yaradıldı.");
        }

        
        // get random user data from api and save to file
        private static async Task PerformRandomDataUsage()
        {
            Console.WriteLine("Yaradılacaq VCard sayını daxil edin:");
            var countInput = Console.ReadLine();

            if (!int.TryParse(countInput, out var count) || count <= 0)
            {
                Console.WriteLine("Düzgün say daxil edin!");
                return;
            }

            var randomUserService = new RandomUserService(count);
            var userData = await randomUserService.GetRandomUserData();

            if (userData == null)
                throw new NotFoundException("İstifadəçi məlumatı tapılmadı");

            var users = FromJsonDeserialize(userData, count);

            if (users == null)
                throw new NotFoundException("İstifadəçi məlumatı tapılmadı");

            foreach (var user in users)
            {
                
                VCardService.SaveVCardAndQrCodeToFolder(user);
            }

            Console.WriteLine($"{count} ədəd VCard yaradıldı.");
        }

        // get user input from console and validate
        private static string GetUserInput(string fieldName)
        {
            while (true)
            {
                Console.Write($"{fieldName}: ");
                var userInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(userInput))
                {
                    return userInput;
                }

                Console.WriteLine($"{fieldName} boş ola bilməz!");
                
            }
        }

        // validate email
        private static string GetValidatedEmail()
        {
            while (true)
            {
                var email = GetUserInput("E-Poçt");
                if (IsValidEmail(email))
                {
                    return email;
                }

                Console.WriteLine("Düzgün e-poçt daxil edin!");
            }
        }
        
        private static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");
            return emailRegex.IsMatch(email);
        }

        
        // deserialize json data
        private static List<CardInfo> FromJsonDeserialize(string jsonData, int count)
        {
            var cardInfos = new List<CardInfo>();
            var jsonObject = JsonDocument.Parse(jsonData).RootElement.GetProperty("results");

            for (var i = 0; i < count; i++)
            {
                var cardInfo = new CardInfo
                {
                    Firstname = jsonObject[i].GetProperty("name").GetProperty("first").GetString(),
                    Surname = jsonObject[i].GetProperty("name").GetProperty("last").GetString(),
                    Email = jsonObject[i].GetProperty("email").GetString(),
                    Phone = jsonObject[i].GetProperty("phone").GetString(),
                    Country = jsonObject[i].GetProperty("location").GetProperty("country").GetString(),
                    City = jsonObject[i].GetProperty("location").GetProperty("city").GetString()
                };
                cardInfos.Add(cardInfo);
            }

            return cardInfos;
        }
    }
}
