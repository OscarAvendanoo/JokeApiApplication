using System;
using System.Text.Json;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main()
        {
            // skapar klienten som används för att skicka http förfrågan

            using HttpClient client = new HttpClient();

            // En lista av olika URLer för olika skämt, det är från dessa URLer vi får våra skämt.
            // Dessa är alltså URL till apiet vi anropar

            string url1 = "https://v2.jokeapi.dev/joke/Programming"; // Example for programming jokes
            string url2 = "https://v2.jokeapi.dev/joke/Misc"; // Example for programming jokes
            string url3 = "https://v2.jokeapi.dev/joke/Pun"; // Example for programming jokes
            string url4 = "https://v2.jokeapi.dev/joke/Spooky"; // Example for programming jokes
            string url5 = "https://v2.jokeapi.dev/joke/Christmas"; // Example for programming jokes

            // Här anropar vi en metod som hämtar, dezerialiserar, och skriver ut skämtet
            // Parametrarna vi skickar med är de olika URLerna för att generera olika skämt
            // samt clienten som vi tidigare skapat och även vilket typ av skämt.
            await GenerateJoke(url1, client,"Programming");
            await GenerateJoke(url2, client,"Misc");
            await GenerateJoke(url3, client,"Pun");
            await GenerateJoke(url4, client,"Spooky");
            await GenerateJoke(url5, client,"Christmas");


        }
        static async Task GenerateJoke(string URL,HttpClient client, string typeOfJoke)
        {
            try
            {
                // skapar en variabel response som blir själva svaret från APIet
                // response innehåller flera delar, en av dessa är en return code som säger
                // om anropet gick bra eller inte, en annan del av respons är content,
                // som innehåller själva skämtet.

                HttpResponseMessage response = await client.GetAsync(URL);
                response.EnsureSuccessStatusCode(); // Throws if not 2xx

                // här tar vi content delen från respons och gör om till en sträng med JSON data
                string json = await response.Content.ReadAsStringAsync();

                // Här dezerialiserar vi json strängen och mappar den till "JokeRespone" modellen
                // som ni hittar längst ner i den här filen

                var jokeData = JsonSerializer.Deserialize<JokeResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Här nedan skriver vi ut själva skämtet med den logik som 
                // APIet tillhandagav via dess Info-sida

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
        // Här är den model som vi mappar till från JSON filen
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
