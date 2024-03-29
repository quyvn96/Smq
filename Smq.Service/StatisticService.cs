﻿using Smq.Common.ViewModels;
using Smq.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smq.Service
{
    public interface IStatisticService
    {
        IEnumerable<RevenueStatisticViewModel> GetGetRevenueStatistic(string fromDate = null, string toDate=null);
    }
    public class StatisticService:IStatisticService
    {
        IOrderRepository _orderRepository;
        public StatisticService(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }
        public IEnumerable<RevenueStatisticViewModel> GetGetRevenueStatistic(string fromDate = null, string toDate = null)
        {
            var data = _orderRepository.GetGetRevenueStatistic(fromDate, toDate);
            return data;
        }
    }
}
