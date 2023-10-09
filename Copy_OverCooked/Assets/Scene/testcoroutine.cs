using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcoroutine : MonoBehaviour
{
    private IEnumerator select;

    public float interval = 0.01f;

    void Awake()
    {
        select = coroutiune1();
        StartCoroutine(select);
    }

    private IEnumerator coroutiune1()
    {
        Debug.Log("abc");
        yield return null;
        Debug.Log("Go Go");
        StopCoroutine(select);
        yield return null;
        //yield return new WaitForSeconds(interval);
        Debug.Log("Stop");
        yield return null;
        Debug.Log("Stop2");
    }

    
}
