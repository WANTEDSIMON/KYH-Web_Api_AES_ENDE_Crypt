using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

// Define Base64 settings globally
const int KEY_SIZE = 16; // Change to desired key size
const int DEFAULT_PASSWORD_LENGTH = 12; // Default password length

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello Earth 🌎!");

// Password generator endpoint
app.MapGet("/password", (HttpContext context) =>
{
    // Get password length from query (default to 12 if not provided)
    int length = context.Request.Query.ContainsKey("length") && int.TryParse(context.Request.Query["length"], out int customLength)
        ? customLength
        : DEFAULT_PASSWORD_LENGTH;

    string password = GenerateSecurePassword(length);
    return Results.Text($"Generated Password: {password}");
});

// Secure password generator function
string GenerateSecurePassword(int length)
{
    const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=[]{}|;:,.<>?";
    
    char[] password = new char[length];

    using (var rng = RandomNumberGenerator.Create())
    {
        byte[] randomBytes = new byte[length];

        rng.GetBytes(randomBytes);

        for (int i = 0; i < length; i++)
        {
            password[i] = chars[randomBytes[i] % chars.Length];
        }
    }

    return new string(password);
}

// Encode endpoint using Base64
app.MapGet("/encode", async (HttpContext context) =>
{
    if (!context.Request.Query.ContainsKey("text"))
        return Results.Text("Error: Missing text parameter.");

    string text = context.Request.Query["text"];
    string encodedText = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));

    return Results.Text($"The input '{text}' is encoded successfully as '{encodedText}'");
});

// Decode endpoint using Base64 (POST method)
app.MapGet("/decode", async (HttpContext context) =>
{
    if (!context.Request.Query.ContainsKey("text"))
        return Results.Text("Error: Missing text parameter.");

    string encodedText = context.Request.Query["text"];

    try
    {
        byte[] decodedBytes = Convert.FromBase64String(encodedText);
        string decodedText = Encoding.UTF8.GetString(decodedBytes);

        return Results.Text($"The input '{encodedText}' is decoded successfully as '{decodedText}'");
    }
    catch (FormatException ex)
    {
        return Results.Text("Error: Invalid Base64 string.");
    }
});

app.Run();