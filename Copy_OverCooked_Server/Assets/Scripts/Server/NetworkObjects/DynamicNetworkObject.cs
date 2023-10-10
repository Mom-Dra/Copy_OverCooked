using UnityEngine;

public class DynamicNetworkObject : NetworkObject
{
    private Vector3 prevPosition;

    private void FixedUpdate()
    {
        if (prevPosition != transform.position)
        {
            Packet packet = new Packet(EActionCode.Transform, id);
            Server.Instance.SendToAllClients(packet);
        }
        prevPosition = transform.position;
    }
}