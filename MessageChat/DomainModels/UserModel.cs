using MessageChat.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageChat.DomainModels
{
    public class UserModel
    {
        public UserModel() { }
        public UserModel(string id, RegistrationDto reg)
        {
            Id = id;
            Name = reg.Name;
            Email = reg.Email;
            Password = reg.Password;
        }
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
