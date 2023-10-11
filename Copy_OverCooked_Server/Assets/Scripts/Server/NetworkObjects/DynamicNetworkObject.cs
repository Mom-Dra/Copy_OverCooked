using UnityEngine;

public class DynamicNetworkObject : NetworkObject
{
    private Vector3 prevPosition;

    private void FixedUpdate()
    {
        if (prevPosition != transform.position)
        {
            using (Packet packet = new Packet(EActionCode.Transform, id))
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