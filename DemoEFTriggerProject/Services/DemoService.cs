using DemoEFTriggerProject.DbClasses;
using DemoEFTriggerProject.DBContext;
using DemoEFTriggerProject.Models;
using DemoEFTriggerProject.PasswordExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DemoEFTriggerProject.Services
{
    public class DemoService
    {
        private readonly DataContext _dataContext;
        private readonly TokenOptionModel _tokenOptions;

        public DemoService(DataContext dataContext,
                            IOptions<TokenOptionModel> options)
        {
            _dataContext = dataContext;
            _tokenOptions = options.Value;
        }

        public async Task CreateUserAsync(UserDto model)
        {
            model.Password.CreatePasswordHash(out string hashedPassword);
            var theUser = new Users()
            {
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                Password = hashedPassword
            };

            await _dataContext.AddAsync(theUser);

            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return  await _dataContext.Users.ToListAsync();
        }

        public async Task<string> GetJwtAsync(LoginModel model)
        {
            var theUser = await _dataContext.Users.Where(x => x.Email == model.Email)
                                                  .SingleOrDefaultAsync();

            if (theUser is null || model.Password.VerifyPasswordHash(theUser.Password) is false)
                throw new Exception("Kullanıcı email veya parola hatalı");

            var accessTokenExpiration = DateTime.Now.AddHours(_tokenOptions.AccessTokenExpiration);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));

            JwtSecurityToken jwtSecurityToken = new (
                    issuer: _tokenOptions.Issuer,
                    expires: accessTokenExpiration,
                    notBefore: DateTime.Now,
                    claims: GetUserClaims(theUser),
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
                 );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private IEnumerable<Claim> GetUserClaims(Users user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name,$"{user.Name} {user.Surname}"),
                new Claim(JwtRegisteredClaimNames.Aud, _tokenOptions.Audience)
            };
        }

        public async Task CreateDemoRecordAsync(DemoClassDto model)
        {
            var dbDemo = new DbDemoClass()
            {
                StringField = model.StringField,
                IntField = model.IntField
            };

            await _dataContext.AddAsync(dbDemo);

            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateDemoRecordAsync(DemoClassDto model)
        {
            var dbDemo = await _dataContext.FindAsync<DbDemoClass>(model.Id);

            dbDemo.StringField = model.StringField;
            dbDemo.IntField = model.IntField;

            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteDemoRecordAsync(int id)
        {
            _dataContext.Remove(await _dataContext.FindAsync<DbDemoClass>(id));

            await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<DbDemoClass>> GetAllDemoRecordsAsync()
        {
            return await _dataContext.DbDemoClasses.ToListAsync();
        }
    }
}
