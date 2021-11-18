using System;
using System.Collections.Generic;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Framework.Models;
using mesoBoard.Services;
using Microsoft.AspNetCore.Mvc;

namespace mesoBoard.Web.ViewComponents
{
    public class BoardStatsViewComponent : ViewComponent
    {
        private readonly IRepository<OnlineUser> _onlineUserRepository;
        private readonly IRepository<OnlineGuest> _onlineGuestRepository;
        private readonly IRepository<User> _userRepository;
        private readonly UserServices _userServices;
        private readonly PostServices _postServices;
        private readonly ThreadServices _threadServices;

        public BoardStatsViewComponent(
            IRepository<OnlineUser> onlineUserRepository,
            IRepository<OnlineGuest> onlineGuestRepository,
            IRepository<User> userRepository,
            UserServices userServices,
            PostServices postServices,
            ThreadServices threadServices
        )
        {
            _onlineUserRepository = onlineUserRepository;
            _onlineGuestRepository = onlineGuestRepository;
            _userRepository = userRepository;
            _userServices = userServices;
            _postServices = postServices;
            _threadServices = threadServices;
        }

        public IViewComponentResult Invoke()
        {
            var onlineUsers = _onlineUserRepository.Get().Select(item =>
                new OnlineUserDetails()
                {
                    DefaultRole = item.User.UserProfile.DefaultRole.HasValue ? item.User.UserProfile.Role : null,
                    OnlineUser = item
                }).ToList();

            var onlineGuests = _onlineGuestRepository.Get().ToList();

            var newestUser = _userRepository.Get().OrderByDescending(item => item.RegisterDate).First();
            var birthdays = _userServices.GetBirthdays(DateTime.UtcNow);

            var model = new BoardStatsViewModel()
            {
                NewestUser = newestUser,
                OnlineGuests = onlineGuests,
                OnlineUsers = onlineUsers,
                TotalPosts = _postServices.TotalPosts(),
                TotalRegisteredUsers = _userRepository.Get().Count(),
                TotalThreads = _threadServices.TotalThreads(),
                BirthdayUsers = birthdays
            };

            ViewData["passwordHash"] = _userServices.HashPassword("password", "test");

            return View(model);
        }
    }
}

