using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Friends
{
    public class FriendAddRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Bio { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Summary { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Headline { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Slug { get; set; }

        [Required]
        [RangeAttribute(1,5000)]
        public int StatusId { get; set; }

        [Required]
        public string PrimaryImageUrl { get; set; }

    }
}


