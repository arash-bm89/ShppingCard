using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingCard.Api.Models;
using ShoppingCard.Domain.Dtos;
using ShoppingCard.Domain.Interfaces;
using ShoppingCard.Domain.Models;
using ShoppingCard.Service.IServices;

namespace ShoppingCard.Api.Controllers;

[Route("user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly string _jwtKey;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;

    public UserController(
        IUserRepository userRepository,
        IMapper mapper,
        IConfiguration configuration,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _jwtService = jwtService;
        _jwtKey = configuration.GetSection("keys").GetValue<string>("jwtKey");
    }


    /// <summary>
    /// create user
    /// </summary>
    /// <param name="userRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> Create([FromBody] UserCreateRequest userRequest)
    {
        var nameExist = await _userRepository
            .HasAnyAsync(x => x.Name == userRequest.Name, HttpContext.RequestAborted);

        if (nameExist) return BadRequest($"Name {userRequest.Name} is already in use");

        var emailExist = await _userRepository
            .HasAnyAsync(x => x.Email == userRequest.Email, HttpContext.RequestAborted);

        if (emailExist) return BadRequest($"Email {userRequest.Email} is already in use");

        // creating user using userCreateDto
        var userDto = _mapper.Map<UserCreateRequest, UserCreateDto>(userRequest);
        userDto.Id = Guid.NewGuid();
        await _userRepository.CreateAsync(userDto, HttpContext.RequestAborted);


        var userResponse = _mapper.Map<UserCreateDto, UserResponse>(userDto);

        return Ok(userResponse);
    }


    /// <summary>
    /// get user by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> Get([FromRoute] Guid id)
    {
        var user = await _userRepository.GetAsync(id, HttpContext.RequestAborted);
        if (user == null) return NotFound($"user with id: {id} not found.");

        var userResponse = _mapper.Map<User, UserResponse>(user);
        return Ok(userResponse);
    }


    /// <summary>
    /// login with name and password
    /// </summary>
    /// <param name="userLoginRequest"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] UserLoginRequest userLoginRequest)
    {
        // can be some situation that we need more login options to add in userLoginRequest
        // so userLoginDto using just for namePassword logic
        var userLoginDto = _mapper.Map<UserLoginRequest, UserLoginDto>(userLoginRequest);
        var isNamePasswordValid =
            await _userRepository.IsNamePasswordValid(userLoginDto, HttpContext.RequestAborted);

        if (!isNamePasswordValid)
            return NotFound("Name or Password is wrong.");

        var user = await _userRepository.GetByNameAsync(userLoginDto.Name);

        var loginResponse = new LoginResponse()
        {
            Token = _jwtService.CreateJwt(user.Id, _jwtKey)
        };

        return Ok(loginResponse);
    }
}