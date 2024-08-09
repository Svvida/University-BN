using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ExternalEntities
{
    public class ExternalParticipantComanies
    {
        [Required]
        public Guid ExternalParticipantId { get; set; }
        public ExternalParticipant? ExternalParticipant { get; set; }
        [Required]
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }

        public ExternalParticipantComanies() { }

        public ExternalParticipantComanies(Guid participantId, Guid companyId)
        {
            ExternalParticipantId = participantId;
            CompanyId = companyId;
        }
    }
}
