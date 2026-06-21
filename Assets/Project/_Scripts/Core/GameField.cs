using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    public SudokuTileView[] Tiles;
    
    public readonly int[] GameFieldArray = new int[81]
    {
        1,2,3,  4,5,6,  7,8,9,
        4,5,6,  7,8,9,  1,2,3,
        7,8,9,  1,2,3,  4,5,6,
        
        2,3,4,  5,6,7,  8,9,1,
        5,6,7,  8,9,1,  2,3,4,
        8,9,1,  2,3,4,  5,6,7,
        
        3,4,5,  6,7,8,  9,1,2,
        6,7,8,  9,1,2,  3,4,5,
        9,1,2,  3,4,5,  6,7,8
    };
    
    public int[] TempField = new int[81]
    {
        1,2,3,  4,5,6,  7,8,9,
        4,5,6,  7,8,9,  1,2,3,
        7,8,9,  1,2,3,  4,5,6,
        
        2,3,4,  5,6,7,  8,9,1,
        5,6,7,  8,9,1,  2,3,4,
        8,9,1,  2,3,4,  5,6,7,
        
        3,4,5,  6,7,8,  9,1,2,
        6,7,8,  9,1,2,  3,4,5,
        9,1,2,  3,4,5,  6,7,8
    };

    private void Start()
    {
        RandomShuffle();
        PrintView();
    }

    private void RandomShuffle()
    {
        TempField = (int[])GameFieldArray.Clone();
        
        Dictionary<int, int> mask = new();
        List<int> randomNumbers = new List<int>{1,2,3,4,5,6,7,8,9};
        randomNumbers.Shuffle();
        for (int i = 0; i < randomNumbers.Count; i++)
        {
            mask.Add(i+1, randomNumbers[i]);
        }

        bool v = GameMath.HeadsOrTails();
        int[] collums = v ? new[]{1,3,2} : new[]{1,2,3};
        v = GameMath.HeadsOrTails();
        int[] rows = v ? new[]{1,3,2} : new[]{1,2,3};
        
        List<int> collumsContainer2 = new List<int>{ 1, 2, 3 };
        collumsContainer2.Shuffle();
        List<int> collumsContainer3 = new List<int>{ 1, 2, 3 };
        collumsContainer3.Shuffle();
        List<int> rowsContainer2 = new List<int>{ 1, 2, 3 };
        rowsContainer2.Shuffle();
        List<int> rowsContainer3 = new List<int>{ 1, 2, 3 };
        rowsContainer3.Shuffle();

        for(int i = 0; i < GameFieldArray.Length; i++)
        {
            int collum = i/3 % 3;
            int collumCorrection = collums[collum] == collum ? 0 :
                collums[collum] < collum ? -3 : +3;
            
            int row = i/27;
            int rowCorrection = rows[row] == row ? 0 :
                rows[row] < row ? -27 : +27;
            
            int collumContainer = i%9;
            
            int collumContainerCorrection;
            int rowContainerCorrection;
            
            
        }
    }

    private void PrintView()
    {
        for (int i = 0; i < GameFieldArray.Length; i++)
        {
            Tiles[i].Main.text = GameFieldArray[i].ToString();
        }
    }
}
