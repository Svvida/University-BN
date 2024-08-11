using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.ExternalEntities
{
    public class ExternalParticipantComanies
    {
        [Required]
        public Guid ExternalParticipantId { get; set; }
        public ExternalParticipant? ExternalParticipant { get; set; }
        [Required]
        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }

        public ExternalParticipantComanies() { }

        public ExternalParticipantComanies(Guid participantId, Guid companyId)
        {
            ExternalParticipantId = participantId;
            CompanyId = companyId;
        }
    }
}
