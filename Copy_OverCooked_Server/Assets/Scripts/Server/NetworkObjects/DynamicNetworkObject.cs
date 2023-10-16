using UnityEngine;

public class DynamicNetworkObject : MonoBehaviour
{
    private SerializedObject serializedObject;
    private Vector3 prevPosition;

    private void Awake()
    {
        serializedObject = GetComponent<SerializedObject>();
        prevPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (prevPosition != transform.position)
        {
            using (Packet packet = new Packet(EActionCode.Transform, serializedObject.Id))
            {
                packet.Write(transform.position);
                packet.Write(transform.rotation);
                packet.Write(transform.localScale);
                Server.Instance.SendToAllClients(packet);
            }
        }
        prevPosition = transform.position;
    }
}