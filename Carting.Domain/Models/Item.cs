using System.ComponentModel.DataAnnotations;

namespace Carting.Carting.Domain.Models
{
    public class Item
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public Image? Image { get; set; }

        [Required]
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
