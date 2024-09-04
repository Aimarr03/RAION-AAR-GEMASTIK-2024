using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using System;

public class ScoringUI : MonoBehaviour, IDataPersistance
{
    [SerializeField] private Transform Container;
    [SerializeField] private TextMeshProUGUI trashCollectedTextUI;
    [SerializeField] private TextMeshProUGUI mutatedSharkCollectedTextUI;
    [SerializeField] private TextMeshProUGUI helpedFishCollectedTextUI;
    [SerializeField] private TextMeshProUGUI coinObtainedTextUI;
    [SerializeField] private TextMeshProUGUI NumericProgressBar;
    [SerializeField] private Image ProgressBar;
    private List<TextMeshProUGUI> textUIList;

    [Header("Progress Ocean Color")]
    [SerializeField] private Color ForegroundOldColor = new Color(139, 190, 209, 47);
    [SerializeField] private Color ForegroundNewColor = new Color(92,136,255, 47);
    [SerializeField] private Color BackgroundOldColor = new Color(51, 103, 60, 254); 
    [SerializeField] private Color BackgroundNewColor = new Color(0, 110, 171, 254);
    [Header("Foreground-Background")]
    [SerializeField] private SpriteRenderer foreground;
    [SerializeField] private SpriteRenderer background;

    [Header("Button"), SerializeField]
    private Button EndExpedictionButton;
    private int moneyObtainedFromEnemyDefeated = 0;
    private int moneyObtainedFromHelpingFish = 0;
    private float maxDuration = 2;
    private float currentWeightTrashCollected;
    
    private int currrentTrashTotalCollected = 0;
    private int trashHasBeenCollectedPreviously = 0;
    private int trashCount = 0;
    
    private int currentSharkTotalCollected = 0;
    private int sharkHasBeenCollectedPreviously = 0;
    private int sharkCount = 0;

    private int currentFishNeedHelpTotalCollected = 0;
    private int fishHasBeenCollectedPreviously = 0;
    private int fishCount = 0;
    
    private int totalCount = 0;
    private int totalCountCollected = 0;

    private float trashProgress = 0f;
    private float sharkProgress = 0f;
    private float fishProgress = 0f;
    private float totalProgress = 0f;

    private DefaultInputAction inputAction;
    private void Awake()
    {
        currentWeightTrashCollected = 0f;
        textUIList = new List<TextMeshProUGUI>
        {
            trashCollectedTextUI, mutatedSharkCollectedTextUI, coinObtainedTextUI, helpedFishCollectedTextUI
        };
        foreach (TextMeshProUGUI text in textUIList)
        {
            text.text = "";
        }
        Container.GetComponent<RectTransform>().DOAnchorPosY(1000, 0f);
        Container.gameObject.SetActive(false);
        foreground.color = ForegroundOldColor;
        background.color = BackgroundOldColor;
        
    }
    private void Start()
    {
        OnCalculateData();
        ExpedictionManager.Instance.OnDoneExpediction += Instance_OnDoneExpediction;
        ExpedictionManager.Instance.OnCollectedTrash += Instance_OnCollectedTrash;
        ExpedictionManager.Instance.OnCaughtFishDelivarable += Instance_OnCaughtFishDelivarable;
        FishBaseNeedHelp.OnBroadcastGettingHelp += FishBaseNeedHelp_OnBroadcastGettingHelp;
        TrashBase.OnCollectedEvent += TrashBase_OnCollectedEvent;
        EnemyBase.OnCaught += EnemyBase_OnCaught;
    }
    private void OnDisable()
    {
        ExpedictionManager.Instance.OnDoneExpediction -= Instance_OnDoneExpediction;
        ExpedictionManager.Instance.OnCollectedTrash -= Instance_OnCollectedTrash;
        ExpedictionManager.Instance.OnCaughtFishDelivarable -= Instance_OnCaughtFishDelivarable;
        FishBaseNeedHelp.OnBroadcastGettingHelp -= FishBaseNeedHelp_OnBroadcastGettingHelp;
        TrashBase.OnCollectedEvent -= TrashBase_OnCollectedEvent;
        EnemyBase.OnCaught -= EnemyBase_OnCaught;
    }

    

    private void EnemyBase_OnCaught(int bounty)
    {
        currentSharkTotalCollected++;
        totalCountCollected++;
        moneyObtainedFromEnemyDefeated += bounty;
    }

