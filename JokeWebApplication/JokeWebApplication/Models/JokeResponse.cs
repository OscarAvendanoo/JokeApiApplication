namespace JokeWebApplication.Models
{
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
