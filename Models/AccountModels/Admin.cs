using System.ComponentModel.DataAnnotations;

namespace YouTubeDataBase.Models.AccountModels
{
    public class Admin
    {
        public int Id { get; set; }

        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role => "admin";
    }
}