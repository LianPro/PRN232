using System.ComponentModel.DataAnnotations;

namespace NguyenPhuocAn_SE17D10_A01.Models
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<NewsArticleTag> NewsArticleTags { get; set; }
    }
}
