using Assets.Scripts.Editor.DarkEngine.DarkObjects;
using Assets.Scripts.Editor.DarkEngine.DarkObjects.DarkLinks;
using Assets.Scripts.Player;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor.DarkEngine.ObjectInstantanceAdjusters
{
    class LandingPointLinkAdjustor : IObjectInstanceAdjustor
    {
        public void Process(int index, DarkObject darkObject, DarkObjectCollection collection)
        {
            var spawnMarker = darkObject.GetComponent<SpawnMarker>();
            if (spawnMarker == null)
                return;

            var links = darkObject.GetLinks(typeof(LandingPointLink));
            int markerCount = links.Count;

            Transform[] markers = new Transform[markerCount];
            foreach (var swl in links)
            {
                var receiver = swl.dest.gameObject;
                markers[((LandingPointLink)swl.data).Value - 1] = receiver.transform;
            }

            var so = new SerializedObject(spawnMarker);
            var spMarker = so.FindProperty("markers");
            spMarker.arraySize = markerCount;
            for (int i = 0; i < markerCount; i++)
            {
                var elem = spMarker.GetArrayElementAtIndex(i);
                elem.objectReferenceValue = markers[i];
            }
            so.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
