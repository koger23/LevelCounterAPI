using System.Collections.Generic;

namespace LevelCounter.Models.DTO
{
    public class LoginResponse
    {
        public List<string> ErrorMessages {get; set; } = new List<string>();
        public string Token {get; set; }
    }
}
