﻿using DriveSafe.Users.Data;
using DriveSafe.Users.Models;
using DriveSafe.Users.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DriveSafe.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}