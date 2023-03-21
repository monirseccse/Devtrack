using AutoMapper;
using DevTrack.Infrastructure.Codes;
using DevTrack.Infrastructure.UnitOfWorks;
using InvitationBO = DevTrack.Infrastructure.BusinessObjects.Invitation;
using InvitationEO = DevTrack.Infrastructure.Entities.Invitation;

namespace DevTrack.Infrastructure.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;

        public InvitationService(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task CreateInvitationsAsync(InvitationBO invitation)
        {
            var invitationEntity = _mapper.Map<InvitationEO>(invitation);

            await _applicationUnitOfWork.Invitations.AddAsync(invitationEntity);
            await _applicationUnitOfWork.SaveAsync();
        }

        public async Task<IList<InvitationBO>> GetInvitationsAsync(int invitationStatus)
        {
            var invitationEntities = await _applicationUnitOfWork.Invitations.GetAsync(x => ((int)x.Status).Equals(invitationStatus), "");

            var invitations = new List<InvitationBO>();

            if(invitationEntities.Count() > 0)
            {
                foreach(var invitationEO in invitationEntities)
                {
                    invitations.Add(_mapper.Map<InvitationBO>(invitationEO));
                }
            }
            return invitations;
        }

        public async Task UpdateInvitationStatusAsync(Guid invitationId, InvitationStatus status)
        {
            var invitationEO = await _applicationUnitOfWork.Invitations.GetByIdAsync(invitationId);
            if(invitationEO != null)
            {
                invitationEO.Status = status;
                await _applicationUnitOfWork.SaveAsync();
            }
            else
                throw new InvalidOperationException("Invitation was not found");
        }

        public async Task<InvitationBO> GetInvitationAsync(Guid invitationId)
        {
            var invitationEO = await _applicationUnitOfWork.Invitations.GetByIdAsync(invitationId);
            if (invitationEO != null)
            {
                return _mapper.Map<InvitationBO>(invitationEO);
            }
            else
                throw new InvalidOperationException("Invitation was not found");
        }
    }
}
