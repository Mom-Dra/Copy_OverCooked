using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [Header("Trigger")]
    [SerializeField]
    private float brightness = 0.4f;

    [Header("Debug")]
    [SerializeField]
    private List<InteractableObject> interactableObjects;

    //[HideInInspector]
    public InteractableObject ClosestInteractableObject;

    private void Awake()
    {
        interactableObjects = new List<InteractableObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableObject interactableObject = other.GetComponent<InteractableObject>();
        if (interactableObject != null && interactableObject.IsInteractable)
        {
            interactableObjects.Add(interactableObject);
            SetClosestObject();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObject io = other.GetComponent<InteractableObject>();
        interactableObjects.Remove(io);
        SetClosestObject();
    }

    private void SetClosestObject()
    {
        if(ClosestInteractableObject != null)
        {
            ClosestInteractableObject.GlowOff();
        }


        ClosestInteractableObject = interactableObjects.OrderBy(item => Vector3.Distance(ConvertYPositionToZero(item.transform.position), ConvertYPositionToZero(transform.position + Vector3.forward)))
        .FirstOrDefault();

        if (ClosestInteractableObject != null)
        {
            ClosestInteractableObject.GlowOn();
        }
    }

    private Vector3 ConvertYPositionToZero(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }

    // 요리가 다 되서 음식이 바뀌거나 하는 등의 경우에는 MissingObject 오류가 발생하게 된다 
    // 다른 함수, 객체에서 아래 함수를 호출하여 리스트에서 대상을 지우고 ClosestObject를 재설정해주어야 한다 
    // 이제 아님 (사용할 일 없을듯)
    public void RemoveObject(InteractableObject io)
    {
        interactableObjects.Remove(io);
        SetClosestObject();
    }

    public bool ContainObject(InteractableObject io)
    {
        return interactableObjects.Contains(io);
    }
}