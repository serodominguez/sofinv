using BackEnd.Application.Dtos.UsersDtos;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;

namespace BackEnd.Application.Services
{
    public class UserService
    {
        private readonly IUsersRepository _userRepository;

        public UserService(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserReadDto>> ReadUsers()
        {
            var users = await _userRepository.ReadUsers();
            return users.Select(u => new UserReadDto
            {
                PK_USER = u.PK_USER,
                USER_NAME = u.USER_NAME,
                NAMES = u.NAMES,
                LAST_NAMES = u.LAST_NAMES,
                IDENTIFICATION = u.IDENTIFICATION,
                PHONE = u.PHONE?.ToString(),
                STATUS = u.STATUS,
                PK_ROLE = u.PK_ROLE,
                PK_STORE = u.PK_STORE,
                CREATION_DATE = u.CREATION_DATE.HasValue ? u.CREATION_DATE.Value.ToString("dd/MM/yyyy HH:mm") : null,
                ROLE_NAME = u.ROLE_NAME,
                STORE_NAME = u.STORE_NAME
            });
        }

        public async Task<IEnumerable<UserReadDto>> SearchUser(string text)
        {
            var users = await _userRepository.SearchUser(text);
            return users.Select(u => new UserReadDto
            {
                PK_USER = u.PK_USER,
                USER_NAME = u.USER_NAME,
                NAMES = u.NAMES,
                LAST_NAMES = u.LAST_NAMES,
                IDENTIFICATION = u.IDENTIFICATION,
                PHONE = u.PHONE?.ToString(),
                STATUS = u.STATUS,
                PK_ROLE = u.PK_ROLE,
                PK_STORE = u.PK_STORE,
                CREATION_DATE = u.CREATION_DATE.HasValue ? u.CREATION_DATE.Value.ToString("dd/MM/yyyy HH:mm") : null,
                ROLE_NAME = u.ROLE_NAME,
                STORE_NAME = u.STORE_NAME
            });
        }

        public async Task<UserCredentialsDto?> CredentialsUser(string userName)
        {
            var user = await _userRepository.CredentialsUser(userName);
            if (user == null) 
                return null;

            return new UserCredentialsDto
            {
                PK_USER = user.PK_USER,
                USER_NAME = user.USER_NAME,
                PASSWORD_HASH = user.PASSWORD_HASH,
                PASSWORD_SALT = user.PASSWORD_SALT,
                PK_ROLE = user.PK_ROLE,
                PK_STORE = user.PK_STORE,
                PK_WAREHOUSE = user.stores?.warehouses?.PK_WAREHOUSE ?? 0,
                ROLE_NAME = user.ROLE_NAME,
                STORE_NAME = user.STORE_NAME
            };
        }

        public async Task<string> CreateUser(UserCreateDto createDto)
        {
            if (string.IsNullOrEmpty(createDto.PASSWORD))
            {
                return "La contraseña no puede estar vacía.";
            }

            var user = new Users
            {
                USER_NAME = createDto.USER_NAME,
                NAMES = createDto.NAMES,
                LAST_NAMES = createDto.LAST_NAMES,
                IDENTIFICATION = string.IsNullOrWhiteSpace(createDto.IDENTIFICATION) ? null : createDto.IDENTIFICATION,
                PHONE = string.IsNullOrWhiteSpace(createDto.PHONE) ? (int?)null : int.Parse(createDto.PHONE),
                PK_ROLE = createDto.PK_ROLE,
                PK_STORE = createDto.PK_STORE
            };

            return await _userRepository.CreateUser(user, createDto.PASSWORD);
        }

        public async Task<string> UpdateUser(UserUpdateDto updateDto)
        {
            var user = new Users
            {
                PK_USER = updateDto.PK_USER,
                USER_NAME = updateDto.USER_NAME,
                NAMES = updateDto.NAMES,
                LAST_NAMES = updateDto.LAST_NAMES,
                IDENTIFICATION = string.IsNullOrWhiteSpace(updateDto.IDENTIFICATION) ? null : updateDto.IDENTIFICATION,
                PHONE = string.IsNullOrWhiteSpace(updateDto.PHONE) ? (int?)null : int.Parse(updateDto.PHONE),
                PK_ROLE = updateDto.PK_ROLE,
                PK_STORE = updateDto.PK_STORE
            };

            return await _userRepository.UpdateUser(user);
        }

        public async Task<string> UpdatePassword(UserUpdateDto updateDto)
        {
            if (string.IsNullOrEmpty(updateDto.PASSWORD))
            {
                return "La contraseña no puede estar vacía.";
            }

            var user = new Users
            {
                PK_USER = updateDto.PK_USER,
                USER_NAME = updateDto.USER_NAME,
                NAMES = updateDto.NAMES,
                LAST_NAMES = updateDto.LAST_NAMES,
                IDENTIFICATION = string.IsNullOrWhiteSpace(updateDto.IDENTIFICATION) ? null : updateDto.IDENTIFICATION,
                PHONE = string.IsNullOrWhiteSpace(updateDto.PHONE) ? (int?)null : int.Parse(updateDto.PHONE),
                PK_ROLE = updateDto.PK_ROLE,
                PK_STORE = updateDto.PK_STORE
            };

            return await _userRepository.UpdatePassword(user, updateDto.PASSWORD);
        }

        public async Task EnabledUser(int id)
        {
            await _userRepository.EnabledUser(id);
        }

        public async Task DisabledUser(int id)
        {
            await _userRepository.DisabledUser(id);
        }
    }
}
