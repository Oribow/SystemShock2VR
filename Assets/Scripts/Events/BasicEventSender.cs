using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Events
{
    public abstract class BasicEventSender : MonoBehaviour
    {
        [SerializeField]
        protected List<BasicEventReceiver> receivers = new List<BasicEventReceiver>();

        public void AddReceiver(BasicEventReceiver receiver)
        {
            receivers.Add(receiver);
        }

        public void RemoveReceiver(BasicEventReceiver receiver)
        {
            receivers.Remove(receiver);
        }

        protected void SendEvent(DarkEvent ev)
        {
            foreach (var r in receivers)
                r.Receive(this, ev);
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var r in receivers)
            {
                Gizmos.DrawLine(transform.position, r.transform.position);
            }
        }
    }
}
