using System;
using System.Collections.Generic;
using System.Linq;
using mesoBoard.Common;
using mesoBoard.Data;

namespace mesoBoard.Services
{
    public class PollServices : BaseService
    {
        private IRepository<PollVote> _pollVoteRepository;
        private IRepository<Poll> _pollRepository;
        private IRepository<Thread> _threadRepository;

        public PollServices(
            IRepository<PollVote> pollVoteRepository,
            IRepository<Poll> pollRepository,
            IRepository<Thread> threadRepository,
            IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _pollVoteRepository = pollVoteRepository;
            _pollRepository = pollRepository;
            _threadRepository = threadRepository;
        }

        public bool HasVoted(int pollID, int userID)
        {
            IEnumerable<PollVote> votes = _pollVoteRepository.Where(item => item.PollOption.PollID.Equals(pollID)).ToList();
            return votes.Any(x => x.PollOption.PollID == pollID && x.UserID == userID);
        }

        public void DeletePoll(int PollID)
        {
            _pollRepository.Delete(PollID);
            Thread thread = _threadRepository.Get(PollID);
            thread.HasPoll = false;
            _threadRepository.Update(thread);
            _unitOfWork.Commit();
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
            _unitOfWork.Commit();
            return vote;
        }

        public void CreatePoll(string pollQuestion, string pollOptions, int threadID)
        {
            string[] splitOptions = pollOptions.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            CreatePoll(pollQuestion, splitOptions, threadID);
        }

        public void CreatePoll(string pollQuestion, string[] pollOptions, int threadID)
        {
            System.Data.Objects.DataClasses.EntityCollection<PollOption> toadd = new System.Data.Objects.DataClasses.EntityCollection<PollOption>();

            foreach (string po in pollOptions)
            {
                toadd.Add(new PollOption
                {
                    Text = po,
                    PollID = threadID
                });
            }

            Poll ThePoll = new Poll
            {
                PollOptions = toadd,
                Question = pollQuestion,
                PollID = threadID
            };

            _pollRepository.Add(ThePoll);
            Thread thread = _threadRepository.Get(threadID);
            thread.HasPoll = true;
            _threadRepository.Update(thread);
            _unitOfWork.Commit();
        }
    }
}