using System;

namespace EventManagement.Models
{
    public class AuditLog
    {
        public int AuditId { get; set; }
        public string Action { get; set; } = "";
        public string EntityName { get; set; } = "";
        public string RecordId { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime ActionDate { get; set; }
    }
}
