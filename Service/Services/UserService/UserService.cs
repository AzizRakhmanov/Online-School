using AutoMapper;
using DAL.IRepository;
using Domain.Models;
using Service.Dto;
using System.Linq.Expressions;

namespace Service.Services.UserService
{

    public class UserService : IUserService
    {
        private readonly ISchoolRepository<User> _repository;
        private readonly IMapper _mapper;

        public UserService(ISchoolRepository<User> repository,
            IMapper maperProfile)
        {
            this._repository = repository;
            this._mapper = maperProfile;
        }
        public async ValueTask<UserForResultDto> AddAsync(UserForCreationDto dto)
        {
            if (dto is null) throw new Exception();

            var dbUser = this._mapper.Map<User>(dto);

            await this._repository.CreateAsync(dbUser);
            await this._repository.SaveAsync();

            return this._mapper.Map<UserForResultDto>(dto);
        }

        public async Task Delete(Guid id)
        {
            await this._repository.Delete(id);
            // await this._repository.SaveAsync();
        }

        public async Task<IEnumerable<UserForResultDto>> RetrieveAllAsync(Expression<Func<User, bool>> expression)
        {
            var allDb = await this._repository.SelectAllAsync(expression);

            var usersResultDto = this._mapper.Map<IEnumerable<UserForResultDto>>(allDb.OrderByDescending(p => p.BirthDate));

            return usersResultDto;
        }

        public async ValueTask<UserForResultDto> RetrieveAsync(Guid id)
        {
            var dbUser = await this._repository.SelectAsync(id);

            var resultUser = this._mapper.Map<UserForResultDto>(dbUser);

            return resultUser;
        }

        public async Task Update(UserForCreationDto dto)
        {
            if (dto is null) throw new NullReferenceException();

            var dbUser = this._mapper.Map<User>(dto);

            await this._repository.Update(dbUser);
            //await this._repository.SaveAsync();
        }

        public async Task<bool> UserOwnPostAsync(Guid userId, Guid getPostId)
        {
            var user = await this._repository.SelectAsync(userId);

            if(user == null)
                return false;

            if (user.Id != userId)
                return false;

            return true;
        }
    }
}
