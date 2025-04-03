using System;
using System.Text.Json;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main()
        {
            using HttpClient client = new HttpClient();
            string url1 = "https://v2.jokeapi.dev/joke/Programming"; // Example for programming jokes
            string url2 = "https://v2.jokeapi.dev/joke/Dark"; // Example for programming jokes
            string url3 = "https://v2.jokeapi.dev/joke/Pun"; // Example for programming jokes
            string url4 = "https://v2.jokeapi.dev/joke/Spooky"; // Example for programming jokes
            string url5 = "https://v2.jokeapi.dev/joke/Christmas"; // Example for programming jokes

            await GenerateJoke(url1, client,"Programming");
            await GenerateJoke(url2, client,"Dark");
            await GenerateJoke(url3, client,"Pun");
            await GenerateJoke(url4, client,"Spooky");
            await GenerateJoke(url5, client,"Christmas");


        }
        static async Task GenerateJoke(string URL,HttpClient client, string typeOfJoke)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(URL);
                response.EnsureSuccessStatusCode(); // Throws if not 2xx

                string json = await response.Content.ReadAsStringAsync();

                // Deserialize JSON response
                var jokeData = JsonSerializer.Deserialize<JokeResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Check if joke data was retrieved
                if (jokeData != null)
                {
                    Console.WriteLine("Joketype:" + " " + typeOfJoke);

                    if (jokeData.Type == "single")
                    {
                        //Console.WriteLine("Raw JSON Response: " + json); // Debugging step
                        //Console.Read();
                        Console.WriteLine($"Joke: {jokeData.joke}"); // Only setup if single
                    }
                    else if (jokeData.Type == "twopart")
                    {
                        Console.WriteLine($"Setup: {jokeData.Setup}");
                        Console.WriteLine($"Punchline: {jokeData.Delivery}");
                    }
                    Console.WriteLine();

                }
                else
                {
                    Console.WriteLine("No joke found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching joke: {ex.Message}");
            }

        }
        public class JokeResponse
        {
            public bool Error { get; set; } // to check if there's an error
            public string Category { get; set; } // Joke category
            public string Type { get; set; } // Type of joke (single or twopart)
            public string joke { get; set; } 
            public string Setup { get; set; } // The setup of a two-part joke
            public string Delivery { get; set; } // The punchline of a two-part joke
            public bool Safe { get; set; } // Safe joke flag
            public string Lang { get; set; } // Language of the joke
        }
    }

}
