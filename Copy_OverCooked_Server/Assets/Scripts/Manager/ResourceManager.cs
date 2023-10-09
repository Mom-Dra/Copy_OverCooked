using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }
    public GameObject Instantiate(string path, Transform parnet = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to Load prefab : {path}");
            return null;
        }

        // "(Clone)" 문자열 지우기
        GameObject go = Object.Instantiate(prefab, parnet);
        int index = go.name.IndexOf("(Clone)");
        if (index > 0)
        {
            go.name = go.name.Substring(0, index);
        }
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }
        Object.Destroy(go);
    }
}
