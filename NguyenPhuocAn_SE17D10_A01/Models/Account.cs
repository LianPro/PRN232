using System.ComponentModel.DataAnnotations;

namespace NguyenPhuocAn_SE17D10_A01.Models
{
    public class Account
    {
        [Key]
        public int AccountID { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        public int Role { get; set; } // 1: Staff, 2: Lecturer, Admin from appsettings

        public ICollection<NewsArticle> NewsArticles { get; set; }
    }
}
