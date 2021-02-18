using Assets.Scripts.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Events.Devices
{
    public class PlayerTeleportPoint : MonoBehaviour, IEventReceiver
    {
        public void Receive(IEventSender sender, DarkEvent darkEvent)
        {
            if (darkEvent.State)
            {
                RaycastHit hit;
                Physics.Raycast(transform.position, Vector3.down, out hit, 2);

                if (hit.collider != null)
                {
                    PlayerHolder.Instance.PlayerController.TeleportTo(hit.point, 1f);
                }
                else
                {
                    PlayerHolder.Instance.PlayerController.TeleportTo(transform.position, 1f);
                }

            }
        }
    }
}