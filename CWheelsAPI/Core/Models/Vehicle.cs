using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [MaxLength(255)]
        public string Model { get; set; }
        [Required]
        [MaxLength(255)]
        public string Engine { get; set; }
        [Required]
        [MaxLength(100)]
        public string Color { get; set; }
        [Required]
        [MaxLength(255)]
        public string Company { get; set; }
        public DateTime DatePosted { get; set; }
        public bool IsHotAndNew { get; set; }
        public bool IsFeatured { get; set; }
        [Required]
        [MaxLength(255)]
        public string Location { get; set; }
        [Required]
        [MaxLength(255)]
        public string Condition { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Photo> Images { get; set; }

        public ICollection<Thumbnail> Thumbnails { get; set; }

        public Vehicle()
        {
            Images = new Collection<Photo>();
            Thumbnails = new Collection<Thumbnail>();
        }
    }
}
