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
        AuthenticateUserDto Authenticate(string email, string password);
        void Create(CreateUserInputDto userInput, string password);
        void Delete(int id);
        GetAllUsersDto GetAll();
        GetByIdUserDto GetById(long id);
        void Update(UpdateUserInput updateInput, string password = null);
    }

    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public AuthenticateUserDto Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            User user = _usersRepository.Get(e => e.Email == email).SingleOrDefault();
            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return new AuthenticateUserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public GetAllUsersDto GetAll()
        {
            throw new NotImplementedException();
        }

        public GetByIdUserDto GetById(long id)
        {
            User user = _usersRepository.GetById(id);

            return new GetByIdUserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
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
            User user = _usersRepository.GetById(updateInput.Id);

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

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }

            if (storedHash.Length != 64)
            {
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            }

            if (storedSalt.Length != 128)
            {
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
            }

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}