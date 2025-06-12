namespace NguyenPhuocAn_SE17D10_A01.Models
{
    public class NewsArticleTag
    {
        public int ArticleID { get; set; }
        public NewsArticle NewsArticle { get; set; }

        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }
}
