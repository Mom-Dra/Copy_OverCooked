using UnityEngine;

public class Water : SerializedObject
{
    private void OnParticleTrigger()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.2f);
        foreach (Collider collider in hits)
        {
            if (collider.TryGetComponent<FireTriggerBox>(out FireTriggerBox fireTriggerBox))
            {
                if (fireTriggerBox.OnFire)
                {
                    fireTriggerBox.HeatDown();
                }
            }
        }
    }
}