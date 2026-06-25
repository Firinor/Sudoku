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
        HardGeneration();
        PrintView();
    }

    private void EasyGeneration()
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

    private void HardGeneration()
    {
        List<int> arr = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        TempField = new int[81];

        for (int i = 0; i < 81; i++)
        {
            if (i % 9 == 0)
                arr.Shuffle();

            TempField[i] = arr[i % 9];
        }
        
        List<int> box1 = new List<int>();
        List<int> box2 = new List<int>();
        List<int> box3 = new List<int>();
        for (int i = 0; i < 9; i++)
        {
            if (i % 3 == 0)
            {
                box1 = new List<int>();
                box2 = new List<int>();
                box3 = new List<int>();
            }
            
            List<int> collum = new List<int>();
            
            for (int j = 0; j < 9; j++)
            {
                int localIndex = i+j*9;
                int number = TempField[localIndex];

                List<int> targetBox = j switch
                {
                    <3 => box1,//123
                    >5 => box3,//789
                    _ => box2//345
                };

                int localIndexIterator = localIndex;
                int errorCooldown = localIndexIterator % 9;
                while (
                    (targetBox.Contains(number) || collum.Contains(number))
                       && errorCooldown < 8)
                {
                    localIndexIterator++;
                    errorCooldown++;
                    (TempField[localIndex], TempField[localIndexIterator]) = (TempField[localIndexIterator], TempField[localIndex]);
                    number = TempField[localIndex];
                }
                
                targetBox.Add(number);
                collum.Add(number);
            }
        }
    }

    private void f()
    {
        bool[] sorted = new bool[81];

        for (int i = 0; i < 9; i++)
        {
            bool backtrack = false;
            
            for (int a = 0; a < 2; a++)
            {
                bool[] registered = new bool[10];
                int rowOrigin = i * 9;
                int colOrigin = i;
                
                for (int j = 0; j < 9; j++)
                {
                    int step = (a % 2 == 0) ? rowOrigin + j : colOrigin + j * 9;
                    int num = TempField[step];

                    if (!registered[num]) 
                        registered[num] = true;
                    else
                    {
                        //BAS (Box and Adjacent-cell Swap)
                        for (int y = j; y >= 0; y--)
                        {
                            int scan = (a % 2 == 0) ? i * 9 + y : i + 9 * y;
                            if (TempField[scan] == num)
                            {
                                for (int z = (a % 2 == 0) ? (i % 3 + 1) * 3 : 0; z < 9; z++)
                                {
                                    if (a % 2 == 1 && z % 3 <= i % 3)
                                        continue;

                                    int boxOrigin = ((scan % 9) / 3) * 3 + (scan / 27) * 27;
                                    int boxStep = boxOrigin + (z / 3) * 9 + (z % 3);
                                    int boxNum = TempField[boxStep];

                                    if ((!sorted[scan] && !sorted[boxStep] && !registered[boxNum])
                                        || (sorted[scan] && !registered[boxNum] && (a % 2 == 0 ? boxStep % 9 == scan % 9 : boxStep / 9 == scan / 9)))
                                    {
                                        TempField[scan] = boxNum;
                                        TempField[boxStep] = num;
                                        registered[boxNum] = true;
                                    }
                                    else if (z == 8)
                                    {
                                        // PAS (Preferred Adjacent Swap)
                                        int searchingNo = num;
                                        bool[] blindSwapIndex = new bool[81];

                                        for (int q = 0; q < 18; q++)
                                        {
                                            for (int b = 0; b <= j; b++)
                                            {
                                                int pacing = (a % 2 == 0) ? rowOrigin + b : colOrigin + b * 9;
                                                if (TempField[pacing] == searchingNo)
                                                {
                                                    int adjacentCell = -1;
                                                    int adjacentNo = -1;
                                                    int decrement = (a % 2 == 0) ? 9 : 1;

                                                    for (int c = 1; c < 3 - (i % 3); c++)
                                                    {
                                                        adjacentCell = pacing + (a % 2 == 0 ? (c + 1) * 9 : c + 1);

                                                        if ((a % 2 == 0 && adjacentCell >= 81)
                                                            || (a % 2 == 1 && adjacentCell % 9 == 0))
                                                            adjacentCell -= decrement;
                                                        else
                                                        {
                                                            adjacentNo = TempField[adjacentCell];
                                                            if (i % 3 != 0
                                                                || c != 1
                                                                || blindSwapIndex[adjacentCell]
                                                                || registered[adjacentNo])
                                                                adjacentCell -= decrement;
                                                        }
                                                        adjacentNo = TempField[adjacentCell];

                                                        if (!blindSwapIndex[adjacentCell])
                                                        {
                                                            blindSwapIndex[adjacentCell] = true;
                                                            TempField[pacing] = adjacentNo;
                                                            TempField[adjacentCell] = searchingNo;
                                                            searchingNo = adjacentNo;

                                                            if (!registered[adjacentNo])
                                                            {
                                                                registered[adjacentNo] = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        //ABS (Advance and Backtrack Sort)
                                        backtrack = true;
                                    }
                                }
                            }
                        }
                    }
                }

                if (a % 2 == 0)
                {
                    for (int j = 0; j < 9; j++) 
                        sorted[i * 9 + j] = true;
                }
                else if (!backtrack)
                {
                    for (int j = 0; j < 9; j++) 
                        sorted[i + j * 9] = true;
                }
                else
                {
                    backtrack = false;
                    for (int j = 0; j < 9; j++) 
                        sorted[i * 9 + j] = false;
                    for (int j = 0; j < 9; j++) 
                        sorted[(i - 1) * 9 + j] = false;
                    for (int j = 0; j < 9; j++) 
                        sorted[i - 1 + j * 9] = false;
                    i -= 2;
                }
            }
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
