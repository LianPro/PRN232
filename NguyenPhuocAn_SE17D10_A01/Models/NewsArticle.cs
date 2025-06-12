using System.ComponentModel.DataAnnotations;

namespace NguyenPhuocAn_SE17D10_A01.Models
{
    public class NewsArticle
    {
        [Key]
        public int ArticleID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public bool Status { get; set; } // true: Active, false: Inactive

        public int CategoryID { get; set; }
        public Category Category { get; set; }

        public int AccountID { get; set; }
        public Account Account { get; set; }

        public ICollection<NewsArticleTag> NewsArticleTags { get; set; }
    }
}
