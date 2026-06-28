using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "Configs/ColorData")]
public class ColorData : ScriptableObject
{
    public Color RootText;
    public Color PlayerText;
    public Color ErrorText;
    [Space]
    public Color DefaultTile;
    public Color PlayerSelectedTile;
    public Color NumberTile;
    public Color HelperTile;
    public Color ErrorTile;
}
