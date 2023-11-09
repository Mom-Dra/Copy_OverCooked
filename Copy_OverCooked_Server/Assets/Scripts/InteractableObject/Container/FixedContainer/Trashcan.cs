using System.Collections;
using UnityEngine;

public class Trashcan : FixedContainer
{
    [Header("Trashcan")]
    [SerializeField]
    private GameObject slowDestroyGO;

    public override bool TryPut(InteractableObject interactableObject)
    {
        StartCoroutine(SlowDestroy(interactableObject));
        return true;
    }

    private IEnumerator SlowDestroy(InteractableObject destroyObject)
    {
        destroyObject.Selectable = false;
        destroyObject.Fix();
        destroyObject.GetComponent<Collider>().isTrigger = true;

        GameObject animGO = Instantiate(slowDestroyGO, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity, transform);
        Transform anchorGO = animGO.transform.GetChild(0);
        destroyObject.transform.parent = anchorGO;
        destroyObject.transform.position = anchorGO.position;

        Animation anim = animGO.GetComponent<Animation>();

        anim.Play();
        while(anim.isPlaying)
        {
            yield return null;
        }
        Destroy(animGO);
    }
}