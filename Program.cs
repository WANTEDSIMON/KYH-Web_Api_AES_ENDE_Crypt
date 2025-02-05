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

app.Run();

string GenerateRandomKey()
{
    byte[] keyBytes = new byte[32]; // AES-256 key size
    RandomNumberGenerator.Fill(keyBytes);
    return Convert.ToBase64String(keyBytes);
}