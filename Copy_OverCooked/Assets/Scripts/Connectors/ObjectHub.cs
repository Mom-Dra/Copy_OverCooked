using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectHub : MonoBehaviour
{
    private static ObjectHub instance;

    [SerializeField]
    private Dictionary<Player, InteractableObject> linkedObjects;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            linkedObjects = new Dictionary<Player, InteractableObject>();
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(this);
        }
    }


    public static ObjectHub Instance
    {
        get
        {
            return instance;
        }
    }

    public bool Connect(Player player, InteractableObject IObject)
    {
        if (!linkedObjects.ContainsKey(player) && !linkedObjects.ContainsValue(IObject))
        {
            linkedObjects.Add(player, IObject);
            return true;

        }
        return false;
    }

    public void Disconnect(Player player)
    {
        linkedObjects.Remove(player);
    }

    public void Disconnect(InteractableObject IObject)
    {
        Player player = linkedObjects.FirstOrDefault(item => item.Value == IObject).Key;
        linkedObjects.Remove(player);
    }

    public InteractableObject GetLinkedObject(Player player)
    {
        if (linkedObjects.ContainsKey(player))
        {
            return linkedObjects[player];
        }
        throw new System.Exception("Invalid Linked Key");
    }

    public Player GetLinkedObject(InteractableObject IObject)
    {
        if (linkedObjects.ContainsValue(IObject))
        {
            return linkedObjects.FirstOrDefault(item => item.Value == IObject).Key;
        }
        throw new System.Exception("Invalid Linked Value");
    }
}