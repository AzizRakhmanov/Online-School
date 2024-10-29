using AutoMapper;
using DAL.IRepository;
using Domain.Models;
using Service.Dto;
using System.Linq.Expressions;

namespace Service.Services.UserService
{

    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        // private readonly ISchoolRepository<IdentityUser> _schoolIdentityRepository;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> repository,
            IMapper maperProfile)
        {
            this._repository = repository;
            this._mapper = maperProfile;
        }
        public async ValueTask<UserForResultDto> AddAsync(UserForCreationDto dto)
        {
            if (dto is null) throw new Exception();

            var dbUser = this._mapper.Map<User>(dto);

            await this._repository.InsertAsync(dbUser);

            return this._mapper.Map<UserForResultDto>(dto);
        }

        public async Task Delete(Guid id)
        {
            await this._repository.DeleteAsync(p => p.Id == id);
            // await this._repository.SaveAsync();
        }

        public IEnumerable<UserForResultDto> RetrieveAll(Expression<Func<User, bool>> expression)
        {
            var allDb = this._repository.SelectAll(expression, new string[] { "IdentityUser" });

            return this._mapper.Map<IEnumerable<UserForResultDto>>(allDb.OrderByDescending(p => p.BirthDate));
        }

        public async ValueTask<UserForResultDto> RetrieveAsync(Guid id)
        {
            var dbUser = await this._repository.SelectAsync(p => p.Id == id,new string[] { "IdentityUser"});

            var resultUser = this._mapper.Map<UserForResultDto>(dbUser);

            return resultUser;
        }

        public void Update(UserForCreationDto dto)
        {
            if (dto is null) throw new NullReferenceException();

            var dbUser = this._mapper.Map<User>(dto);

            this._repository.Update(dbUser);
            //await this._repository.SaveAsync();
        }

        //public async Task<bool> UserOwnPostAsync(Guid userId,Guid getPostId)
        //{
        //    var user = await this._repository.SelectAsync(userId);

        //    if (user == null)
        //        return false;

        //    if (user.Id != userId)
        //        return false;

        //    return true;
        //}
    }
}
