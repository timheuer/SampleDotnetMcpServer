using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

/// <summary>
/// Weather-related MCP tools for checking weather information.
/// These tools can be invoked by MCP clients to get weather data.
/// </summary>
internal class WeatherTools
{
    private readonly HttpClient _httpClient;

    public WeatherTools(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [McpServerTool]
    [Description("Gets current weather information for a specific US zip code.")]
    public async Task<IEnumerable<ContentBlock>> GetWeatherByZipCode(
        [Description("5-digit US zip code (e.g., 90210)")] string zipCode)
    {
        List<ContentBlock> contents = new();

        // Validate zip code format
        if (string.IsNullOrWhiteSpace(zipCode) || zipCode.Length != 5 || !zipCode.All(char.IsDigit))
        {
            contents.Add(new TextContentBlock()
            {
                Annotations = new()
                {
                    Audience = [Role.User, Role.Assistant],
                    Priority = 1.0f
                },
                Text = "Error: Please provide a valid 5-digit US zip code."
            });
            return contents;
        }

        try
        {
            // For this demo, we'll use the free wttr.in service which doesn't require an API key
            // Format: https://wttr.in/{location}?format=j1
            var response = await _httpClient.GetAsync($"https://wttr.in/{zipCode}?format=j1");

            if (!response.IsSuccessStatusCode)
            {
                contents.Add(new TextContentBlock()
                {
                    Annotations = new()
                    {
                        Audience = [Role.User, Role.Assistant],
                        Priority = 1.0f
                    },
                    Text = $"Error: Unable to fetch weather data for zip code {zipCode}. Status: {response.StatusCode}"
                });
                return contents;
            }

            var jsonContent = await response.Content.ReadAsStringAsync();

            // Parse the JSON response to extract key weather information
            using var document = JsonDocument.Parse(jsonContent);
            var root = document.RootElement;

            // Extract current conditions
            var currentCondition = root.GetProperty("current_condition")[0];
            var temp_F = currentCondition.GetProperty("temp_F").GetString();
            var temp_C = currentCondition.GetProperty("temp_C").GetString();
            var humidity = currentCondition.GetProperty("humidity").GetString();
            var weatherDesc = currentCondition.GetProperty("weatherDesc")[0].GetProperty("value").GetString();
            var feelsLike_F = currentCondition.GetProperty("FeelsLikeF").GetString();
            var feelsLike_C = currentCondition.GetProperty("FeelsLikeC").GetString();
            var windSpeed = currentCondition.GetProperty("windspeedMiles").GetString();
            var windDir = currentCondition.GetProperty("winddir16Point").GetString();

            // Get location info
            var nearestArea = root.GetProperty("nearest_area")[0];
            var areaName = nearestArea.GetProperty("areaName")[0].GetProperty("value").GetString();
            var region = nearestArea.GetProperty("region")[0].GetProperty("value").GetString();

            // Format the response
            var weatherReport = $"Weather Report for {zipCode} ({areaName}, {region}):\n\n" +
                $"🌡️  Temperature: {temp_F}°F ({temp_C}°C)\n" +
                $"🌡️  Feels Like: {feelsLike_F}°F ({feelsLike_C}°C)\n" +
                $"☁️  Conditions: {weatherDesc}\n" +
                $"💧 Humidity: {humidity}%\n" +
                $"💨 Wind: {windSpeed} mph {windDir}\n";

            contents.Add(new TextContentBlock()
            {
                Annotations = new()
                {
                    Audience = [Role.User, Role.Assistant],
                    Priority = 1.0f
                },
                Text = weatherReport
            });

            contents.Add(new TextContentBlock()
            {
                Annotations = new()
                {
                    Audience = [Role.User],
                    Priority = 0.7f
                },
                Text = "Data provided by wttr.in"
            });

            return contents;
        }
        catch (HttpRequestException ex)
        {
            contents.Add(new TextContentBlock()
            {
                Annotations = new()
                {
                    Audience = [Role.User, Role.Assistant],
                    Priority = 1.0f
                },
                Text = $"Error: Network error while fetching weather data - {ex.Message}"
            });
            return contents;
        }
        catch (JsonException ex)
        {
            contents.Add(new TextContentBlock()
            {
                Annotations = new()
                {
                    Audience = [Role.User, Role.Assistant],
                    Priority = 1.0f
                },
                Text = $"Error: Failed to parse weather data - {ex.Message}"
            });
            return contents;
        }
        catch (Exception ex)
        {
            contents.Add(new TextContentBlock()
            {
                Annotations = new()
                {
                    Audience = [Role.User, Role.Assistant],
                    Priority = 1.0f
                },
                Text = $"Error: An unexpected error occurred - {ex.Message}"
            });
            return contents;
        }
    }

    [McpServerTool]
    [Description("Gets a simple weather forecast for the next few days for a specific US zip code.")]
    public async Task<IEnumerable<ContentBlock>> GetWeatherForecast(
        [Description("5-digit US zip code (e.g., 90210)")] string zipCode,
        [Description("Number of days to forecast (1-3 days)")] int days = 3)
    {
        List<ContentBlock> contents = new();

        // Validate inputs
        if (string.IsNullOrWhiteSpace(zipCode) || zipCode.Length != 5 || !zipCode.All(char.IsDigit))
        {
            contents.Add(new TextContentBlock()
            {
                Annotations = new()
                {
                    Audience = [Role.User, Role.Assistant],
                    Priority = 1.0f
                },
                Text = "Error: Please provide a valid 5-digit US zip code."
            });
        }

        if (days < 1 || days > 3)
        {
            contents.Add(new TextContentBlock()
            {
                Annotations = new()
                {
                    Audience = [Role.User, Role.Assistant],
                    Priority = 1.0f
                },
                Text = "Error: Forecast days must be between 1 and 3."
            });
        }

        try
        {
            var response = await _httpClient.GetAsync($"https://wttr.in/{zipCode}?format=j1");

            if (!response.IsSuccessStatusCode)
            {
                contents.Add(new TextContentBlock()
                {
                    Annotations = new()
                    {
                        Audience = [Role.User, Role.Assistant],
                        Priority = 1.0f
                    },
                    Text = $"Error: Unable to fetch weather data for zip code {zipCode}. Status: {response.StatusCode}"
                });
            }

            var jsonContent = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(jsonContent);
            var root = document.RootElement;

            // Get location info
            var nearestArea = root.GetProperty("nearest_area")[0];
            var areaName = nearestArea.GetProperty("areaName")[0].GetProperty("value").GetString();
            var region = nearestArea.GetProperty("region")[0].GetProperty("value").GetString();

            var forecast = $"Weather Forecast for {zipCode} ({areaName}, {region}):\n\n";

            var weather = root.GetProperty("weather");

            for (int i = 0; i < Math.Min(days, weather.GetArrayLength()); i++)
            {
                var day = weather[i];
                var date = day.GetProperty("date").GetString();
                var maxTemp_F = day.GetProperty("maxtempF").GetString();
                var minTemp_F = day.GetProperty("mintempF").GetString();
                var maxTemp_C = day.GetProperty("maxtempC").GetString();
                var minTemp_C = day.GetProperty("mintempC").GetString();

                // Get the weather description for the day (using the middle time slot)
                var hourly = day.GetProperty("hourly");
                var midDayWeather = hourly[hourly.GetArrayLength() / 2];
                var weatherDesc = midDayWeather.GetProperty("weatherDesc")[0].GetProperty("value").GetString();

                var dayName = DateTime.Parse(date ?? DateTime.Today.ToString("yyyy-MM-dd")).ToString("dddd, MMM d");

                forecast += $"📅 {dayName}:\n";
                forecast += $"   🌡️  High: {maxTemp_F}°F ({maxTemp_C}°C)\n";
                forecast += $"   🌡️  Low: {minTemp_F}°F ({minTemp_C}°C)\n";
                forecast += $"   ☁️  Conditions: {weatherDesc}\n\n";
            }

            contents.Add(new TextContentBlock()
            {
                Annotations = new()
                {
                    Audience = [Role.User, Role.Assistant],
                    Priority = 1.0f
                },
                Text = forecast
            });

            contents.Add(new TextContentBlock()
            {
                Annotations = new()
                {
                    Audience = [Role.User],
                    Priority = 0.7f
                },
                Text = "Data provided by wttr.in"
            });
        }
        catch (HttpRequestException ex)
        {
            contents.Add(new TextContentBlock()
            {
                Annotations = new()
                {
                    Audience = [Role.User, Role.Assistant],
                    Priority = 1.0f
                },
                Text = $"Error: Network error while fetching weather forecast - {ex.Message}"
            });
        }
        catch (JsonException ex)
        {
            contents.Add(new TextContentBlock()
            {
                Annotations = new()
                {
                    Audience = [Role.User, Role.Assistant],
                    Priority = 1.0f
                },
                Text = $"Error: Failed to parse weather forecast data - {ex.Message}"
            });
        }
        catch (Exception ex)
        {
            contents.Add(new TextContentBlock()
            {
                Annotations = new()
                {
                    Audience = [Role.User, Role.Assistant],
                    Priority = 1.0f
                },
                Text = $"Error: An unexpected error occurred - {ex.Message}"
            });
        }

        return contents;
    }
}