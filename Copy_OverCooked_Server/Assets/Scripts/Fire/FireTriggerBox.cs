using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FireTriggerBox : SerializedObject
{
    [Header("Fire")]
    [SerializeField]
    private float burnAgainTime = 1.5f;
    [SerializeField]
    private Vector3 uIOffset = Vector3.up;

    private GameObject firePrefab;

    private Image progressBar;
    private Image gauge;

    private static readonly WaitForSeconds workInterval = new WaitForSeconds(0.1f);
    private static readonly WaitForSeconds heatInterval = new WaitForSeconds(2f);
    private static readonly float waitForDisappearImage = 0.5f;

    private float extinguishRate = 1f;
    private float overheatRate = 0f;

    private bool onFire = false;

    private bool reset = false;

    public bool OnFire
    {
        get => onFire;
    }

    protected override void Awake()
    {
        GameObject _prefab = SerialCodeDictionary.Instance.FindBySerialCode(EObjectSerialCode.Fire);
        firePrefab = Instantiate(_prefab, transform.position + Vector3.down, _prefab.transform.rotation, transform);

        progressBar = SerialCodeDictionary.Instance.InstantiateBySerialCode<Image>(EObjectSerialCode.Img_Progress);
        progressBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + uIOffset);
        gauge = progressBar.transform.GetChild(1).GetComponent<Image>();
        gauge.fillAmount = extinguishRate;
        progressBar.gameObject.SetActive(false);
    }

    protected override void Start()
    {

    }

    private IEnumerator WaitForBurnCoroutine()
    {
        float time = 0f;
        while(time < burnAgainTime)
        {
            if (reset)
            {
                reset = false;
                time = 0f;
            }
            time += 0.1f;
            yield return workInterval;
        }

        StartCoroutine(HeatUpToMaxCoroutine());
    }

    private IEnumerator HeatUpToMaxCoroutine()
    {
        while(extinguishRate < 1f)
        {
            if(reset)
               yield break;
            
            extinguishRate += 0.05f;
            gauge.fillAmount = extinguishRate;
            yield return workInterval;
        }
        extinguishRate = 1f;

        StartCoroutine(WaitForDisappearImageCoroutine());
    }

    private IEnumerator WaitForDisappearImageCoroutine()
    {
        float time = 0f;
        while(time < waitForDisappearImage)
        {
            if (reset)
                yield break;
            time += 0.1f;
            yield return workInterval;
        }
        progressBar.gameObject.SetActive(false);
    }

    private IEnumerator MoveFireCoroutine()
    {
        while(true)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 0.5f);
            foreach(Collider hit in hits)
            {
                if(hit.TryGetComponent<FireTriggerBox>(out FireTriggerBox fireTriggerBox))
                {
                    if (!fireTriggerBox.OnFire)
                    {
                        fireTriggerBox.HeatUp();
                    }
                }
            }
            yield return heatInterval;
        }
    }

    public void HeatUp()
    {
        overheatRate += 1f;
        if (overheatRate >= 5f)
        {
            Ignite();
        }
    }

    public void HeatDown()
    {
        if (!progressBar.gameObject.activeSelf)
        {
            progressBar.gameObject.SetActive(true);
            StartCoroutine(WaitForBurnCoroutine());
        }
        extinguishRate -= 0.002f;
        gauge.fillAmount = extinguishRate;
        reset = true;
        if (extinguishRate <= 0f)
        {
            Extinguish();
        }
    }

    public void Ignite()
    {
        overheatRate = 0f;
        extinguishRate = 1f;
        firePrefab.gameObject.SetActive(true);
        onFire = true;
        StartCoroutine(MoveFireCoroutine());
    }

    private void Extinguish()
    {
        overheatRate = 0f;
        StopAllCoroutines();
        if (progressBar.gameObject.activeSelf)
        {
            progressBar.gameObject.SetActive(false);
        }
        firePrefab.gameObject.SetActive(false);
        onFire = false;
    }

    
}