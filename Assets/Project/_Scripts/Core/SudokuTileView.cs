using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SudokuTileView : MonoBehaviour
{
    public bool IsPlayable;

    public TextMeshProUGUI Main;
    
    public TextMeshProUGUI mark1;
    public TextMeshProUGUI mark2;
    public TextMeshProUGUI mark3;
    public TextMeshProUGUI mark4;
    public TextMeshProUGUI mark5;
    public TextMeshProUGUI mark6;
    public TextMeshProUGUI mark7;
    public TextMeshProUGUI mark8;
    public TextMeshProUGUI mark9;

    public Image MainColor;

    public void ResetTileColor()
    {
        MainColor.color = Color.white;
    }

    public void ErrorColor()
    {
        MainColor.color = Color.paleVioletRed;
    }
}