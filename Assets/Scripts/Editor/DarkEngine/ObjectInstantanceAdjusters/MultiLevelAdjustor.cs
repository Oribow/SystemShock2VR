using Assets.Scripts.Editor.DarkEngine.DarkObjects;
using Assets.Scripts.Editor.DarkEngine.DarkObjects.DarkProps;
using Assets.Scripts.Events.Devices;
using UnityEditor;
using Valve.VR;

namespace Assets.Scripts.Editor.DarkEngine.ObjectInstantanceAdjusters
{
    class MultiLevelAdjustor : IObjectInstanceAdjustor
    {
        public void Process(int index, DarkObject darkObject, DarkObjectCollection collection)
        {
            var destLoc = darkObject.GetProp<DestLocProp>();
            var destLevel = darkObject.GetProp<DestLevelProp>();

            if (destLoc == null || destLevel == null)
                return;

            var lw = darkObject.gameObject.AddComponent<LevelSwitcher>();
            var svll = darkObject.gameObject.AddComponent<SteamVR_LoadLevel>();

            svll.levelName = destLevel.Value;
            svll.autoTriggerOnEnable = false;
            svll.postLoadSettleTime = 1;
            svll.loadAsync = false;

            var so = new SerializedObject(lw);

            so.FindProperty("destLoc").intValue = destLoc.Value;

            so.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
