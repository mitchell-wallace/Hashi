using System;
using System.Collections.Generic;

namespace HashiAPI_1.Models
{
    public partial class User
    {
        public int UID { get; set; } // user mapping ID
        public string Email { get; set; } = null!;
        public string? DisplayName { get; set; }
        public string JiraId { get; set; } = null!;
        public string WrikeId { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        override public string ToString()
        {
            return $"{DisplayName} | Jira ID: {JiraId} | Wrike ID: WrikeId";
        }
    }
}
