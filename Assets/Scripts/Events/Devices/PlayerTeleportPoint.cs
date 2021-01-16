using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Events.Devices
{
    public class PlayerTeleportPoint : BasicEventReceiver
    {
        public override void Receive(BasicEventSender sender, DarkEvent darkEvent)
        {
            throw new NotImplementedException();
            /*
            if (darkEvent.State)
            {
                RaycastHit hit;
                Physics.Raycast(transform.position, Vector3.down, out hit, 2);

                if (hit.collider != null)
                {
                    PlayerController.Instance.TeleportTo(hit.point, transform.rotation);
                }
                else
                {
                    PlayerController.Instance.TeleportTo(transform.position, transform.rotation);
                }

            }*/
        }
    }
}