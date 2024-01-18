using StoredProcuduresTest.Data;
using AutoMapper;
using StoredProcuduresTest.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StoredProcuduresTest.Dtos;
using Microsoft.Extensions.Configuration;

namespace StoredProcuduresTest.Data
{
    public class UserRepository : IUserRepository
    {
        DataContextEF _db;
        Mapper _mapper;
        public UserRepository(IConfiguration configuration)
        {
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDto, User>();

            }));
            _db = new DataContextEF(configuration);
        }


        public bool SaveChanges()
        {
            return _db.SaveChanges() > 0;
        }

        public async Task<bool> UpdateEntity(User user)
        {
            User? userDb = await _db.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
            if (userDb != null)
            {
                userDb = _mapper.Map<User>(user);
                SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteEntity(int id)
        {
            User? userDb = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (userDb != null)
            {
                _db.Users.Remove(userDb);
                await _db.SaveChangesAsync();
                return true;
            }
            else
                throw new Exception("Expected non-null value");
        }

        public async Task AddEntity(UserDto user)
        {
            if (user != null)
            {
            User userDb = _mapper.Map<User>(user);
            await _db.AddAsync(userDb);
            SaveChanges();
            }
        }

        public async Task<User> GetSingle(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user != null)
            {
                return user;
            }
            throw new Exception("Record doesn't exists");
        }

        public async Task<IEnumerable<User>> Get()
        {
            var users = await _db.Users.ToListAsync<User>();
            return users;
        }
    }
}
