using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserRating
    {
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        public string LocationName { get; set; }

        [Range(0, 5, ErrorMessage = "Value cannot be larger than 5")]
        public int Rating { get; set; }

        public string UserNotes { get; set; }

        public DateTime Timestamp { get; set; }

    }
}
