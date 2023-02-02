using System.ComponentModel.DataAnnotations;

namespace signalrtest.Models
{
    public class ChatRoom
    {
        [Required]
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
