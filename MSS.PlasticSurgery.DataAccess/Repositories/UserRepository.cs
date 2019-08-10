using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MSS.PlasticSurgery.DataAccess.Entities;
using MSS.PlasticSurgery.DataAccess.EntityFrameworkCore;
using MSS.PlasticSurgery.DataAccess.Repositories.Interfaces;

namespace MSS.PlasticSurgery.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public User GetById(int id)
        {
            return _appDbContext.Users.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _appDbContext.Users;
        }

        public User Add(User user)
        {
            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();

            return user;
        }

        public User Update(User user)
        {
            _appDbContext.Users.Attach(user).State = EntityState.Modified;
            _appDbContext.SaveChanges();

            return user;
        }

        public User Delete(int id)
        {
            User user = _appDbContext.Users.Find(id);

            if (user != null)
            {
                _appDbContext.Users.Remove(user);
                _appDbContext.SaveChanges();
            }

            return user;
        }
    }
}