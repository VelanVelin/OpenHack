using System;
using System.Collections.Generic;
using System.Text;

namespace CreateRating.Model
{
    class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
