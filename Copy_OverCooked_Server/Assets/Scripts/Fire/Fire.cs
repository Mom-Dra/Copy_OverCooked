using UnityEngine;

public class Fire : SerializedObject
{
    private void OnParticleTrigger()
    {
        //Collider[] hits = Physics.OverlapSphere(transform.position, 1f);
        //Debug.Log($"{name}, hits : {hits.Length}");
        //foreach (Collider collider in hits)
        //{

        //Debug.Log($"{name}, hit name : {collider.name}");
        //    if(collider.TryGetComponent<FireTriggerBox>(out FireTriggerBox fireTriggerBox))
        //    {
        //        if (!fireTriggerBox.OnFire)
        //        {
        //            fireTriggerBox.HeatUp();
        //        }
        //    }
        //}
    }
}