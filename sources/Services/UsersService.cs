using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DAL.Abstract;
using Domain;
using Services.Dtos.Users;

namespace Services
{
    public interface IUsersService
    {
        AuthenticateUserDto Authenticate(string username, string password);
        void Create(CreateUserInputDto userInput, string password);
        void Delete(int id);
        GetAllUsersDto GetAll();
        GetByIdUserDto GetById(int id);
        void Update(UpdateUserInput updateInput, string password = null);
    }

    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public AuthenticateUserDto Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public GetAllUsersDto GetAll()
        {
            throw new NotImplementedException();
        }

        public GetByIdUserDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Create(CreateUserInputDto userInput, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new PasswordIsRequiredException();
            }

            if (_usersRepository.Get(e => e.Email == userInput.Email).Any())
            {
                throw new EmailIsAlreadyTakenException(userInput.Email);
            }

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            var user = new User
            {
                FirstName = userInput.FirstName,
                LastName = userInput.LastName,
                Email = userInput.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _usersRepository.Add(user);
        }

        public void Update(UpdateUserInput updateInput, string password = null)
        {
            var user = _usersRepository.GetById(updateInput.Id);

            user.FirstName = updateInput.FirstName;
            user.LastName = updateInput.LastName;
            user.Email = updateInput.Email;

            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _usersRepository.Update(user);
        }

        public void Delete(int id)
        {
            _usersRepository.Delete(id);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}