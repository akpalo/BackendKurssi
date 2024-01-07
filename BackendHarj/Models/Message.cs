using System.ComponentModel.DataAnnotations;

namespace BackendHarj.Models
{
    public class Message
    {
        public long Id { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }
        public string Body { get; set; }

        [Required]
        public User Sender { get; set; }
        
        public User? Recipient { get; set; }

        public Message? PrevMessage { get; set; }
    }

    public class MessageDTO
    {
        public long Id { get; set; }
        [MaxLength(255)]
        public string Title { get; set; }
        public string Body { get; set; }
        [Required]

        public string Sender { get; set; }

        public string? Recipient { get; set; }

        public long? PrevMessageID { get; set; }
    }
}
