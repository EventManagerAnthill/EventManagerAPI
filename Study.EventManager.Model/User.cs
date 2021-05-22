using Study.EventManager.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
[assembly: InternalsVisibleToAttribute("Study.EventManager.Services")]

namespace Study.EventManager.Model
{
    
    public class User
    {
        internal User()
        { }

        public User(string userName, string passWord, string firstName, string lastName, string email) 
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(passWord))
                throw new ArgumentNullException("userName");

            Username = userName;
            Password = passWord;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsVerified = false;
        }

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Middlename { get; set; }

        public DateTime BirthDate { get; set; }

        [Required]
        public string Email { get; set; }

        public string Phone { get; set; }

        public UserSex Sex { get; set; }

        public string Username { get; set; }

        [Required]
        public string Password { get; set; }       

        public bool IsVerified { get; set; }
    }
}
