using Carting.Carting.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Carting.Carting.Domain.Entities
{
    public class Cart
    {
        [Required]
        public string Id { get; set; }
        public List<Item> Items { get; set; }

        public Cart(string id)
        {
            Id = id;
            Items = new List<Item>();
        }
    }
}
