using Assets.Scripts.Editor.DarkEngine.DarkObjects;
using Assets.Scripts.Editor.DarkEngine.DarkObjects.DarkProps;
using Assets.Scripts.Editor.DarkEngine.Files;
using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor.DarkEngine.ObjectInstantanceAdjusters
{
    class ModelAdjustor : IObjectInstanceAdjustor
    {
        UnitySS2AssetRepository unitySS2AssetRepo;
        BinFileRepository binFileRepo;

        public ModelAdjustor(UnitySS2AssetRepository unitySS2AssetRepo, BinFileRepository binFileRepo)
        {
            this.unitySS2AssetRepo = unitySS2AssetRepo;
            this.binFileRepo = binFileRepo;
        }

        public void Process(int index, DarkObject darkObject, DarkObjectCollection collection)
        {
            if (darkObject.HasPropDirectly<ModelNameProp>())
                InstanceAdjustorUtil.ChangeModelAt(darkObject, unitySS2AssetRepo, binFileRepo);

            if (darkObject.HasPropDirectly<RenderTypeProp>())
            {
                var renderType = darkObject.GetProp<RenderTypeProp>().Value;
                if (renderType == RenderType.NotRendered || renderType == RenderType.EditorOnly)
                {
                    var mrs = darkObject.gameObject.GetComponentsInChildren<MeshRenderer>();
                    foreach (var mr in mrs)
                        mr.enabled = false;
                }
            }

            if ((!darkObject.HasPropDirectly<ImmobileProp>() || darkObject.GetProp<ImmobileProp>().Value))
            {
                GameObjectUtility.SetStaticEditorFlags(darkObject.gameObject, ImporterSettings.staticFlags);
            }
        }
    }
}
