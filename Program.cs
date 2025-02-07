using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello Earth 🌎!");

// Generate a new AES key on every request (refresh to get a new one)
app.MapGet("/key", () => GenerateRandomKey());


string GenerateRandomKey()
{
    byte[] keyBytes = new byte[32]; // AES-256 key size
    RandomNumberGenerator.Fill(keyBytes);
    return Convert.ToBase64String(keyBytes);
}

// Encrypt endpoint using AES
app.MapGet("/encrypt", (HttpContext context) =>
{
    if (!context.Request.Query.ContainsKey("text") || !context.Request.Query.ContainsKey("key"))
        return Results.Text("Error: Missing text or key parameters.");

    string text = context.Request.Query["text"];
    string key = context.Request.Query["key"];
    string encrypted = Encrypt(text, key);
    return Results.Text($"The input '{text}' with key '{key}' is encrypted as: {encrypted}");
});

// Encrypt text: /encrypt?text=HelloWorld&key=mySecretKey123456


string Encrypt(string text, string key)
{
    try
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key.PadRight(32)); // Ensure key is 32 bytes (AES-256)
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = new byte[16]; // Static IV (for now, should be random in production)

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var writer = new StreamWriter(cryptoStream))
                {
                    writer.Write(text);
                }
                string encryptedText = Convert.ToBase64String(ms.ToArray());
                return encryptedText;
            }
        }
    }
    catch (Exception ex)
    {
        return $"Error during encryption: {ex.Message}";
    }
}

app.Run();
