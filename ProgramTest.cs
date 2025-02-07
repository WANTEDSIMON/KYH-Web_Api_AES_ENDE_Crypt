using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class ProgramTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ProgramTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async void GetPassword_ShouldReturnGeneratedPassword()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/password");

        response.EnsureSuccessStatusCode();
        var password = await response.Content.ReadAsStringAsync();

        Assert.Contains("Generated Password:", password);
        Assert.True(password.Length > "Generated Password: ".Length);
    }

    [Fact]
    public async void Encode_ShouldReturnBase64EncodedString()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/encode?text=hello");

        response.EnsureSuccessStatusCode();
        var encodedText = await response.Content.ReadAsStringAsync();

        Assert.Contains("The input 'hello' is encoded successfully as", encodedText);
    }

    [Fact]
    public async void Decode_ShouldReturnDecodedString()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/decode?text=aGVsbG8=");

        response.EnsureSuccessStatusCode();
        var decodedText = await response.Content.ReadAsStringAsync();

        Assert.Contains("The input 'aGVsbG8=' is decoded successfully as 'hello'", decodedText);
    }

    [Fact]
    public async void Encode_ShouldReturnErrorForMissingText()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/encode");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var errorMessage = await response.Content.ReadAsStringAsync();

        Assert.Equal("Error: Missing text parameter.", errorMessage);
    }

    [Fact]
    public async void Decode_ShouldReturnErrorForMissingText()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/decode");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var errorMessage = await response.Content.ReadAsStringAsync();

        Assert.Equal("Error: Missing text parameter.", errorMessage);
    }

    [Fact]
    public async void Decode_ShouldReturnErrorForInvalidBase64String()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/decode?text=invalid_base64");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var errorMessage = await response.Content.ReadAsStringAsync();

        Assert.Equal("Error: Invalid Base64 string.", errorMessage);
    }
}