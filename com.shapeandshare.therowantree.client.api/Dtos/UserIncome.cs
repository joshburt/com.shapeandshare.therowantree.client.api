using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Dtos
{
    public partial class UserIncome
    {
        public int UserIncomeId { get; set; }
        public int UserId { get; set; }
        public int IncomeSourceId { get; set; }
        public int Amount { get; set; }

        public virtual IncomeSourceType IncomeSource { get; set; }
        public virtual User User { get; set; }
    }
}
