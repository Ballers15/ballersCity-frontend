using System.Collections.Generic;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Domain
{
    public class LevelUp
    {
        public int level { get; set; }

        public IEnumerable<Reward> rewards { get; set; }

        public virtual void Apply(object sender)
        {
            foreach (var reward in rewards)
            {
                reward.Apply(sender, false);
            }
        }
    }
}