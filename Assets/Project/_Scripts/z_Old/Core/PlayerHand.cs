using System;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField]
    private PlayerInputHolder playerInput;

    public Action<SudokuTileView> OnTileClick;

    private void Start()
    {
        playerInput.onClick += FindTile;
    }

    private void FindTile(Vector2 position)
    {
        SudokuTileView tile = GetRayHitTile(position);
        
        if(tile == null)
            return;
        
        OnTileClick?.Invoke(tile);
    }

    private static SudokuTileView GetRayHitTile(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);

        SudokuTileView result = null;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.TryGetComponent(out result);
        }
        return result;
    }

    private void OnDestroy()
    {
        playerInput.onClick -= FindTile;
    }
}
