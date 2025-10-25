namespace Application.DTOs
{

    public class MessageDto
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }


    public class SendMessageRequest
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Text { get; set; }
    }
}