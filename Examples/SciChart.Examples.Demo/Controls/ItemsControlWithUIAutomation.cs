using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using SciChart.Examples.Demo.Helpers;

namespace SciChart.Examples.Demo.Controls
{
    public class ItemsControlWithUIAutomation : ItemsControl
    {
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new GenericAutomationPeer(this);
        }
    }
}
