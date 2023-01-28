using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.Implementations;
using Common.Utilities;
using Microsoft.EntityFrameworkCore;
using ShoppingCard.Domain.Dtos;
using ShoppingCard.Domain.Filters;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;

namespace ShoppingCard.Repository.Implementations;

public class UserRepository : BaseRepository<User, UserFilter>, IUserRepository
{
    private readonly IMapper _mapper;

    public UserRepository(ApplicationDbContext db, IMapper mapper) : base(db)
    {
        _mapper = mapper;
    }

    protected override IQueryable<User> ConfigureInclude(IQueryable<User> query)
    {
        return query.Include(x => x.Orders).ThenInclude(x => x.Payments);
    }

    protected override IQueryable<User> ConfigureListInclude(IQueryable<User> query)
    {
        return query;
    }

    protected override IQueryable<User> ApplyFilter(IQueryable<User> query, UserFilter filter)
    {
        return query;
    }

    public Task CreateAsync(UserCreateDto userDto, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<UserCreateDto, User>(userDto);

        user.HashedPassword = SecurityHelper.ComputeSHA256(userDto.Password);
        return base.CreateAsync(user, cancellationToken);
    }

    public async Task<bool> IsNamePasswordValid(UserLoginDto userLoginDto, CancellationToken cancellationToken)
    {
        var passwordHash = SecurityHelper.ComputeSHA256(userLoginDto.Password);
        return await HasAnyAsync(x => x.Name == userLoginDto.Name && x.HashedPassword == passwordHash
            , cancellationToken);
    }

    public async Task<User?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
    }
}