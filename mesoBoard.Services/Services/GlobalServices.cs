using System;
using System.Collections.Generic;
using mesoBoard.Common;
using mesoBoard.Data;
using mesoBoard.Data.Repositories;

namespace mesoBoard.Services
{
    public class GlobalServices : BaseService 
    {
        IRepository<OnlineGuest> _onlineGuestRepository;
        IRepository<OnlineUser> _onlineUserRepository;
        StoredProcedures _storedProcedures;

        public GlobalServices(
            IRepository<OnlineGuest> onlineGuests, 
            IRepository<OnlineUser> onlineUsers, 
            StoredProcedures storedProcedures,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _onlineGuestRepository = onlineGuests;
            _onlineUserRepository = onlineUsers;
            _storedProcedures = storedProcedures;
        }

        public void PruneOnlineGuests()
        {
            IEnumerable<OnlineGuest> guests = _storedProcedures.Get_Inactive_OnlineGuests();
            _onlineGuestRepository.Delete(guests);
            _unitOfWork.Commit();
        }

        public void PruneOnlineUsers()
        {
            IEnumerable<OnlineUser> users = _storedProcedures.Get_Inactive_OnlineUsers();
            _onlineUserRepository.Delete(users);
            _unitOfWork.Commit();
        }


        public void OnlineUserRoutine(User user, string ipAddress)
        {
            bool IsAuthenticated = user != null;

            if (IsAuthenticated)
            {
                OnlineUser onlineUser = _onlineUserRepository.Get(user.UserID);
                OnlineGuest onlineGuest = _onlineGuestRepository.First(item => item.IP.Equals(ipAddress));

                if (onlineGuest != null)
                    _onlineGuestRepository.Delete(onlineGuest.OnlineGuestID);

                if (onlineUser == null)
                {
                    _onlineUserRepository.Add(new OnlineUser
                    {
                        UserID = user.UserID,
                        Date = DateTime.UtcNow,
                    }
                    );
                }
                else
                {
                    onlineUser.Date = DateTime.UtcNow;
                    _onlineUserRepository.Update(onlineUser);
                }
            }
            else
            {
                OnlineGuest onlineGuest = _onlineGuestRepository.First(item => item.IP.Equals(ipAddress));
                if (onlineGuest == null)
                {
                    _onlineGuestRepository.Add(new OnlineGuest
                    {
                        IP = ipAddress,
                        Date = DateTime.UtcNow
                    });
                }
                else
                {
                    onlineGuest.Date = DateTime.UtcNow;
                    _onlineGuestRepository.Update(onlineGuest);
                }
            }

            _unitOfWork.Commit();
        }
    }
}