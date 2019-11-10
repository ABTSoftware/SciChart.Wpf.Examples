using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media;

namespace SciChart.Examples.Demo.Helpers
{
    /// <summary>
    /// Used to assist automation peers for ItemsControl. See also <see cref="ItemsControlAutomationPeer"/>
    /// </summary>
    public class GenericAutomationPeer : UIElementAutomationPeer
    {
        public GenericAutomationPeer(UIElement owner) : base(owner)
        {
        }

        protected override List<AutomationPeer> GetChildrenCore()
        {
            var list = base.GetChildrenCore();
            list.AddRange(GetChildPeers(Owner));
            return list;
        }

        private List<AutomationPeer> GetChildPeers(UIElement element)
        {
            var list = new List<AutomationPeer>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as UIElement;
                if (child != null)
                {
                    AutomationPeer childPeer;
                    if (child is GroupItem)
                    {
                        childPeer = new GenericAutomationPeer(child);
                    }
                    else
                    {
                        childPeer = UIElementAutomationPeer.CreatePeerForElement(child);
                    }
                    if (childPeer != null)
                    {
                        list.Add(childPeer);
                    }
                    else
                    {
                        list.AddRange(GetChildPeers(child));
                    }
                }
            }
            return list;
        }
    }
}
