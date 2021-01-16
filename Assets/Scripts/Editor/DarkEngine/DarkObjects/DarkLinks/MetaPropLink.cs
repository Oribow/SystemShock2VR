using System.IO;

namespace Assets.Scripts.Editor.DarkEngine.DarkObjects.DarkLinks
{
    class MetaPropLink : LinkData
    {
        public int priority;
        public override void Load(BinaryReader reader, int linkId)
        {
            priority = reader.ReadInt32();
        }
    }
}
