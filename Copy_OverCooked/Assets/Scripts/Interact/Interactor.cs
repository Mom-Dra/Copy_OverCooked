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
        Debug.Log($"Enter: {other.name}");

        InteractableObject interactableObject = other.GetComponent<InteractableObject>();
        if (interactableObject != null && interactableObject.IsInteractable)
        {
            interactableObjects.Add(interactableObject);
            Debug.Log($"Enter: {interactableObject}");
            SetClosestObject();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObject io = other.GetComponent<InteractableObject>();
        interactableObjects.Remove(io);
        Debug.Log($"Exit: {other.name}");
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

    // �丮�� �� �Ǽ� ������ �ٲ�ų� �ϴ� ���� ��쿡�� MissingObject ������ �߻��ϰ� �ȴ� 
    // �ٸ� �Լ�, ��ü���� �Ʒ� �Լ��� ȣ���Ͽ� ����Ʈ���� ����� ����� ClosestObject�� �缳�����־�� �Ѵ� 
    // ���� �ƴ� (����� �� ������)
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