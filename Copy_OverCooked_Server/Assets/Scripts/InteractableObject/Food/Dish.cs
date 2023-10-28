using System.Collections;
using System.Collections.Generic;
using UnityEngine;

# region Comment
// 현재 문제점

// 1. 완성 음식들은 현재 Recipe 클래스로 모두 구현 가능하다.
// 하지만 그 중간 단계의 음식들은 아직 Prefab으로도, Recipe으로도 만들어 놓지 않았다.

// 예를 들면, 생선회 초밥은 김, 밥, 생선, 이 3가지 재료로 이루어진다.
// 완성된 생선회 초밥 Prefab을 하나 딱 만들면 끝이지만,
// 김+밥, 밥+생선, 김+생선 <= 이와 같은 중간 단계의 Prefab들을 표현하기 위해서는 각각을 또 만들어줘야 한다.

// 게다가 이번 게임에 구현될 음식들은 1개가 아니라 무수히 많다. (총 27개, 음식 종류는 10개)

// 이런 이유로 완성 요리의 중간 단계 Prefab을 일일히 다 만드는 것은 매우 비효율적이라고 생각된다.

// 2. 중간 단계의 음식 Prefab들은 각각 모양이 다르다. 그리고, 변하는 방식도 다르다.
// 위와 같이 생선회 초밥을 예로 들어보자.
// 접시에는 갓 지은 밥이 올려져 있다.
// 이 후에 추가로 어떤 음식이 올라오느냐에 따라 밥 Prefab의 모양은 바뀔 수도 있고, 바뀌지 않을 수도 있다.
// 만약 김이 들어왔다면, 김, 밥 모두 모양이 바뀐다.
// 만약 생선이 들어왔다면, 생선만 모양이 바뀌고, 밥은 바뀌지 않는다.

// 이런 씹

// 생각한 플랜 A
// 음식 종류별로 Prefab을 만든다. (샐러드류, 초밥류, 파스타류 등등...)
// 그리고 해당 Prefab 내에서 중간 단계의 재료들을 Active True False로 켰다 끄면서 모양을 표현한다.
// On, Off 두 가지 기능을 N개 만큼 해야되므로, 비트 배열을 사용한다.

// Dish 객체로 초밥을 만든다고 하면,
// 뭉쳐진 밥, 감싸진 김, 자른 생선, 자른 새우, 자른 오이, 이렇게 5개가 있다고 해보자.
// 총 5개의 GameObject이므로, 5비트로 모든 종류의 음식 모양을 표현할 수 있다.

// 만약 뭉쳐진 밥, 자른 생선이 재료로 들어와 있는 경우라면,
// 비트 배열은 10100 이 상태가 되고, Dish 객체 내에서는 이에 맞게 알맞은 GameObject를 On, Off 해줄 것이다.

// 초밥류 음식에서, 김과 밥은 필수 재료라고 볼 수 있다.
// 밥+생선, 밥+새우, 김+오이 <= 이렇게는 합칠 수 있지만,
// 오이+생선, 새우+생선 <= 이렇게는 합칠 수 없다.

// a
// 따라서 보유하고 있는 재료 중에 김 또는 밥이 있어야지만 합쳐질 수 있도록 미리 검사하여야 한다.

// b
// 또한 생선, 새우, 오이는 이 중 1개만 재료로 넣을 수 있으므로,
// 3개 중 하나라도 이미 재료에 있다면, 나머지 2개의 재료는 들어오지 못하도록 검사하여야 한다.

// + 사실 a와 b중 하나만 검사해도 될 것 같다. 


// 그렇다면, Plate, FoodTray 에서는 이를 어떻게 검사할까?
// 멤버 변수로 Dish 또는 이를 관리할 수 있는 중간 관리자 역할을 하는 객체를 두면 된다.

// 음식이 들어올 때 빈 접시라면, 그냥 넣는다 (기본적인 IsValid 검사는 하고 난 후)
// (FoodTray라면, 자신 또한 Food이므로 위의 경우는 없고, 무조건 레시피를 검사한다.)
// 그리고 들어온 음식에 포함되는 모든 Dish(=Recipe)들을 미리 뽑아온다. 
// 미리 추려놓으면 추후 들어오는 재료들을 검사할 때 빠르게 할 수 있기 때문 (이것은 일단 내 생각)


// 음식이 들어올 때, 현재 가지고 있는 재료들과 추려놓은 Dish들을 비교한다.
// 현재 재료들 중 임의의 Dish의 필수 재료가 포함되어 있다면,
// 해당 Dish를 가져오고, 더 이상 Dish를 추려내지 않는다.
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

    // 완성 음식 선택 가능 
    [SerializeField] 
    private List<PrefabToCombine> prefabToCombines;

    // 비트 배열을 사용하여, 프리팹을 On, Off 시키며 음식의 모습을 여러 가지로 나타냄
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
                throw new System.Exception($"{name}, 조합 음식 중 해당 Dish의 재료에 해당하지 않은 것이 있습니다.");
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