using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ������

// 1. �ϼ� ���ĵ��� ���� Recipe Ŭ������ ��� ���� �����ϴ�.
// ������ �� �߰� �ܰ��� ���ĵ��� ���� Prefab���ε�, Recipe���ε� ����� ���� �ʾҴ�.

// ���� ���, ����ȸ �ʹ��� ��, ��, ����, �� 3���� ���� �̷������.
// �ϼ��� ����ȸ �ʹ� Prefab�� �ϳ� �� ����� ��������,
// ��+��, ��+����, ��+���� <= �̿� ���� �߰� �ܰ��� Prefab���� ǥ���ϱ� ���ؼ��� ������ �� �������� �Ѵ�.

// �Դٰ� �̹� ���ӿ� ������ ���ĵ��� 1���� �ƴ϶� ������ ����. (�� 27��, ���� ������ 10��)

// �̷� ������ �ϼ� �丮�� �߰� �ܰ� Prefab�� ������ �� ����� ���� �ſ� ��ȿ�����̶�� �����ȴ�.

// 2. �߰� �ܰ��� ���� Prefab���� ���� ����� �ٸ���. �׸���, ���ϴ� ��ĵ� �ٸ���.
// ���� ���� ����ȸ �ʹ��� ���� ����.
// ���ÿ��� �� ���� ���� �÷��� �ִ�.
// �� �Ŀ� �߰��� � ������ �ö�����Ŀ� ���� �� Prefab�� ����� �ٲ� ���� �ְ�, �ٲ��� ���� ���� �ִ�.
// ���� ���� ���Դٸ�, ��, �� ��� ����� �ٲ��.
// ���� ������ ���Դٸ�, ������ ����� �ٲ��, ���� �ٲ��� �ʴ´�.

// �̷� ��

// ������ �÷� A
// ���� �������� Prefab�� �����. (�������, �ʹ��, �Ľ�Ÿ�� ���...)
// �׸��� �ش� Prefab ������ �߰� �ܰ��� ������ Active True False�� �״� ���鼭 ����� ǥ���Ѵ�.
// On, Off �� ���� ����� N�� ��ŭ �ؾߵǹǷ�, ��Ʈ �迭�� ����Ѵ�.

// Dish ��ü�� �ʹ��� ����ٰ� �ϸ�,
// ������ ��, ������ ��, �ڸ� ����, �ڸ� ����, �ڸ� ����, �̷��� 5���� �ִٰ� �غ���.
// �� 5���� GameObject�̹Ƿ�, 5��Ʈ�� ��� ������ ���� ����� ǥ���� �� �ִ�.

// ���� ������ ��, �ڸ� ������ ���� ���� �ִ� �����,
// ��Ʈ �迭�� 10100 �� ���°� �ǰ�, Dish ��ü �������� �̿� �°� �˸��� GameObject�� On, Off ���� ���̴�.

// �ʹ�� ���Ŀ���, ��� ���� �ʼ� ����� �� �� �ִ�.
// ��+����, ��+����, ��+���� <= �̷��Դ� ��ĥ �� ������,
// ����+����, ����+���� <= �̷��Դ� ��ĥ �� ����.

// a
// ���� �����ϰ� �ִ� ��� �߿� �� �Ǵ� ���� �־������ ������ �� �ֵ��� �̸� �˻��Ͽ��� �Ѵ�.

// b
// ���� ����, ����, ���̴� �� �� 1���� ���� ���� �� �����Ƿ�,
// 3�� �� �ϳ��� �̹� ��ῡ �ִٸ�, ������ 2���� ���� ������ ���ϵ��� �˻��Ͽ��� �Ѵ�.

// + ��� a�� b�� �ϳ��� �˻��ص� �� �� ����. 


// �׷��ٸ�, Plate, FoodTray ������ �̸� ��� �˻��ұ�?
// ��� ������ Dish �Ǵ� �̸� ������ �� �ִ� �߰� ������ ������ �ϴ� ��ü�� �θ� �ȴ�.

