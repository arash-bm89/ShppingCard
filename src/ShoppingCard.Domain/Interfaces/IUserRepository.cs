using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using ShoppingCard.Domain.Dtos;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Domain.Interfaces;

public interface IUserRepository : IBaseRepository<User, UserFilter>
{
    Task CreateAsync(UserCreateDto userDto, CancellationToken cancellationToken);

    Task<bool> IsNamePasswordValid(UserLoginDto userLoginDto, CancellationToken cancellationToken);

    Task<User?> GetByNameAsync(string name);
}