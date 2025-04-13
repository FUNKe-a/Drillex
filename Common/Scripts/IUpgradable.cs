using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drillex.Common.Scripts
{
    public interface IUpgradable
    {
        void Upgrade();

        string UpgradeText();
        bool SufficientFunds();
        string GetPrice();
    }
}
