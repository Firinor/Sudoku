using System;
using System.Collections.Generic;
using UnityEngine;

public class SudokuRules : MonoBehaviour
{
    public GameField GameField;
    
    public event Action OnTilesChanged;
    
    [ContextMenu(nameof(CheckWinCondition))]
    public void CheckWinCondition()
    {
        Debug.Log(IsSolved(GameField.Field, out var errors));

        foreach (var errorsIndex in errors)
        {
            GameField.Tiles[errorsIndex].ErrorColor();
        }
    }

    public static bool IsSolved(int[] field, out List<int> errorsIndexes)
    {
        errorsIndexes = new();

        bool result = true;
        
        for (int i = 0; i < 81; i++)
            if (field[i] == 0) 
                result = false;

        for (int i = 0; i < 9; i++)
        {
            if (!HasAllDigits(field, i * 9, 1, out int error))
            {
                if (error != 0)
                    errorsIndexes.Add(error);
                result = false;
            }
            
            if (!HasAllDigits(field, i, 9, out error))
            {
                if (error != 0)
                    errorsIndexes.Add(error);
                result = false;
            }
            
            int start = (i / 3) * 27 + (i % 3) * 3;
            if (!HasAllDigits(field, start, 9, out error, true))
            {
                if (error != 0)
                    errorsIndexes.Add(error);
                result = false;
            }
        }
    
        return result;
    }
    
    private static bool HasAllDigits(int[] field, int start, int step, out int error, bool container = false)
    {
        error = 0;
        bool[] seen = new bool[10];
        for (int j = 0; j < 9; j++)
        {
            int idx = start + j * step;
            if (container)
                idx = (j / 3) * 9 + (j % 3);
            int num = field[idx];
            if (num < 1 || num > 9 || seen[num])
            {
                error = idx;
                return false;
            }
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