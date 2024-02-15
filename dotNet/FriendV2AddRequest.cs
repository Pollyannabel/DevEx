using Sabio.Models.Domain.Friends;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Friends
{
    public class FriendV2AddRequest
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
        [RangeAttribute(1, 5000)]
        public int StatusId { get; set; }

        [Required]
        [MinLength(2)]
        public string PrimaryImageUrl { get; set; } //wiki has this as string and not Image type

        public string ImageTypeId { get; set; }





    }
}
