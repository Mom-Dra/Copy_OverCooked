using System.Collections;
using System.Collections.Generic;
using UnityEngine;

# region Comment
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
# endregion

public class Dish : Food
{
    [Header("Dish")]
    [SerializeField]
    private int maxIngredientCount = 4;

    [SerializeField]
    private bool stacking = false;

    [SerializeField]
    private List<EObjectSerialCode> dishIngredients;    

    // �ϼ� ���� ���� ���� 
    [SerializeField] 
    private List<PrefabToCombine> prefabToCombines;

    // ��Ʈ �迭�� ����Ͽ�, �������� On, Off ��Ű�� ������ ����� ���� ������ ��Ÿ��
    private Dictionary<int, PrefabToCombine> prefabDictionary = new Dictionary<int, PrefabToCombine>();

    private List<GameObject> activePrefabs = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        ingredients.Clear();
        Debug.Log($"{name} Clear : {ingredients.Count}");
    }

    public void Init() 
    {
        int bits = 0x00;

        foreach (PrefabToCombine prefab in prefabToCombines)
        {
            bits = SerialCodeToBit(prefab.ingredients);
            if(bits == -1)
            {
                throw new System.Exception($"{name}, ���� ���� �� �ش� Dish�� ��ῡ �ش����� ���� ���� �ֽ��ϴ�.");
            }
            prefabDictionary.Add(bits, prefab);
        }
    }

    private void SetActiveAll(bool active)
    {
        foreach(GameObject prefab in activePrefabs)
        {
            prefab.SetActive(active);
        }
    }

    private int SerialCodeToBitIndex(EObjectSerialCode serialCode)
    {
        for(int i=0; i< dishIngredients.Count; ++i)
        {
            if (dishIngredients[i] == serialCode)
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
            bits |= 1 << shift;
        }
        return bits;
    }

    public bool IsValidRecipe(List<EObjectSerialCode> ingredients)
    {
        return prefabDictionary.ContainsKey(SerialCodeToBit(ingredients));
        //int count = 0;
        //foreach(EObjectSerialCode serial in ingredients)
        //{
        //    foreach(EObjectSerialCode serial2 in dishIngredients)
        //    {
        //        if (serial == serial2)
        //        {
        //            ++count;
        //            break;
        //        }
        //    }
        //}
        //return count == ingredients.Count;
    }

    public bool IsValidIngredients(IFood iFood)
    {
        if(ingredients.Count + iFood.Ingredients.Count > maxIngredientCount)
        {
            return false;
        }
        if (stacking)
        {
            Debug.Log(prefabDictionary.ContainsKey(SerialCodeToBit(iFood.Ingredients)));
            return prefabDictionary.ContainsKey(SerialCodeToBit(iFood.Ingredients));
        }
        List<EObjectSerialCode> tmp = new List<EObjectSerialCode>();
        tmp.AddRange(iFood.Ingredients);
        tmp.AddRange(ingredients);
        return prefabDictionary.ContainsKey(SerialCodeToBit(tmp));
    }

    public void Combine(IFood iFood, bool destory = true)
    {
        ingredients.AddRange(iFood.Ingredients);
        foreach(EObjectSerialCode ic in ingredients)
        {
            Debug.Log($"Combine SC : {ic}");
        }

        //List<EObjectSerialCode> tmp = new List<EObjectSerialCode>();
        //tmp.AddRange(iFood.Ingredients);
        //tmp.AddRange(ingredients);

        SetActiveAll(false);
        int bit = SerialCodeToBit(ingredients);


        if (prefabDictionary.ContainsKey(bit))
        {
            prefabDictionary[bit].SetActive(true);
            activePrefabs.AddRange(prefabDictionary[bit].prefabs);
        }

        if(destory)
            Destroy(iFood.GameObject);
    }
}