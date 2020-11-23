using System.Runtime.Serialization;

namespace MessageChat.Dto
{
    /// <summary>
    /// Данные для регистрации
    /// </summary>
    [DataContract]
    public class RegistrationDto
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Email пользователя в MD5
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя в MD5
        /// </summary>
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}
