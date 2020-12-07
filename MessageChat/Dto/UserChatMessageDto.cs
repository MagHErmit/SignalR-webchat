namespace MessageChat.Dto
{
    public class UserChatMessageDto
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public int ChatId { get; set; }

        public bool IsMy { get; set; }

        public string Text { get; set; }
    }
}