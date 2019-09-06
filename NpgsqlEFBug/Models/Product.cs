using System;
using System.ComponentModel.DataAnnotations;

namespace NpgsqlEFBug
{
    public class Product
    {
        public Product()
        {
        }

        [Key]
        public long Id { get; set; }

        public string Title { get; set; }

        public DateTime CreateDateUTC { get; set; } = DateTime.UtcNow;
    }
}