    private void TrashBase_OnCollectedEvent()
    {
        currrentTrashTotalCollected++;
        totalCountCollected++;
        OnCalculateBackgroundColorTransition();
    }
    private void Instance_OnCollectedTrash(float weightTrashCollected)
    {
        currentWeightTrashCollected += weightTrashCollected;
    }
    private void FishBaseNeedHelp_OnBroadcastGettingHelp(int bounty)
    {
        totalCountCollected++;
        currentFishNeedHelpTotalCollected++;
        moneyObtainedFromEnemyDefeated += bounty;
    }
    private void Instance_OnCaughtFishDelivarable(IDelivarable fishDelivarable)
    {
        fishDelivarable.OnDelivered();
        switch (fishDelivarable.GetDelivarableType())
        {
            case DelivarableType.Mutated:
                currentSharkTotalCollected++;
                totalCountCollected++;
                moneyObtainedFromEnemyDefeated += fishDelivarable.GetBounty();
                break;
            case DelivarableType.Poisoned: 
                
                break;
        }
        
    }
    private void Instance_OnDoneExpediction(bool obj, PlayerCoreSystem coreSystem)
    {
        Debug.Log("Invoke when Done Expediction");
        Container.gameObject.SetActive(true);
        StartCoroutine(OnStartDisplayScore(coreSystem));
        inputAction = new DefaultInputAction();
        inputAction.ExpedictionUI.Enable();
        inputAction.ExpedictionUI.Confirm.performed += Confirm_performed;
        ExpedictionManager.Instance.OnDoneExpediction -= Instance_OnDoneExpediction;
    }

