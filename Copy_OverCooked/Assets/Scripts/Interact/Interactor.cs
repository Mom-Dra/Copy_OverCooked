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

    // �丮�� �� �Ǽ� ������ �ٲ�ų� �ϴ� ���� ��쿡�� MissingObject ������ �߻��ϰ� �ȴ� 
    // �ٸ� �Լ�, ��ü���� �Ʒ� �Լ��� ȣ���Ͽ� ����Ʈ���� ����� ����� ClosestObject�� �缳�����־�� �Ѵ� 
    public void RemoveObject(InteractableObject io)
    {
        interactableObjects.Remove(io);
        SetClosestObject();
    }
}