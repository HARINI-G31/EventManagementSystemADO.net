using System;
using EventManagement.Models;
using EventManagement.Repositories;

namespace EventManagement.Services
{
    public class AuditLogService
    {
        private readonly AuditLogRepository repository =
            new AuditLogRepository();

        public void LogAction(
            string action,
            string entityName,
            string recordId,
            string description)
        {
            AuditLog log = new AuditLog
            {
                Action = action,
                EntityName = entityName,
                RecordId = recordId,
                Description = description
            };

            repository.AddAuditLog(log);
        }
    }
}
