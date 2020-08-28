using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Uplift.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [DisplayName("Service Name")]
        public string Name { get; set; }
        
        [Required]
        [DisplayName("Price")]
        public double Price { get; set; }
        
        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }
        
        [Required]
        [DataType(DataType.ImageUrl)]
        [DisplayName("Image URL")]
        public string ImageUrl { get; set; }

        [Required]
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }

        [Required]
        public int FrequencyID { get; set; }
        [ForeignKey("FrequencyID")]
        public Frequency Frequency { get; set; }
    }
}
