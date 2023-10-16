using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

public class SendObjectArgs : IDisposable
{
    private InteractableObject item;
    private List<EObjectSerialCode> containObjects = new List<EObjectSerialCode>();

    public InteractableObject Item
    {
        get => item;
    }

    public List<EObjectSerialCode> ContainObjects
    {
        get => containObjects;
    }

    public SendObjectArgs(InteractableObject item, List<EObjectSerialCode> containObjects)
    {
        this.item = item;
        if(containObjects != null)
        {
            foreach (EObjectSerialCode serialCode in containObjects)
            {
                this.containObjects.Add(serialCode);
            }
        } 
        else
        {
            this.containObjects.Add(item.SerialCode);
        }
        
    }

    public void Dispose()
    {
        item = null;
        containObjects = null;
        GC.SuppressFinalize(this);
    }
}