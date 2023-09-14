using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class A
{

}

public class Test : MonoBehaviour
{
    List<A> abc = new List<A>();

    // Start is called before the first frame update
    void Start()
    {
        A a1 = new A();
        A a2 = new A();

        abc.Add(a1);

        abc.Remove(a2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
