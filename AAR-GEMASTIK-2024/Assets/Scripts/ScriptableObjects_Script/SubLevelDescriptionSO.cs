using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sub Level Name")]
public class SubLevelDescriptionSO : ScriptableObject
{
    [TextArea(1, 3)]
    public string subLevelName;
    [TextArea(10, 30)]
    public string description;
}
