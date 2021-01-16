using System;
using System.IO;

namespace Assets.Scripts.Editor.DarkEngine.DarkObjects.DarkLinks
{
    abstract class LinkData
    {
        public static string GetLinkDataName(Type linkType)
        {
            string fullName = linkType.Name;
            int cutOff = Math.Min(11, fullName.Length - 4);
            return linkType.Name.Substring(0, cutOff);
        }

        public abstract void Load(BinaryReader reader, int linkId);
    }
}
