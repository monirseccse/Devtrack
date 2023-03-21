using AutoMapper;
using DevTrack.Infrastructure.UnitOfWorks;
using ApplicationUserBO = DevTrack.Infrastructure.BusinessObjects.ApplicationUser;

namespace DevTrack.Infrastructure.Services
{
    public class ApplicationUserService: IApplicationUserService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;

        public ApplicationUserService(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task<ApplicationUserBO> GetByUserIdAsync(Guid Id)
        {
            var userEO = await _applicationUnitOfWork
                .ApplicationUsers.GetByIdAsync(Id);

            var userBO = _mapper.Map<ApplicationUserBO>(userEO);
            
            return userBO;
        }

        public ApplicationUserBO GetByUserId(Guid Id)
        {
            var userEO = _applicationUnitOfWork.ApplicationUsers.GetById(Id);
            var userBO = _mapper.Map<ApplicationUserBO>(userEO);

            return userBO;
        }

        public async Task EditAccountAsync(ApplicationUserBO user)
        {
            var userEO = await _applicationUnitOfWork.ApplicationUsers.GetByIdAsync(user.Id);
            if (userEO != null)
            {
                userEO.Name = user.Name;
                userEO.Image = user.Image;
                await _applicationUnitOfWork.SaveAsync();
            }
            else
                throw new InvalidOperationException("User was not found");
        }
    }
}
