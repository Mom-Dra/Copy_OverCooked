using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UI;

public class MissionManager : MonobehaviorSingleton<MissionManager>
{
    private static readonly float DURATION = 0.75f;

    [SerializeField]
    private float[] boardImageOffset = new float[5]
    {
        0f, 0f, 0f, 0f, 0f
    };

    [Header("Mission Manager")]
    [SerializeField]
    private float newMissionInterval = 5f;
    [SerializeField]
    private int maxMissionCount = 2;

    [SerializeField]
    private MissionBoard missionBoardImage;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private Vector2 boardInstantiatePos;

    private List<MissionBoard> missionBoardList = new List<MissionBoard>();
    private Mission[] missionList;

    private WaitForSeconds missionInterval;

    private int score = 0;


    protected override void Awake()
    {
        base.Awake();
        missionList = Resources.LoadAll<Mission>("Prefabs/Mission");
        missionInterval = new WaitForSeconds(newMissionInterval);
        scoreText.text = score.ToString();
    }

    private void Start()
    {
        StartCoroutine(MissionCoroutine());
    }

    private IEnumerator MissionCoroutine()
    {
        int randomIndex = -1;
        while (true)
        {
            if (missionBoardList.Count < maxMissionCount)
            {
                randomIndex = Random.Range(0, missionList.Length);
                AddMission(missionList[randomIndex]);
                yield return missionInterval;
            }
            yield return null;
        }
    }



    private MissionBoard CreateMissionBoard(Mission mission)
    {
        MissionBoard missionBoard = Instantiate(missionBoardImage, boardInstantiatePos , Quaternion.identity, GameObject.Find("Canvas").transform);
        missionBoard.Draw(mission);
        return missionBoard;
    }


    private void AddMission(Mission mission)
    {
        MissionBoard missionBoard = CreateMissionBoard(mission);
        missionBoardList.Add(missionBoard);

        SmoothMoveImage(missionBoardList.Count - 1);
    }

    public void Submit(List<EObjectSerialCode> ingredients)
    {
        foreach(MissionBoard mb in missionBoardList)
        {
            List<EObjectSerialCode> missionIngredients = mb.Mission.ingredients;
            if(ingredients.Count == missionIngredients.Count)
            {
                if (ingredients.OrderBy(e => e).SequenceEqual(missionIngredients.OrderBy(e => e)))
                {
                    StartCoroutine(OnMissionSuccess(mb));
                    return;
                }
            }
        }
        StartCoroutine(OnMissinoNotFounded());
    }

    private IEnumerator OnMissinoNotFounded()
    {
        foreach(MissionBoard mb in missionBoardList)
        {
            mb.color = Color.red;
        }

        yield return new WaitForSeconds(0.5f);

        foreach (MissionBoard mb in missionBoardList)
        {
            mb.color = Color.white;
        }
    }

    private IEnumerator OnMissionSuccess(MissionBoard missionBoard)
    {
        int removeIndex = missionBoardList.IndexOf(missionBoard);

        score += (removeIndex == 0) ? 100 : 50;
        scoreText.text = score.ToString();

        missionBoard.color = Color.green;

        yield return new WaitForSeconds(0.5f);

        missionBoardList.Remove(missionBoard);
        Destroy(missionBoard.gameObject);

        SmoothMoveImage(removeIndex);
    }

    private void SmoothMoveImage(int startIdx)
    {
        if(startIdx >= missionBoardList.Count)
        {
            return;
        }

        for(int i = startIdx; i < missionBoardList.Count; i++)
        {
            StartCoroutine(SmoothMoveCoroutine(missionBoardList[i], i));
        }
    }

    private IEnumerator SmoothMoveCoroutine(Image image, int offsetIndex)
    {
        RectTransform targetTransform = image.rectTransform;
        Vector3 startPos = targetTransform.anchoredPosition;
        Vector3 destPos = new Vector3(boardImageOffset[offsetIndex], 400f, 0f);

        float startTime = Time.time;

        while(Time.time < startTime + DURATION)
        {
            float  percentageComplete = (Time.time - startTime) / DURATION;
            targetTransform.anchoredPosition = Vector3.Lerp(startPos, destPos, percentageComplete);
            yield return null;
        }
        targetTransform.anchoredPosition = destPos;
    }

    
}