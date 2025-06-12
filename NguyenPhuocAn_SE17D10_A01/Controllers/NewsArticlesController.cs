using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using NguyenPhuocAn_SE17D10_A01.DataAccess;
using NguyenPhuocAn_SE17D10_A01.Models;
using NguyenPhuocAn_SE17D10_A01.Services;

namespace NguyenPhuocAn_SE17D10_A01.Controllers
{
    [Route("odata/NewsArticles")]
    public class NewsArticlesController : ODataController
    {
        private readonly NewsArticleService _newsArticleService;
        private readonly NewsManagementContext _context;

        public NewsArticlesController(NewsArticleService newsArticleService, NewsManagementContext context)
        {
            _newsArticleService = newsArticleService ?? throw new ArgumentNullException(nameof(newsArticleService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var articles = await _newsArticleService.GetAllAsync();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var article = await _newsArticleService.GetByIdAsync(id);
                return Ok(article);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewsArticleDTO articleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var article = new NewsArticle
                {
                    Title = articleDto.Title,
                    Content = articleDto.Content,
                    CreatedDate = DateTime.UtcNow,
                    Status = articleDto.Status,
                    CategoryID = articleDto.CategoryID,
                    AccountID = articleDto.AccountID,
                    NewsArticleTags = articleDto.TagIds?.Select(tagId => new NewsArticleTag { TagID = tagId }).ToList()
                };

                await _newsArticleService.AddAsync(article);
                return Created(article);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NewsArticleDTO articleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != articleDto.ArticleID)
                return BadRequest("Article ID mismatch.");

            try
            {
                var article = await _newsArticleService.GetByIdAsync(id);
                if (article == null) return NotFound();

                article.Title = articleDto.Title;
                article.Content = articleDto.Content;
                article.Status = articleDto.Status;
                article.CategoryID = articleDto.CategoryID;
                article.AccountID = articleDto.AccountID;

                // Update tags
                var existingTags = await _context.NewsArticleTags.Where(nat => nat.ArticleID == id).ToListAsync();
                _context.NewsArticleTags.RemoveRange(existingTags);

                if (articleDto.TagIds != null)
                {
                    article.NewsArticleTags = articleDto.TagIds.Select(tagId => new NewsArticleTag { ArticleID = id, TagID = tagId }).ToList();
                    await _context.SaveChangesAsync();
                }

                await _newsArticleService.UpdateAsync(article);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _newsArticleService.DeleteAsync(id);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Invalid news article ID.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }

    public class NewsArticleDTO
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
        public int CategoryID { get; set; }
        public int AccountID { get; set; }
        public List<int>? TagIds { get; set; }
    }
}
