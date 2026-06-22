using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    public SudokuTileView[] Tiles;
    
    private readonly int[] GameFieldArray = new int[81]
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

    private void OnEnable()
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
        int[] collums = v ? new[]{0,2,1} : new[]{0,1,2};
        v = GameMath.HeadsOrTails();
        int[] rows = v ? new[]{0,2,1} : new[]{0,1,2};
        
        List<int> collumsContainer1 = new List<int>{ 0, 1, 2 };
        List<int> collumsContainer2 = new List<int>{ 0, 1, 2 };
        List<int> collumsContainer3 = new List<int>{ 0, 1, 2 };
        collumsContainer2.Shuffle();
        collumsContainer3.Shuffle();
        List<int> rowsContainer1 = new List<int>{ 0, 1, 2 };
        List<int> rowsContainer2 = new List<int>{ 0, 1, 2 };
        List<int> rowsContainer3 = new List<int>{ 0, 1, 2 };
        rowsContainer2.Shuffle();
        rowsContainer3.Shuffle();

        for(int i = 0; i < GameFieldArray.Length; i++)
        {
            int collum = i/3 % 3;
            int collumCorrection = collums[collum] == collum ? 0 :
                collums[collum] < collum ? -3 : +3;
            
            int row = i/27;
            int rowCorrection = rows[row] == row ? 0 :
                rows[row] < row ? -27 : +27;
            
            int collumContainer = i%3;
            int collumContainerCorrection = collum switch //-2 -1 0 +1 +2
            {
                0 => 0,
                1 => collumsContainer2[collumContainer] - collumContainer,
                2 => collumsContainer3[collumContainer] - collumContainer,
                _ => 0
            };
            
            int rowContainer = i/9 % 3;
            int rowContainerCorrection = row switch //-18 -9 0 +9 +18
            {
                0 => 0,
                1 => rowsContainer2[rowContainer] - rowContainer,
                2 => rowsContainer3[rowContainer] - rowContainer,
                _ => 0
            } * 9;

            int resultIndex = i + collumCorrection + rowCorrection + collumContainerCorrection + rowContainerCorrection;
            int result = GameFieldArray[resultIndex];
            TempField[i] = mask[result];
        }
    }

    private void PrintView()
    {
        for (int i = 0; i < TempField.Length; i++)
        {
            Tiles[i].Main.text = TempField[i].ToString();
        }
    }
}
