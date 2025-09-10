using Application.Dto.User;
using Domain.Contracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Logic.UserService
{
    public interface IUserService
    {
        Task<UserJoinDto?> GetUserWithPermissionsAsync(string username);
    }
}
