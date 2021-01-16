using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AssetDatabase.StartAssetEditing();
        try
        {
            Texture2D tex = new Texture2D(10, 10);
            tex.SetPixel(0, 0, Color.magenta);
            tex.Apply();
            var path = Path.Combine(Directory.GetParent(Application.dataPath).ToString(), "Assets/image.png");
            System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
            AssetDatabase.ImportAsset(path);
            
            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            Material mat = new Material(Shader.Find("Diffuse"));
            mat.mainTexture = tex;

            AssetDatabase.CreateAsset(mat, "Assets/mat.mat");

            GameObject g = new GameObject("g", typeof(MeshRenderer));
            g.GetComponent<MeshRenderer>().sharedMaterial = mat;

            PrefabUtility.SaveAsPrefabAsset(g, "Assets/g.prefab");
        }
        finally {
            AssetDatabase.StopAssetEditing();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
