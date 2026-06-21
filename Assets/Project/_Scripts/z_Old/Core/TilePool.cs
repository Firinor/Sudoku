using System.Collections.Generic;
using UnityEngine;

public class TilePool : MonoBehaviour
{
    [SerializeField] private SudokuTileView prefab;
    
    public SudokuTileView Get()
    {
        SudokuTileView result = null;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            if(transform.GetChild(i).gameObject.activeSelf)
                continue;

            result = transform.GetChild(i).GetComponent<SudokuTileView>();
            break;
        }
        
        if (result is null)
            result = Instantiate(prefab, transform);
        
        //result.gameObject.SetActive(true);
        
        return result;
    }
    
    public void Release(SudokuTileView tile)
    {
        DestroyImmediate(tile.gameObject);
    }
    public void ClearAll(bool instant = false)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if(instant)
                DestroyImmediate(transform.GetChild(i).gameObject);
            else
                Destroy(transform.GetChild(i).gameObject);
        }
    }

    public List<SudokuTileView> GetAll(bool isAddUnplayable = false)
    {
        List<SudokuTileView> result = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            if(!isAddUnplayable 
               && !transform.GetChild(i).GetComponent<SudokuTileView>().IsPlayable)
                continue;
            
            if(!transform.GetChild(i).gameObject.activeSelf)
                continue;

            result.Add(transform.GetChild(i).GetComponent<SudokuTileView>());
        }
        return result;
    }
}