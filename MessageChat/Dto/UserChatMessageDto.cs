using MessageChat.DomainModels;
using System.Runtime.Serialization;

namespace MessageChat.Dto
{
    /// <summary>
    /// Данные сообщения
    /// </summary>
    [DataContract]
    public class UserChatMessageDto
    {
        public UserChatMessageDto() { }

        public UserChatMessageDto(MessageModel message)
        {
            UserName = message.UserName; 
            ChatId = message.ChatId;
            UserId = message.UserId;
            Text = message.Text;
        }
        /// <summary>
        /// Id пользователя
        /// </summary>
        [DataMember(Name = "userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [DataMember(Name = "userName")]
        public string UserName { get; set; }

        /// <summary>
        /// Id чата
        /// </summary>
        [DataMember(Name = "chatId")]
        public int ChatId { get; set; }

        /// <summary>
        /// Принадлежит ли сообщение текущему пользователю
        /// </summary>
        [DataMember(Name = "isMy")]
        public bool IsMy { get; set; }

        /// <summary>
        /// Содержимое сообщения
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}