using Assets.Scripts.DebugHelper;
using Assets.Scripts.Editor.DarkEngine.DarkObjects;
using Assets.Scripts.Editor.DarkEngine.DarkObjects.DarkProps;
using Assets.Scripts.Editor.DarkEngine.Exceptions;
using Assets.Scripts.Editor.DarkEngine.Files;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor.DarkEngine.SmartObjectPrefabCreator
{
    static class PrefabCreatorUtil
    {
        public static GameObject CreateGO(DarkObject darkObject)
        {
            GameObject g = new GameObject(darkObject.GetProp<SymNameProp>().Value);

            Debug.Assert(darkObject.gameObject == null);
            darkObject.gameObject = g;
            return g;
        }

        public static GameObject CreateModelGOAt(DarkObject darkObject, UnitySS2AssetRepository unitySS2AssetRepo, BinFileRepository binFileRepo)
        {
            Debug.Assert(darkObject.gameObject == null);
            string modelName = darkObject.GetProp<ModelNameProp>().Value.ToLower();
            if (modelName == "fx_particle")
                return new GameObject("fx_particle");
            bool isObj = binFileRepo.IsObjectMesh(modelName);

            var modelPrefab = unitySS2AssetRepo.LoadObjPrefabAsset(modelName, isObj);
            var gameObj = (GameObject)PrefabUtility.InstantiatePrefab(modelPrefab);
            gameObj.name = darkObject.GetProp<SymNameProp>().Value;

            darkObject.gameObject = gameObj;

            if (darkObject.GetProp<ImmobileProp>()?.Value ?? false)
            {
                GameObjectUtility.SetStaticEditorFlags(gameObj, ImporterSettings.staticFlags);
            }

            var renderType = darkObject.GetProp<RenderTypeProp>()?.Value ?? RenderType.Normal;
            if (renderType == RenderType.NotRendered || renderType == RenderType.EditorOnly)
            {
                var mrs = gameObj.GetComponentsInChildren<MeshRenderer>();
                foreach (var mr in mrs)
                    mr.enabled = false;
            }

            return darkObject.gameObject;
        }

        public static void ApplyCollider(DarkObject darkObject)
        {
            var type = darkObject.GetProp<PhysTypeProp>();
            var state = darkObject.GetProp<PhysStateProp>();
            var dim = darkObject.GetProp<PhysDimsProp>();

            if (type == null || state == null || dim == null)
            {
                throw new DarkException("Can't apply collider due to missing props.");
            }

            switch (type.type)
            {
                case ModelType.OBB:
                    var boxCol = darkObject.gameObject.AddComponent<BoxCollider>();
                    boxCol.center = dim.offset[0];
                    boxCol.size = new Vector3(Mathf.Abs(dim.size.x), Mathf.Abs(dim.size.y), Mathf.Abs(dim.size.z));
                    break;
                case ModelType.Sphere:
                    var sphereCol = darkObject.gameObject.AddComponent<SphereCollider>();
                    sphereCol.center = dim.offset[0];
                    sphereCol.radius = dim.radius[0];
                    break;

            }
        }

        public static void AddComments(DarkObject dobj)
        {
            var comment = dobj.gameObject.AddComponent<CommentBox>();
            dobj.WriteAsComment(comment);
        }

        public static void AdjustPosition(DarkObject dobj)
        {
            var propPos = dobj.GetProp<PositionProp>();
            if (propPos == null)
                throw new DarkException("Tried to adjust position of object with no prop pos. " + dobj.ToString());

            Quaternion rotation = Quaternion.AngleAxis(propPos.facing.y, Vector3.up) *
                Quaternion.AngleAxis(propPos.facing.z, Vector3.forward) *
                Quaternion.AngleAxis(propPos.facing.x, Vector3.right);

            Vector3 scale = Vector3.one;
            var propScale = dobj.GetProp<ScaleProp>();
            if (propScale != null)
            {
                scale = propScale.Value;
            }

            var m = Matrix4x4.TRS(propPos.position, rotation, scale);

            m = ImporterSettings.modelCoorTransl * m * ImporterSettings.modelCoorTransl.inverse;
            dobj.gameObject.transform.localPosition = m.GetPosition();
            dobj.gameObject.transform.localRotation = rotation;
            dobj.gameObject.transform.localScale = m.lossyScale;
        }
    }
}
