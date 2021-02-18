using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.Events.Devices
{
    public class LevelSwitcher : MonoBehaviour, IEventReceiver
    {
        [SerializeField]
        int destLoc;

        public void Receive(IEventSender sender, DarkEvent ev)
        {
            PlayerHolder.Instance.TeleportOnLevelLoad.SetTargetPosition(destLoc);
            GetComponent<Valve.VR.SteamVR_LoadLevel>().Trigger();
        }
    }
}
