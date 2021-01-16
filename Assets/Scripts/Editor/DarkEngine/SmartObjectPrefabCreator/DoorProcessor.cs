using Assets.Scripts.Editor.DarkEngine.DarkObjects;
using Assets.Scripts.Editor.DarkEngine.DarkObjects.DarkProps;
using Assets.Scripts.Events.Devices;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor.DarkEngine.SmartObjectPrefabCreator
{
    class DoorProcessor : ISmartObjectPrefabCreator
    {
        public void ApplyFlags(DarkObject darkObject, HashSet<Type> flags)
        {
            if (flags.Contains(typeof(DecorationCreator)) && darkObject.HasProp<TransDoorProp>())
            {
                flags.Add(typeof(DoorProcessor));
            }
        }

        public void Initialize(int objCount)
        {
        }

        public void Preprocess(int index, DarkObject darkObject, DarkObjectCollection collection)
        {
            
        }

        public void Process(int index, DarkObject darkObject, DarkObjectCollection collection)
        {
            TransDoorProp door = darkObject.GetProp<TransDoorProp>();
            GameObject g = darkObject.gameObject;

            var doorComp = g.AddComponent<Door>();
            SerializedObject so = new SerializedObject(doorComp);

            so.FindProperty("closed").floatValue = door.closed;
            so.FindProperty("open").floatValue = door.open;
            so.FindProperty("speed").floatValue = door.baseSpeed;

            switch (door.axis)
            {
                case 0:
                    so.FindProperty("moveAxis").vector3Value = Vector3.left;
                    break;
                case 1:
                    so.FindProperty("moveAxis").vector3Value = Vector3.forward;
                    break;
                case 2:
                    so.FindProperty("moveAxis").vector3Value = Vector3.up;
                    break;
            }
            so.FindProperty("status").enumValueIndex = (int)door.state;
            so.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
