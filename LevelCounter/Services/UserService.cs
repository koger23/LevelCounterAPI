using AutoMapper;
using LevelCounter.Models;
using LevelCounter.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelCounter.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private const string DEFAULT_ROLE = "User";
        private readonly string apiSecretKey;
        private readonly IMapper mapper;
        private readonly ApplicationContext context;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IMapper mapper, ApplicationContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            apiSecretKey = configuration.GetSection("APISecretKey").Value;
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<List<Relationship>> GetFriendsAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var friends = context.Relationships
                .Where(r => r.State == "confirmed")
                .Where(r => r.RelatingUserId == user.Id || r.UserId == user.Id)
                .ToList();
            return friends;
        }

        public async Task<List<Relationship>> GetPendingRequestsAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var requests = context.Relationships
                .Where(r => r.State == "pending")
                .Where(r => r.RelatingUserId == user.Id)
                .ToList() ?? new List<Relationship>();
            return requests;
        }

        public async Task<List<Relationship>> GetUnconfirmedAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var requests = context.Relationships
                .Where(r => r.State == "pending")
                .Where(r => r.UserId == user.Id)
                .ToList() ?? new List<Relationship>();
            return requests;
        }

        public async Task<List<Relationship>> GetBlockedAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var requests = context.Relationships
                .Where(r => r.State == "blocked")
                .Where(r => r.UserId == user.Id)
                .ToList() ?? new List<Relationship>();
            return requests;
        }
    }
}
