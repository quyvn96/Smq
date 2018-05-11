using Smq.Data.Infrastructure;
using Smq.Data.Repositories;
using Smq.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smq.Service
{
    public interface IFeedbackService
    {
        Feedback Create(Feedback feedback);
        IEnumerable<Feedback> GetAll(string keyword);
        bool UpdateFeedbackStatus(int id, bool status);
        Feedback DeleteFeedback(int id);
        void Save();
    }
    public class FeedbackService:IFeedbackService
    {
        IFeedbackRepository _feedbackRepository;
        IUnitOfWork _unitOfWork;
        public FeedbackService(IFeedbackRepository feedbackRepository, IUnitOfWork unitOfWork)
        {
            this._feedbackRepository = feedbackRepository;
            this._unitOfWork = unitOfWork;
        }
        public Feedback Create(Feedback feedback)
        {
            return _feedbackRepository.Add(feedback);
        }

        public IEnumerable<Feedback> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _feedbackRepository.GetMulti(n => n.Name.Contains(keyword) || n.Email.Contains(keyword));
            }
            else
            {
                return _feedbackRepository.GetAll();
            }
        }
        public bool UpdateFeedbackStatus(int id, bool status)
        {
            return _feedbackRepository.UpdateFeedbackStatus(id, status);
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }

        public Feedback DeleteFeedback(int id)
        {
            if(id != 0)
            {
                return _feedbackRepository.Delete(id);
            }
            else
            {
                return new Feedback();
            }
        }
    }
}
