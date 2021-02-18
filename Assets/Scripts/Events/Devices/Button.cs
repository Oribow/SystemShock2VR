using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.VR.InteractionSystem;

namespace Assets.Scripts.Events.Devices
{
    public class Button : BasicEventSender
    {
        private void OnHandHoverBegin(Hand hand)
        {
            SendEvent(new DarkEvent(true));
        }
    }
}
