using System;
using System.Collections.Generic;

namespace HashiAPI_1.Models
{
    public partial class Project
    {
        public int PID { get; set; } // project mapping ID
        public string ProjectName { get; set; } = null!;
        public string JiraId { get; set; } = null!;
        public string WrikeId { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        override public string ToString()
        {
            return $"{ProjectName} | Jira ID: {JiraId} | Wrike ID: WrikeId";
        }
    }
}
