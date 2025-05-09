using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drillex.Common.Scripts
{
    public interface IUpgradable
    {
        public ulong UpgradePrice { get; }
        
        bool Upgrade();
        string UpgradeText();
        string GetPriceText();
    }
}
