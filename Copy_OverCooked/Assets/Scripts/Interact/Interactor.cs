using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public static readonly List<Interactor> interactors = new List<Interactor>();

    [Header("Debug")]
    [SerializeField]
    private List<InteractableObject> interactableObjects;

    [HideInInspector]
    public InteractableObject ClosestInteractableObject;

    private void Awake()
    {
        interactableObjects = new List<InteractableObject>();
        interactors.Add(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableObject interactableObject = other.GetComponent<InteractableObject>();
        if (interactableObject != null && interactableObject.IsInteractable)
        {
            interactableObjects.Add(interactableObject);
            SetClosestObject();
            GlowOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObject io = other.GetComponent<InteractableObject>();
        GlowOff(io);
        interactableObjects.Remove(io);
        SetClosestObject();
    }

    private void GlowOn()
    {
        if (ClosestInteractableObject != null)
        {
            Material material = ClosestInteractableObject.GetComponent<MeshRenderer>().material;
            material.SetColor("_EmissionColor", new Color(10, 10, 10));
        }
    }

    private void GlowOff(InteractableObject gameObject)
    {
        if (gameObject != null)
        {
            Material material = gameObject.GetComponent<MeshRenderer>().material;
            material.SetColor("_EmissionColor", new Color(0, 0, 0));
        }
    }

    private void SetClosestObject()
    {
        if (interactableObjects.Count == 1)
        {
            ClosestInteractableObject = interactableObjects.FirstOrDefault();
        } else
        {
            ClosestInteractableObject = interactableObjects.OrderBy(item => Vector3.Distance(ConvertYPositionToZero(item.transform.position), ConvertYPositionToZero(transform.position)))
            .FirstOrDefault();
        }
    }

    private Vector3 ConvertYPositionToZero(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }

    // 요리가 다 되서 음식이 바뀌거나 하는 등의 경우에는 MissingObject 오류가 발생하게 된다 
    // 다른 함수, 객체에서 아래 함수를 호출하여 리스트에서 대상을 지우고 ClosestObject를 재설정해주어야 한다 
    public void RemoveObject(InteractableObject io)
    {
        interactableObjects.Remove(io);
        SetClosestObject();
    }
}