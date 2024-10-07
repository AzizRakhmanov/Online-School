using Domain.Models;
using Service.Dto;
using System.Linq.Expressions;

namespace Service.Services.UserService
{
    public interface IUserService
    {
        public ValueTask<UserForResultDto> AddAsync(UserForCreationDto dto);

        public ValueTask<UserForResultDto> RetrieveAsync(Guid id);

        public Task<IEnumerable<UserForResultDto>> RetrieveAllAsync(Expression<Func<User, bool>> expression);

        public Task Update(UserForCreationDto dto);

        //  public Task<bool> UserOwnPostAsync(Guid userId, Guid getPostId);

        public Task Delete(Guid id);
    }
}
