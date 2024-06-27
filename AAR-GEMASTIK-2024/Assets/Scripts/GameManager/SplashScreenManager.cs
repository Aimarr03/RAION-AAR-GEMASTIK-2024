using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenManager : MonoBehaviour
{
    [SerializeField] private RectTransform LogoGemastik;
    [SerializeField] private RectTransform LogoUB;
    [SerializeField] private TextMeshProUGUI RAION_SEA;
    [SerializeField] private TextMeshProUGUI Memperkenalkan;

    private void Start()
    {
        SplashScreenStart();
    }
    private async void SplashScreenStart()
    {
        await LogoGemastik.GetComponent<Image>().DOFade(1, 0.7f).AsyncWaitForCompletion();
        await Task.Delay(1000);
        await LogoGemastik.GetComponent<Image>().DOFade(0, 0.7f).AsyncWaitForCompletion();
        await Task.Delay(500);
        LogoUB.GetComponent<Image>().DOFade(1, 0.7f);
        Memperkenalkan.DOFade(1, 0.7f).SetDelay(0.2f);
        RAION_SEA.DOFade(1, 0.7f);
        await Task.Delay(1700);
        LogoUB.GetComponent<Image>().DOFade(0, 0.7f);
        Memperkenalkan.DOFade(0, 0.7f).SetDelay(0.2f);
        RAION_SEA.DOFade(0, 0.7f);
        await Task.Delay(1700);
        SceneManager.LoadScene(1);
    }
}
