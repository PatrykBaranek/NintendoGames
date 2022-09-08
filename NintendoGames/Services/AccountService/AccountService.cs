using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NintendoGames.Entities;
using NintendoGames.Exceptions;
using NintendoGames.Models.AccountModels;

namespace NintendoGames.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly NintendoDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(NintendoDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public async Task Register(CreateUserDto createUserDto)
        {

            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                Email = createUserDto.Email,
                RoleId = createUserDto.RoleId
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, createUserDto.Password);

            newUser.PasswordHash = hashedPassword;



            var createWishList = new WishList
            {
                Id = Guid.NewGuid(),
                UserId = newUser.Id
            };

            newUser.WishListId = createWishList.Id;

            await _dbContext.User.AddAsync(newUser);
            await _dbContext.WishList.AddAsync(createWishList);
            await _dbContext.SaveChangesAsync();

        }

        public async Task<string> GenerateJwt(LoginDto loginDto)
        {
            var user = await _dbContext.User
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user is null)
                throw new BadRequestException("Invalid email or password");


            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new BadRequestException("Invalid email or password");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(60);

            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}
