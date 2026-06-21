using System;
using UnityEngine;

public class SudokuRules : MonoBehaviour
{
    public GameField GameField;
    
    public event Action OnTilesChanged;
    
    [ContextMenu(nameof(CheckWinCondition))]
    public void CheckWinCondition()
    {
        Debug.Log(IsSolved(GameField.TempField));
    }

    public bool IsSolved(int[] field)
    {
        for (int i = 0; i < 81; i++)
            if (field[i] == 0) return false;
        
        for (int i = 0; i < 9; i++)
        {
            if (!HasAllDigits(field, i * 9, 1)) return false;
            if (!HasAllDigits(field, i, 9)) return false;
            if (!HasAllDigits(field, (i / 3) * 27 + (i % 3) * 3, 9, 3)) return false;
        }
    
        return true;
    }
    
    private bool HasAllDigits(int[] field, int start, int step, int rowLength = 9)
    {
        bool[] seen = new bool[10];
        for (int j = 0; j < 9; j++)
        {
            int idx = start + (j / rowLength) * 9 + (j % rowLength) * step;
            int num = field[idx];
            if (num < 1 || num > 9 || seen[num]) return false;
            seen[num] = true;
        }
        return true;
    }
    
    public void UnselectTile()
    {
    }

    public void Initialize(SaveData player)
    {
    }
}