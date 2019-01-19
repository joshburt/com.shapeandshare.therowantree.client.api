using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Dtos
{
    public partial class IncomeSourceType
    {
        public IncomeSourceType()
        {
            IncomeSource = new HashSet<IncomeSource>();
            UserIncome = new HashSet<UserIncome>();
        }

        public int IncomeSourceId { get; set; }
        public string IncomeSourceName { get; set; }
        public string IncomeSourceDescription { get; set; }

        public virtual ICollection<IncomeSource> IncomeSource { get; set; }
        public virtual ICollection<UserIncome> UserIncome { get; set; }
    }
}
