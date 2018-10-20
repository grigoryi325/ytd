using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YouTubeDataBase.Models.AccountModels
{   
    public class User
    {
        private readonly string RoleUser = "user";

        public int Id { get; set; }

        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role
        {
            get
            {
                return RoleUser;
            }
            set
            {
                value = RoleUser;
            }
        }
    }
}