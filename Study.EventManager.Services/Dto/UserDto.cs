using Study.EventManager.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Study.EventManager.Services.Dto
{
    public class UserDto
    { 
        public int Id { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Middlename { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public UserSex Sex { get; set; }

        [JsonIgnore]
        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}