    private void Confirm_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        EndExpedictionButton.onClick.Invoke();
        inputAction.ExpedictionUI.Confirm.performed -= Confirm_performed;
    }

    private void OnCalculateBackgroundColorTransition()
    {
        float totalProgress = (currentWeightTrashCollected + trashHasBeenCollectedPreviously) / trashCount;
        foreground.color = Color.Lerp(ForegroundOldColor, ForegroundNewColor, totalProgress);
        background.color = Color.Lerp(BackgroundOldColor, BackgroundNewColor, totalProgress);
    }
    private IEnumerator OnStartDisplayScore(PlayerCoreSystem coreSystem)
    {
        Container.GetComponent<RectTransform>().DOAnchorPosY(0, 0.4f).SetEase(Ease.OutBack);
        
        yield return new WaitForSeconds(0.57f);
        yield return OnIncrementValueUI(trashCollectedTextUI, currrentTrashTotalCollected);
        yield return OnIncrementValueUI(mutatedSharkCollectedTextUI, currentSharkTotalCollected);
        yield return OnIncrementValueUI(helpedFishCollectedTextUI, currentFishNeedHelpTotalCollected);

        int totalMoneyObtained = EconomyManager.Instance.GetMoneyMultiplierBasedOnTrash(currentWeightTrashCollected);
        totalMoneyObtained += moneyObtainedFromEnemyDefeated;
        totalMoneyObtained += moneyObtainedFromHelpingFish;
        
        yield return OnIncrementValueUI(coinObtainedTextUI, totalMoneyObtained);
        EconomyManager.Instance.OnGainMoney(totalMoneyObtained);

        float totalProgress = ((float)totalCountCollected) / totalCount;
        float trashProgress = ((float)(currrentTrashTotalCollected + trashHasBeenCollectedPreviously)) / trashCount;
        float sharkProgress = ((float)(currentSharkTotalCollected + sharkHasBeenCollectedPreviously)) / sharkCount;
        float fishProgress = ((float)(currentFishNeedHelpTotalCollected + fishHasBeenCollectedPreviously)) / fishCount;
        Debug.Log(totalProgress);
        Debug.Log(trashProgress);
        Debug.Log(sharkProgress);
        Debug.Log(fishProgress);
        /*totalProgress = (float)Math.Round(totalProgress, 2);
        trashProgress = (float)Math.Round(trashProgress, 2);
        sharkProgress = (float)Math.Round(sharkProgress, 2);
        fishProgress = (float)Math.Round(fishProgress, 2);*/

        this.totalProgress = totalProgress;
        this.trashProgress = trashProgress;
        this.sharkProgress = sharkProgress;
        this.fishProgress = fishProgress;

        yield return OnIncrementValueUI(NumericProgressBar, ProgressBar, this.totalProgress);
    }
    private IEnumerator OnIncrementValueUI(TextMeshProUGUI text, Image targetUI, float targetValue)
    {
        int startValue = 0;
        float currentDuration = 0;
        while (currentDuration < maxDuration)
        {
            currentDuration += Time.deltaTime;
            float t = Mathf.Clamp01(currentDuration / maxDuration);
            float currentValue = Mathf.Lerp(startValue, targetValue, t);
            targetUI.fillAmount = currentValue;
            text.text = (currentValue*100).ToString("0.00") + " %";
            yield return null;
        }
        text.text = (targetValue*100).ToString("0.00") + " %";
    }
    private IEnumerator OnIncrementValueUI(TextMeshProUGUI targetUI, float targetValue)
    {
        int startValue = 0;
        float currentDuration = 0;
        while(currentDuration < maxDuration)
        {
            currentDuration += Time.deltaTime;
            float t = Mathf.Clamp01(currentDuration / maxDuration);
            float currentValue = Mathf.Lerp(startValue, targetValue, t);
            targetUI.text = currentValue.ToString("0.0");
            yield return null;
        }
        targetUI.text = targetValue.ToString("0");
    }
    public void OnLoadShopping()
    {
        DataManager.instance.SaveGame();
        GameManager.Instance.LoadScene("ShoppingMenu");
    }
    private void OnCalculateData()
    {
        IEnumerable<TrashBase> trashCalculatedFromScene = FindObjectsOfType<MonoBehaviour>().OfType<TrashBase>();
        trashHasBeenCollectedPreviously = 0;
        foreach (TrashBase trash in trashCalculatedFromScene)
        {
            if(trash.HasBeenCollected()) trashHasBeenCollectedPreviously++;
        }
        trashCount = trashCalculatedFromScene.Count();

        IEnumerable<SharkBase> SharkCalculatedFromScene = FindObjectsOfType<MonoBehaviour>().OfType<SharkBase>();
        sharkHasBeenCollectedPreviously = 0;
        foreach(SharkBase shark in SharkCalculatedFromScene)
        {
            if(shark.HasBeenDelivered()) sharkHasBeenCollectedPreviously++;
        }
        sharkCount = SharkCalculatedFromScene.Count();
        
        IEnumerable<FishBaseNeedHelp> FishNeedHelpList = FindObjectsOfType<MonoBehaviour>().OfType<FishBaseNeedHelp>();
        fishHasBeenCollectedPreviously = 0;
        foreach (FishBaseNeedHelp fish in FishNeedHelpList)
        {
            if (fish.HasBeenHelped()) fishHasBeenCollectedPreviously++;
        }
        fishCount = FishNeedHelpList.Count();

        totalCount = sharkCount + trashCount + fishCount;
        totalCountCollected = trashHasBeenCollectedPreviously + sharkHasBeenCollectedPreviously+ fishHasBeenCollectedPreviously;
    }

    public void LoadScene(GameData gameData)
    {
        if (gameData.tutorialGameplay) return;
        LevelData levelData = gameData.GetLevelData(GameManager.Instance.level);
        float trashProgress = levelData.trashProgress;
        foreground.color = Color.Lerp(ForegroundOldColor, ForegroundNewColor, trashProgress);
    }
    
    public void SaveScene(ref GameData gameData)
    {
        LevelData levelData = gameData.GetLevelData(GameManager.Instance.level);
        levelData.sharkMutatedCountDone = sharkHasBeenCollectedPreviously + currentSharkTotalCollected;
        levelData.sharkMutatedProgress = sharkProgress;
        levelData.trashCountDone = trashHasBeenCollectedPreviously + currrentTrashTotalCollected;
        levelData.trashProgress = trashProgress;
        levelData.fishNeededHelpCountDone = fishHasBeenCollectedPreviously + currentFishNeedHelpTotalCollected;
        levelData.fishNeededHelpProgress = fishProgress;
        levelData.progress = totalProgress;
        gameData.money = EconomyManager.Instance.currentMoney;
        if(!levelData.hasBeenExpediction) levelData.hasBeenExpediction = true;
    }
    
}
