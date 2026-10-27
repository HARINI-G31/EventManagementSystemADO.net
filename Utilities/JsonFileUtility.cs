using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EventManagement.Utilities
{
    public class JsonFileUtility
    {
        private readonly JsonSerializerOptions options =
            new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

        public void SaveData<T>(string filePath, List<T> data)
        {
            try
            {
                string? directory = Path.GetDirectoryName(filePath);

                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(data, options);

                File.WriteAllText(filePath, json);

                Console.WriteLine($"Data saved successfully to {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving data: " + ex.Message);
            }
        }

        public List<T> LoadData<T>(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"File not found: {filePath}");
                    return new List<T>();
                }

                string json = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    Console.WriteLine($"File is empty: {filePath}");
                    return new List<T>();
                }

                List<T>? data = JsonSerializer.Deserialize<List<T>>(
                    json, options);

                return data ?? new List<T>();
            }
            catch (JsonException)
            {
                Console.WriteLine(
                    $"Invalid JSON in {filePath}. Starting with an empty list.");

                return new List<T>();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
                return new List<T>();
            }
        }
    }
}