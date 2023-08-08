using System.ComponentModel.DataAnnotations;

namespace ChatAPI.Models
{
    public class ChatMessage
    {
        [Required]
        public string User { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
