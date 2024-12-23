﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10091324_PROG7312_Part1.Model
{
    public class User
    {
        // The blueprint of the class was autogenerated by Visual Studio
        // and modified to fit our needs
        // Declaring fields for the user profile 

        [Key]
        public Guid Id { get; set; }  // Primary Key

        [MaxLength(50)]
        public string Username { get; private set; }

        [Required]
        [EmailAddress]
        public string Email { get; private set;  }

        [Required]
        [MinLength(8)]
        public string Password { get; private set;  }

        public DateTime DateCreated { get; private set;  }

        public string ProfileImgPath { get; set; }

        public string Role { get; private set; }

        // Navigation properties to related entities
        public ICollection<ServiceRequest> ServiceRequests { get; set; }
        public ICollection<Incident> Incidents { get; set; }

        // Parameterless constructor for Entity Framework
        public User() { }

        public User(string username, string email, string password, string role)
        {
            // Assigning data passed to the fields belonging to the user class
            Id = Guid.NewGuid();  // Generate a new GUID for the Id
            Username = username;
            Email = email;
            Password = password;
            DateCreated = DateTime.Now;
            Role = role;
            ServiceRequests = new List<ServiceRequest>();
            Incidents = new List<Incident>();
        }

        public User(string username, string email, string password, string profileImgPath, string role)
        {
            // Assigning data passed to the fields belonging to the user class
            Id = Guid.NewGuid();  // Generate a new GUID for the Id
            Username = username;
            Email = email;
            Password = password;
            DateCreated = DateTime.Now;
            ProfileImgPath = profileImgPath;
            Role = role;
            ServiceRequests = new List<ServiceRequest>();
            Incidents = new List<Incident>();
        }
    }
}
