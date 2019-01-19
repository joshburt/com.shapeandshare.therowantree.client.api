using System;
using System.Collections.Generic;

namespace com.shapeandshare.therowantree.client.api.Dtos
{
    public partial class Perk
    {
        public int UserPerkId { get; set; }
        public int UserId { get; set; }
        public int PerkId { get; set; }

        public virtual PerkType PerkNavigation { get; set; }
        public virtual User User { get; set; }
    }
}
