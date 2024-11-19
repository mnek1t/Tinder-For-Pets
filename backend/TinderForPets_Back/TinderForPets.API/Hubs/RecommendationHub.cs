using Microsoft.AspNetCore.SignalR;
using TinderForPets.Application.Services;
using TinderForPets.Core.Models;

namespace TinderForPets.API.Hubs
{
    public class RecommendationHub : Hub
    {
        private readonly RecommendationService _recommendationService;
        public RecommendationHub(RecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }
        public override async Task OnConnectedAsync() 
        {
            await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined.");

        }
        //public static async Task SendRecommendation(string user, object recommendation) 
        //{
        //    await Clients.User(user).SendAsync("ReceiveRecommendation", recommendation);
        //}
    }
}
