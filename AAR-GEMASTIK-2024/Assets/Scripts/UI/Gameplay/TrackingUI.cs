using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrackingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI depthText;
    [SerializeField] private TextMeshProUGUI horizontalText;
    [SerializeField] private Transform anchor;
    [SerializeField] private PlayerCoreSystem playerCoreSystem;
    string depthFormat = "Kedalaman:\n";
    string horizontalFormat = "Jarak Horizontal:\n";


    // Update is called once per frame
    void Update()
    {
        float distanceX = (anchor.position.x - playerCoreSystem.transform.position.x)/10;
        float distanceY = (anchor.position.y - playerCoreSystem.transform.position.y)/7.5f;
        depthText.text = $"{depthFormat}{distanceY.ToString("0.0")}m";
        horizontalText.text = $"{horizontalFormat}{distanceX.ToString("0.0")}m";
    }
}
