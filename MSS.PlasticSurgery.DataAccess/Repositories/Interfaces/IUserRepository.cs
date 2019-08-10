using System;
using System.Collections.Generic;
using System.Text;
using MSS.PlasticSurgery.DataAccess.Entities;

namespace MSS.PlasticSurgery.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetById(int id);

        IEnumerable<User> GetAll();

        User Add(User user);

        User Update(User user);

        User Delete(int id);
    }
}
