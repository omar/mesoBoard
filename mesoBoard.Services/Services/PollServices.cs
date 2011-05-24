using System;
using System.Collections.Generic;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;

namespace mesoBoard.Services
{
    public class PollServices 
    {
        IRepository<PollVote> _pollVoteRepository;
        IRepository<Poll> _pollRepository;
        IRepository<Thread> _threadRepository;

        public PollServices(
            IRepository<PollVote> pollVoteRepository,
            IRepository<Poll> pollRepository,
            IRepository<Thread> threadRepository)
        {
            _pollVoteRepository = pollVoteRepository;
            _pollRepository = pollRepository;
            _threadRepository = threadRepository;
        }

        public bool HasVoted(int pollID, int userID)
        {
            IEnumerable<PollVote> votes = _pollVoteRepository.Where(item => item.PollOption.PollID.Equals(pollID));

            return votes.Any(x => x.PollOption.PollID == pollID && x.UserID == userID);
        }

        public void DeletePoll(int PollID)
        {
            _pollRepository.Delete(PollID); 
            Thread thread = _threadRepository.Get(PollID);
            thread.HasPoll = false;
            _threadRepository.Update(thread);
        }

        public void CreatePoll(string pollQuestion, string pollOptions, int threadID)
        {
            string[] splitOptions = pollOptions.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            CreatePoll(pollQuestion, splitOptions, threadID);
        }

        public Poll GetPoll(int pollID)
        {
            return _pollRepository.Get(pollID);
        }

        public PollVote CastVote(int userID, int pollOptionID)
        {
            PollVote vote = new PollVote()
            {
                PollOptionID = pollOptionID,
                UserID = userID
            };
            _pollVoteRepository.Add(vote);
            return vote;
        }

        public void CreatePoll(string pollQuestion, string[] pollOptions, int threadID)
        {
            System.Data.Objects.DataClasses.EntityCollection<PollOption>  toadd = new System.Data.Objects.DataClasses.EntityCollection<PollOption>();

            foreach (string po in pollOptions)
            {
                toadd.Add(new PollOption
                {
                    Text =  po,
                    PollID = threadID
                });
                    
            }

            Poll ThePoll = new Poll
            {
                PollOptions = toadd,
                Question =  pollQuestion,
                PollID = threadID
            };

            _pollRepository.Add(ThePoll);
            Thread thread = _threadRepository.Get(threadID);
            thread.HasPoll = true;
            _threadRepository.Update(thread);
        }

        //public bool CanCastVote(int pollID, int userID)
        //{
        //    var poll = _pollRepository.Get(pollID);

        //    var pollingPermissions = _permissionServices.(poll.Thread.ForumID, userID, Permission.Polling);
        //    if(pollingPermissions 

        //    var pollVote = _pollVoteRepository.First(item => item.PollOption.PollID == pollID && item.UserID == userID);
        //    if (pollVote != null)
        //        return false;

            

        //    if (poll.Thread.IsLocked)
        //        return false;

            
        //}
    }
}