using UnityEngine;

public class SendContainerArgs
{
    private InteractableObject item;
    private Container container;

    public InteractableObject Item
    {
        get
        {
            return item;
        }
        set
        {
            item = value;
        }
    }

    public Container Container
    {
        get
        {
            return container;
        }
        set
        {
            container = value;
        }
    }

    public SendContainerArgs()
    {

    }

    public SendContainerArgs(InteractableObject item, Container container)
    {
        this.item = item;
        this.container = container;
    }

    public void OnReceive()
    {
        if(container != null)
        {
            container.Remove();
        }
    }
}