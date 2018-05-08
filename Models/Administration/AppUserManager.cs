using Andromeda.Models.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Models.Administration
{
    public class UserStore : IUserStore<User, Guid>
    {
        public DBContext Context { get; set; }

        public UserStore(DBContext db)
        {
            Context = db;
        }

        public async Task CreateAsync(User user)
        {
            await Task.Run(() =>
            {
                Context.Users.Add(user);
            });
        }

        public async Task DeleteAsync(User user)
        {
            await Task.Run(() =>
            {
                Context.Users.Remove(user);
            });
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public async Task<User> FindByIdAsync(Guid userId)
        {
            User user = null;
            await Task.Run(() =>
            {
                user = Context.Users.Find(userId);
            });

            return user;
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            User user = null;
            await Task.Run(() =>
            {
                user = Context.Users.SingleOrDefault(o => o.UserName == userName);
            });

            return user;
        }

        public async Task UpdateAsync(User newUser)
        {
            await Task.Run(() =>
            {
                User user = Context.Users.Find(newUser.Id);
                user.UserName = newUser.UserName;                
                user.LastName = user.LastName;
                user.Login = user.Login;
                user.Password = newUser.Password;
                user.Patronimyc = newUser.Patronimyc;
                user.UserRoles = newUser.UserRoles;

                Context.SaveChanges();
            });
        }
    }

    public class AppUserManager : UserManager<User, Guid>
    {
        public AppUserManager(IUserStore<User, Guid> store) : base(store)
        {
        }
        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options,
            IOwinContext context)
        {
            DBContext db = context.Get<DBContext>();
            AppUserManager manager = new AppUserManager(new UserStore(db));
            return manager;
        }
    }
}
