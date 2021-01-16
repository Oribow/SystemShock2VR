using Assets.Scripts.Editor.DarkEngine.DarkObjects;
using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Editor.DarkEngine.ObjectInstantanceAdjusters
{
    class SwitchLinkAdjustor : IObjectInstanceAdjustor
    {
        public void Process(int index, DarkObject darkObject, DarkObjectCollection collection)
        {
            foreach (var swl in darkObject.GetLinks("SwitchLink"))
            {
                var sender = swl.src.GetComponent<BasicEventSender>();
                var receiver = swl.dest.GetComponent<BasicEventReceiver>();

                if (sender != null && receiver != null)
                    sender.AddReceiver(receiver);
            }
        }
    }
}
