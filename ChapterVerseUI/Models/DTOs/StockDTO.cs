using System.ComponentModel.DataAnnotations;

namespace ChapterVerseUI.Models.DTOs
{
    public class StockDTO
    {
        public int BookId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative vale")]
        public int Quantity { get; set; }
    }
}
