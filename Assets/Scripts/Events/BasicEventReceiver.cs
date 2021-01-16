using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public abstract class BasicEventReceiver : MonoBehaviour
    {
        public abstract void Receive(BasicEventSender sender, DarkEvent ev);
    }
}
