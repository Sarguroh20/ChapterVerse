using System.ComponentModel.DataAnnotations;

namespace ChapterVerseUI.Models.DTOs
{
    public class GenreDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(45)]
        public string  GenreName { get; set; }
    }
}
