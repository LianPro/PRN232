using System.ComponentModel.DataAnnotations;

namespace NguyenPhuocAn_SE17D10_A01.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public bool Status { get; set; } // true: Active, false: Inactive

        public ICollection<NewsArticle> NewsArticles { get; set; }
    }
}
