using UnityEngine;

public class CoreData : MonoBehaviour
{
    public static ColorData Colors;
    [SerializeField] private ColorData _colorData;
    
    private void Awake()
    {
        Colors = _colorData;
    }
}