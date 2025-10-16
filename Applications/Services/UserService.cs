using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Identity;
using Models;

namespace Applications.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepo;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserService(IUserRepository userRepo , SignInManager<ApplicationUser> signInManager)
        {
            this.userRepo = userRepo;
            this.signInManager = signInManager;
        }

    }
}
