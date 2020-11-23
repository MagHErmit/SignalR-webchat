﻿using System.Runtime.Serialization;

namespace MessageChat.Dto
{
    /// <summary>
    /// Данные для авторизации
    /// </summary>
    [DataContract]
    public class LoginInDto
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Пароль пользователя в MD5
        /// </summary>
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}