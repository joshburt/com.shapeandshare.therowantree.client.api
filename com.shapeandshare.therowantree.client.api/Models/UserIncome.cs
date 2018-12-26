using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Models
{
    public partial class UserIncome
    {
        public int UserIncomeId { get; set; }
        public int UserId { get; set; }
        public int IncomeSourceId { get; set; }
        public int Amount { get; set; }

        public IncomeSourceType IncomeSource { get; set; }
        public User User { get; set; }
    }
}
