using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Models
{
    public partial class Perk
    {
        public int UserPerkId { get; set; }
        public int UserId { get; set; }
        public int PerkId { get; set; }

        public PerkType PerkNavigation { get; set; }
        public User User { get; set; }
    }
}
