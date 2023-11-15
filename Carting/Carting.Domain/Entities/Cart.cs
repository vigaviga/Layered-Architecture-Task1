using Carting.Carting.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Carting.Carting.Domain.Entities
{
    public class Cart
    {
        [Required]
        public int Id { get; set; }
        public List<Item> Items { get; set; }
    }
}