// ������ ���� �� �� ���ö��, �׳� �ִ´� (�⺻���� IsValid �˻�� �ϰ� �� ��)
// (FoodTray���, �ڽ� ���� Food�̹Ƿ� ���� ���� ����, ������ �����Ǹ� �˻��Ѵ�.)
// �׸��� ���� ���Ŀ� ���ԵǴ� ��� Dish(=Recipe)���� �̸� �̾ƿ´�. 
// �̸� �߷������� ���� ������ ������ �˻��� �� ������ �� �� �ֱ� ���� (�̰��� �ϴ� �� ����)


// ������ ���� ��, ���� ������ �ִ� ����� �߷����� Dish���� ���Ѵ�.
// ���� ���� �� ������ Dish�� �ʼ� ��ᰡ ���ԵǾ� �ִٸ�,
// �ش� Dish�� ��������, �� �̻� Dish�� �߷����� �ʴ´�.


public class Dish : InteractableObject, IFood, IFoodUIAttachable
{
    [SerializeField]
    private List<EObjectSerialCode> ingredients;

    // �ϼ� ���� ���� ���� 
    [SerializeField] 
    private List<PrefabToCombine> prefabToCombines;

    // ��Ʈ �迭�� ����Ͽ�, �������� On, Off ��Ű�� ������ ����� ���� ������ ��Ÿ��
    private Dictionary<int, PrefabToCombine> prefabDictionary = new Dictionary<int, PrefabToCombine>();

    private List<EObjectSerialCode> currIngredients = new List<EObjectSerialCode>();

    private FoodUIComponent uIComponent;

    public bool IsCookable
    {
        get => true;
    }

    public EFoodState FoodState
    {
        get => EFoodState.Cooked;
    }

    public List<EObjectSerialCode> Ingredients
    {
        get => currIngredients;
    }

    public FoodUIComponent FoodUIComponent
    {
        get => uIComponent;
    }

    public int CurrCookingRate { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int CurrOverTime { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public GameObject GameObject => throw new System.NotImplementedException();

    protected override void Awake()
    {
        int bits = 0x00;

        foreach (PrefabToCombine prefab in prefabToCombines)
        {
            List<EObjectSerialCode> list = new List<EObjectSerialCode>();
            list.Add(prefab.mainIngredient);

            bits = SerialCodeToBit(prefab.subIngredients);
            prefabDictionary.Add(bits, prefab);
        }
    }

    private void SetActiveAll(bool active)
    {
        foreach(PrefabToCombine prefab in prefabToCombines)
        {
            prefab.SetActive(active);
        }
    }

    private int SerialCodeToBitIndex(EObjectSerialCode serialCode)
    {
        for(int i=0; i<ingredients.Count; ++i)
        {
            if (ingredients[i] == serialCode)
            {
                return i;
            }
        }
        return -1;
    }

    private int SerialCodeToBit(List<EObjectSerialCode> putIngredients)
    {
        int bits = 0x00;
        int shift = -1;
        foreach (EObjectSerialCode serial in putIngredients)
        {
            shift = SerialCodeToBitIndex(serial);
            if(shift == -1)
            {
                return -1;
            }
            bits = (bits | 1) << shift;
        }
        return bits;
    }

    public bool IsValidRecipe(List<EObjectSerialCode> ingredients)
    {
        return prefabDictionary.ContainsKey(SerialCodeToBit(ingredients));
    }

    public bool IsValidIngredients(IFood iFood)
    {
        List<EObjectSerialCode> tmp = new List<EObjectSerialCode>();
        tmp.AddRange(iFood.Ingredients);
        tmp.AddRange(currIngredients);
        return prefabDictionary.ContainsKey(SerialCodeToBit(tmp));
    }

    public void Combine(IFood iFood)
    {
        List<EObjectSerialCode> tmp = new List<EObjectSerialCode>();
        tmp.AddRange(iFood.Ingredients);
        tmp.AddRange(currIngredients);

        SetActiveAll(false);
        int bit = SerialCodeToBit(tmp);
        if (prefabDictionary.ContainsKey(bit))
        {
            prefabDictionary[bit].SetActive(true);
            currIngredients.AddRange(iFood.Ingredients);
        }
    }

    public void OnBurned()
    {
        
    }

    public void AddIngredientImages()
    {
        Debug.Log("���� ���� ���ߴٰ�");
    }
}