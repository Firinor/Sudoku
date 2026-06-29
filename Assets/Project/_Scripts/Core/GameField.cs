using System.Collections.Generic;
using System.Linq;
using Sudoku;
using UnityEngine;

public class GameField : MonoBehaviour
{
    public SudokuTileView[] Tiles;
    
    public readonly int[] Field = new int[81]
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
        int[] TempField = (int[])Field.Clone();
        
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

        for(int i = 0; i < Field.Length; i++)
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
            int result = Field[resultIndex];
            TempField[i] = mask[result];
        }
    }

    private void HardGeneration()
    {
        HashSet<int> box1 = new ();
        HashSet<int> box2 = new ();
        HashSet<int> box3 = new ();
        HashSet<int> box4 = new ();
        HashSet<int> box5 = new ();
        HashSet<int> box6 = new ();
        HashSet<int> box7 = new ();
        HashSet<int> box8 = new ();
        HashSet<int> box9 = new ();
        
        HashSet<int> column1 = new ();
        HashSet<int> column2 = new ();
        HashSet<int> column3 = new ();
        HashSet<int> column4 = new ();
        HashSet<int> column5 = new ();
        HashSet<int> column6 = new ();
        HashSet<int> column7 = new ();
        HashSet<int> column8 = new ();
        HashSet<int> column9 = new ();
    
        HashSet<int> row1 = new ();
        HashSet<int> row2 = new ();
        HashSet<int> row3 = new ();
        HashSet<int> row4 = new ();
        HashSet<int> row5 = new ();
        HashSet<int> row6 = new ();
        HashSet<int> row7 = new ();
        HashSet<int> row8 = new ();
        HashSet<int> row9 = new ();
        
        HashSet<int> targetBox;
        HashSet<int> column;
        HashSet<int> row;

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
            
            int rowIndex = i % 9;
            row = rowIndex switch
            {
                0 => row1,
                1 => row2,
                2 => row3,
                3 => row4,
                4 => row5,
                5 => row6,
                6 => row7,
                7 => row8,
                _ => row9
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
                case 13:
                case 14:
                    P_ColumnSorterer(box1);
                    break;
                case 32:
                    FindHiddenLocks_SetToBlockedHashSet_Index32();
                    J_ColumnSorterer();
                    break;
                /*case 33:
                    List<HashSet<int>> results = FindHiddenLocks_SetToReuireList_Index33_34_35();
                    ToNumber_ColumnSorterer(results[0]);
                    i++;
                    rowMajorIndex = (i % 9) * 9 + (i / 9);
                    ToNumber_ColumnSorterer(results[1]);
                    i++;
                    rowMajorIndex = (i % 9) * 9 + (i / 9);
                    ToNumber_ColumnSorterer(results[2]);
                    break;*/
                default:
                    DefaultColumnSorterer();
                    break;
            }
        }
        /*
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
        }*/
        void J_ColumnSorterer()
        {
            int number = Field[rowMajorIndex];

            nominants = Block.Revert(targetBox);
            nominants.IntersectWith(Block.Revert(column));
            nominants.IntersectWith(Block.Revert(row));
            nominants.ExceptWith(blocked);
            
            if (nominants.Count == 0)
            {
                Debug.LogError("Zero nominants");
                number = 0;
            }
            else
            {
                number = nominants.ToList().PullRandom();
            }

            Field[rowMajorIndex] = number;
            
            targetBox.Add(number);
            column.Add(number);
            row.Add(number);
        }
        void P_ColumnSorterer(HashSet<int> upperBox)
        {
            int number = Field[rowMajorIndex];

            nominants = Block.Revert(targetBox);
            nominants.IntersectWith(Block.Revert(column));
            nominants.IntersectWith(Block.Revert(row));
            
            required = Block.Revert(upperBox);
            required.ExceptWith(targetBox);
            
            int cellsLeft = 6 - column.Count;

            if (cellsLeft == required.Count)
                nominants = required;
            
            if (nominants.Count == 0)
            {
                Debug.LogError("Zero nominants");
                number = 0;
            }
            else
            {
                number = nominants.ToList().PullRandom();
            }

            Field[rowMajorIndex] = number;
            
            targetBox.Add(number);
            column.Add(number);
            row.Add(number);
        }
        void DefaultColumnSorterer()
        {
            int number = Field[rowMajorIndex];

            nominants = Block.Revert(targetBox);
            nominants.IntersectWith(Block.Revert(column));
            nominants.IntersectWith(Block.Revert(row));
            
            if (nominants.Count == 0)
            {
                Debug.LogError("errorCooldown > 8");
                number = 0;
            }
            else
            {
                number = nominants.ToList().PullRandom();
            }

            Field[rowMajorIndex] = number;
            
            targetBox.Add(number);
            column.Add(number);
            row.Add(number);
        }
        
        void FindHiddenLocks_SetToBlockedHashSet_Index32()
        {
            blocked = column4.ToHashSet();
            blocked.Add(Field[54]);
            blocked.Add(Field[55]);
            blocked.Add(Field[56]);
            if (blocked.Count == 8)
            {
                blocked = Block.Revert(blocked);
                return;
            }
            blocked = column4.ToHashSet();
            blocked.Add(Field[63]);
            blocked.Add(Field[64]);
            blocked.Add(Field[65]);
            if (blocked.Count == 8)
            {
                blocked = Block.Revert(blocked);
                return;
            }
            blocked = column4.ToHashSet();
            blocked.Add(Field[72]);
            blocked.Add(Field[73]);
            blocked.Add(Field[74]);
            if (blocked.Count == 8)
            {
                blocked = Block.Revert(blocked);
                return;
            }
            blocked = new();
        }
        /*List<HashSet<int>> FindHiddenLocks_SetToReuireList_Index33_34_35()
        {
            List<HashSet<int>> result = new ();

            nominants = Block.Revert(column4.ToHashSet());

            HashSet<int> checkLine = new HashSet<int>()
            {
                TempField[54],
                TempField[55],
                TempField[56]
            };
            checkLine.IntersectWith(nominants);
            if (checkLine.Count == 2)
            {
                HashSet<int> Line2 = new(nominants);
                Line2.ExceptWith(checkLine);
                nominants.Remove(Line2.First());
                result.Add(Line2);
                result.Add(nominants);
                result.Add(nominants);

                return result;
            }

            checkLine = new HashSet<int>()
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
                nominants.Remove(Line2.First());
                result.Add(nominants);
                result.Add(Line2);
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
                result.Add(nominants);
                result.Add(Line3);

                return result;
            }

            result.Add(nominants);
            result.Add(nominants);
            result.Add(nominants);

            if (checkLine.Count == 1)
            {
                nominants.Remove(checkLine.First());
                bool random = GameMath.HeadsOrTails();
                if (random)
                    result[0] = checkLine;
                else
                    result[1] = checkLine;
            }

            return result;
        }*/
    }

    private void PrintView()
    {
        for (int i = 0; i < Field.Length; i++)
        {
            Tiles[i].Main.text = Field[i].ToString();
        }
    }
}
