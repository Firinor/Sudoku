using System.Collections.Generic;
using System.Linq;
using Sudoku;
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
    
    public void OnEnable()
    {
        HardGeneration();
        GridToWhite();
        PrintView();
    }

    private void GridToWhite()
    {
        foreach (var tile in Tiles)
            tile.ResetTileColor();
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
        List<int> box4 = new List<int>();
        List<int> box5 = new List<int>();
        List<int> box6 = new List<int>();
        List<int> box7 = new List<int>();
        List<int> box8 = new List<int>();
        List<int> box9 = new List<int>();

        List<int> column1 = new List<int>();
        List<int> column2 = new List<int>();
        List<int> column3 = new List<int>();
        List<int> column4 = new List<int>();
        List<int> column5 = new List<int>();
        List<int> column6 = new List<int>();
        List<int> column7 = new List<int>();
        List<int> column8 = new List<int>();

        List<int> column9 = new List<int>();

        List<int> targetBox;
        List<int> column;

        HashSet<int> required = new();
        HashSet<int> blocked = new();
        HashSet<int> nominants = new();
        
        int rowMajorIndex;
        
        for (int i = 0; i < 72; i++) //We are sorting only 8 columns. The last column will be resolved automatically.
        {
#region box
            //column-major cicle
            //147
            //258
            //369
            //row-major cicle
            //123
            //456
            //789
            rowMajorIndex = (i % 9) * 9 + (i / 9);//From column-major to row-major
            
            int collumIndex = i / 9;
            column = collumIndex switch
            {
                0 => column1,
                1 => column2,
                2 => column3,
                3 => column4,
                4 => column5,
                5 => column6,
                6 => column7,
                7 => column8,
                _ => column9
            };
            
            targetBox = i switch
            {
                0 or 1 or 2 or
                9 or 10 or 11 or
                18 or 19 or 20 => box1,
                3 or 4 or 5 or
                12 or 13 or 14 or
                21 or 22 or 23 => box2,
                6 or 7 or 8 or
                15 or 16 or 17 or
                24 or 25 or 26 => box3,

                27 or 28 or 29 or
                36 or 37 or 38 or
                45 or 46 or 47 => box4,
                30 or 31 or 32 or
                39 or 40 or 41 or
                48 or 49 or 50 => box5,
                33 or 34 or 35 or
                42 or 43 or 44 or
                51 or 52 or 53 => box6,

                54 or 55 or 56 or
                63 or 64 or 65 or
                72 or 73 or 74 => box7,
                57 or 58 or 59 or
                66 or 67 or 68 or
                75 or 76 or 77 => box8,
                _ => box9
            };
#endregion
            switch (i)
            {
                case 12:
                    required = Block.Revert(box1).ToHashSet();
                    P_ColumnSorterer();
                    break;
                case 13:
                case 14:
                    P_ColumnSorterer();
                    break;
                case 32:
                    FindHiddenLocks_SetToBlockedHashSet_Index32();
                    J_ColumnSorterer();
                    break;
                case 33:
                    List<HashSet<int>> results = FindHiddenLocks_SetToReuireList_Index33_34_35();
                    ToNumber_ColumnSorterer(results[0]);
                    i++;
                    rowMajorIndex = (i % 9) * 9 + (i / 9);
                    ToNumber_ColumnSorterer(results[1]);
                    i++;
                    rowMajorIndex = (i % 9) * 9 + (i / 9);
                    ToNumber_ColumnSorterer(results[2]);
                    break;
                default:
                    DefaultColumnSorterer();
                    break;
            }
        }

        void ToNumber_ColumnSorterer(HashSet<int> target)
        {
            int number = TempField[rowMajorIndex];

            int rowIndexIterator = rowMajorIndex;
            int errorCooldown = rowIndexIterator % 9; //9 collums
            
            while (!target.Contains(number))
            {
                rowIndexIterator++;
                errorCooldown++;
                if (errorCooldown > 8) //out of index
                {
                    Debug.LogError("errorCooldown > 8");
                    break;
                }

                (TempField[rowMajorIndex], TempField[rowIndexIterator]) = (TempField[rowIndexIterator], TempField[rowMajorIndex]);
                number = TempField[rowMajorIndex];
            }

            targetBox.Add(number);
            column.Add(number);
        }
        void J_ColumnSorterer()
        {
            int number = TempField[rowMajorIndex];

            int rowIndexIterator = rowMajorIndex;
            int errorCooldown = rowIndexIterator % 9; //9 collums
            
            while (targetBox.Contains(number)
                   || column.Contains(number)
                   || blocked.Contains(number))
            {
                rowIndexIterator++;
                errorCooldown++;
                if (errorCooldown > 8) //out of index
                {
                    Debug.LogError("errorCooldown > 8");
                    break;
                }

                (TempField[rowMajorIndex], TempField[rowIndexIterator]) = (TempField[rowIndexIterator], TempField[rowMajorIndex]);
                number = TempField[rowMajorIndex];
            }

            targetBox.Add(number);
            column.Add(number);
            blocked.Remove(number);
        }
        
        void P_ColumnSorterer()
        {
            int boxIndex = rowMajorIndex / 9 % 3;
            int number = TempField[rowMajorIndex];

            int rowIndexIterator = rowMajorIndex;
            int errorCooldown = rowIndexIterator % 9; //9 collums

            foreach (var variable in targetBox)
            {
                required.Remove(variable);
            }

            bool onlyRequired = 3 - required.Count == boxIndex;

            while (targetBox.Contains(number)
                   || column.Contains(number)
                   || (onlyRequired && !required.Contains(number)))
            {
                rowIndexIterator++;
                errorCooldown++;
                if (errorCooldown > 8) //out of index
                {
                    Debug.LogError("errorCooldown > 8");
                    break;
                }

                (TempField[rowMajorIndex], TempField[rowIndexIterator]) = (TempField[rowIndexIterator], TempField[rowMajorIndex]);
                number = TempField[rowMajorIndex];
            }

            targetBox.Add(number);
            column.Add(number);
            required.Remove(number);
        }
        void DefaultColumnSorterer()
        {
            int number = TempField[rowMajorIndex];

            int rowIndexIterator = rowMajorIndex;
            int errorCooldown = rowIndexIterator % 9; //9 collums
            
            while (targetBox.Contains(number)
                   || column.Contains(number))
            {
                rowIndexIterator++;
                errorCooldown++;
                if (errorCooldown > 8) //out of index
                {
                    Debug.LogError("errorCooldown > 8");
                    break;
                }

                (TempField[rowMajorIndex], TempField[rowIndexIterator]) = (TempField[rowIndexIterator], TempField[rowMajorIndex]);
                number = TempField[rowMajorIndex];
            }

            targetBox.Add(number);
            column.Add(number);
        }
        
        void FindHiddenLocks_SetToBlockedHashSet_Index32()
        {
            blocked = column4.ToHashSet();
            blocked.Add(TempField[54]);
            blocked.Add(TempField[55]);
            blocked.Add(TempField[56]);
            int countLine = blocked.Count;
            if (countLine == 8)
            {
                blocked = Block.Revert(blocked);
                return;
            }
            blocked = column4.ToHashSet();
            blocked.Add(TempField[63]);
            blocked.Add(TempField[64]);
            blocked.Add(TempField[65]);
            countLine = blocked.Count;
            if (countLine == 8)
            {
                blocked = Block.Revert(blocked);
                return;
            }
            blocked = column4.ToHashSet();
            blocked.Add(TempField[72]);
            blocked.Add(TempField[73]);
            blocked.Add(TempField[74]);
            countLine = blocked.Count;
            if (countLine == 8)
            {
                blocked = Block.Revert(blocked);
                return;
            }
            blocked = new();
        }
        List<HashSet<int>> FindHiddenLocks_SetToReuireList_Index33_34_35()
        {
            List<HashSet<int>> result = new ();
            
            nominants = Block.Revert(column4.ToHashSet());
            result.Add(nominants);
            
            HashSet<int> checkLine = new HashSet<int>()
            {
                TempField[63],
                TempField[64],
                TempField[65]
            };
            checkLine.IntersectWith(nominants);
            if (checkLine.Count == 2)
            {
                HashSet<int> Line2 = new(nominants);
                Line2.ExceptWith(checkLine);
                result.Add(Line2);

                nominants.Remove(Line2.First());
                result.Add(nominants);
                
                return result;
            }
            
            checkLine = new HashSet<int>()
            {
                TempField[72],
                TempField[73],
                TempField[74]
            };
            
            checkLine.IntersectWith(nominants);
            if (checkLine.Count == 2)
            {
                HashSet<int> Line3 = new(nominants);
                Line3.ExceptWith(checkLine);
                
                nominants.Remove(Line3.First());
                result.Add(nominants);
                
                result.Add(Line3);

                return result;
            }

            result.Add(nominants);
            result.Add(nominants);
            
            return result;
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
