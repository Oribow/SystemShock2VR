using Assets.Scripts.Editor.DarkEngine.DarkObjects;
using Assets.Scripts.Editor.DarkEngine.DarkObjects.DarkLinks;
using Assets.Scripts.Editor.DarkEngine.DarkObjects.DarkProps;
using Assets.Scripts.Editor.DarkEngine.Files;
using Assets.Scripts.Editor.DarkEngine.LevelFile;
using Assets.Scripts.Editor.DarkEngine.ObjectInstantanceAdjusters;
using Assets.Scripts.Editor.DarkEngine.SmartObjectPrefabCreator;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor.DarkEngine.Importer
{
    class ObjectTreeLoader
    {
        LevelFileLoader levelFileLoader;
        UnitySS2AssetRepository unitySS2AssetRepo;
        BinFileRepository binFileRepo;

        public ObjectTreeLoader(LevelFileRepository levelFileRepo, UnitySS2AssetRepository unitySS2AssetRepo, BinFileRepository binFileRepo)
        {
            this.levelFileLoader = new LevelFileLoader(levelFileRepo);
            this.unitySS2AssetRepo = unitySS2AssetRepo;
            this.binFileRepo = binFileRepo;
        }

        public void Load(string selectedLevel)
        {
            var levelFiles = levelFileLoader.Load(selectedLevel);
            var objectCollection = new DarkObjectCollection();
            foreach (var levelFile in levelFiles)
            {
                LoadAttributs(objectCollection, levelFile);
            }

            var processors = CreateProcessors();
            var objs = objectCollection.Where(d => d.IsInstance && d.GetParentWithId(-1) != null).ToArray();

            DestroyPreviousObjectRoot();
            InstantiateTree(objectCollection);

            for (int i = 0; i < objs.Length; i++)
            {
                var darkObj = objs[i];
                GameObject g;
                if (darkObj.Parent != null)
                {
                    g = (GameObject)PrefabUtility.InstantiatePrefab(unitySS2AssetRepo.LoadProcessedObjPrefabAsset(darkObj.Parent.FullPath() + "_" + darkObj.Parent.id));
                }
                else
                {
                    g = new GameObject(darkObj.Name);
                }

                darkObj.gameObject = g;
                PrefabCreatorUtil.AdjustPosition(darkObj);
                PrefabCreatorUtil.AddComments(darkObj);
            }

            for (int i = 0; i < objs.Length; i++)
            {
                var darkObj = objs[i];
                foreach (var processor in processors)
                {
                    processor.Process(i, darkObj, objectCollection);
                }
            }

            for (int i = 0; i < objs.Length; i++)
            {
                var darkObj = objs[i];
                GameObject g = darkObj.gameObject;
                g.transform.SetParent(darkObj.Parent.gameObject.transform, true);
            }
        }

        public void LoadRooms()
        {

        }

        private static void LoadAttributs(DarkObjectCollection coll, LevelFileGroup db)
        {
            coll.LoadPropertyChunk<SymNameProp>(db);
            coll.LoadPropertyChunk<ModelNameProp>(db);
            coll.LoadPropertyChunk<PositionProp>(db);
            coll.LoadPropertyChunk<ScaleProp>(db);
            coll.LoadPropertyChunk<RenderTypeProp>(db);
            coll.LoadPropertyChunk<ImmobileProp>(db);

            coll.LoadPropertyChunk<PhysTypeProp>(db);
            coll.LoadPropertyChunk<PhysStateProp>(db);
            coll.LoadPropertyChunk<PhysDimsProp>(db);

            coll.LoadPropertyChunk<TripFlagsProp>(db);

            coll.LoadLinkAndDataChunk<MetaPropLink>(db);
            coll.LoadLinkChunk(db, "SwitchLink");
        }

        private IObjectInstanceAdjustor[] CreateProcessors()
        {
            // order is from special to generic
            return new IObjectInstanceAdjustor[] {
                new TripWireAdjustor(),
                new SwitchLinkAdjustor(),
                new ModelAdjustor(unitySS2AssetRepo, binFileRepo)
            };
        }

        private static void DestroyPreviousObjectRoot()
        {
            var prev = GameObject.Find("Object");
            if (prev != null && prev.transform.parent == null)
                GameObject.DestroyImmediate(prev);
        }

        private void InstantiateTree(DarkObjectCollection darkObjectCollection)
        {
            // instantiate archetype objects
            foreach (var d in darkObjectCollection)
            {
                if (d.IsInstance || (d.id != -1 && d.GetParentWithId(-1) == null))
                    continue;

                d.gameObject = new GameObject(d.Name);
                PrefabCreatorUtil.AddComments(d);
            }

            // set parents
            foreach (var d in darkObjectCollection)
            {
                if (d.IsInstance)
                    continue;

                var parent = d.Parent;
                if (parent?.gameObject != null)
                {
                    d.gameObject.transform.SetParent(parent.gameObject.transform);
                }
            }
        }
    }
}
