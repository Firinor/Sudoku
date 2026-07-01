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
        
        for (int i = 0; i < 81; i++) //We are sorting only 8 columns. The last column will be resolved automatically.
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
            rowMajorIndex = ColumnMajorIndex_To_RowMajorIndex(i);//From column-major to row-major
            
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
                case 33:
                    FindHiddenLocks_SetToIndex33_34_35();
                    i += 2;//to next column => i = 36
                    break;
                case 38:
                    FindHiddenLocks_SetToIndex38__39_40_41__42_43_44();
                    i = 44;//to next box => i = 45
                    break;
                default:
                    DefaultColumnSorterer();
                    break;
            }
        }
        return;
        
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
            int number;
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
                Debug.LogError("errorCooldown "+rowMajorIndex+" > 8");
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
        void FindHiddenLocks_SetToIndex33_34_35()
        {
            nominants = Block.Revert(column);

            int row7blokeratorCount = nominants.Intersect(row7).Count();
            int row8blokeratorCount = nominants.Intersect(row8).Count();;
            int row9blokeratorCount = nominants.Intersect(row9).Count();;

            int i33 = ColumnMajorIndex_To_RowMajorIndex(33);
            int i34 = ColumnMajorIndex_To_RowMajorIndex(34);
            int i35 = ColumnMajorIndex_To_RowMajorIndex(35);
            
            if (row7blokeratorCount == 3 || row8blokeratorCount == 3 || row9blokeratorCount == 3)
            {
                Debug.LogError("FindHiddenLocks_SetToIndex33_34_35 =>" 
                               + " row7:" + row7blokeratorCount 
                               + " row8:" + row8blokeratorCount 
                               + " row9:" + row9blokeratorCount+"!");
            }
            else if (row7blokeratorCount == 2 || row8blokeratorCount == 2 || row9blokeratorCount == 2)
            {
                int number;
                if (row7blokeratorCount == 2)
                {
                    number = nominants.Except(row7).First();
                    Field[i33] = number;
                    nominants.Remove(number);
                    box6.Add(number);
                    column4.Add(number);
                    row7.Add(number);

                    List<int> listNominants = nominants.ToList();
                    listNominants.Shuffle();
                    Field[i34] = listNominants[0];
                    box6.Add(Field[i34]);
                    column4.Add(Field[i34]);
                    row8.Add(Field[i34]);
                    Field[i35] = listNominants[1];
                    box6.Add(Field[i35]);
                    column4.Add(Field[i35]);
                    row9.Add(Field[i35]);
                }
                else if (row8blokeratorCount == 2)
                {   
                    number = nominants.Except(row8).First();
                    Field[i34] = number;
                    nominants.Remove(number);
                    box6.Add(number);
                    column4.Add(number);
                    row8.Add(number);

                    List<int> listNominants = nominants.ToList();
                    listNominants.Shuffle();
                    Field[i33] = listNominants[0];
                    box6.Add(Field[i33]);
                    column4.Add(Field[i33]);
                    row7.Add(Field[i33]);
                    Field[i35] = listNominants[1];
                    box6.Add(Field[i35]);
                    column4.Add(Field[i35]);
                    row9.Add(Field[i35]);
                }
                else//row9blokeratorCount == 2
                {
                    number = nominants.Except(row9).First();
                    Field[i35] = number;
                    nominants.Remove(number);
                    box6.Add(number);
                    column4.Add(number);
                    row9.Add(number);

                    List<int> listNominants = nominants.ToList();
                    listNominants.Shuffle();
                    Field[i33] = listNominants[0];
                    box6.Add(Field[i33]);
                    column4.Add(Field[i33]);
                    row7.Add(Field[i33]);
                    Field[i34] = listNominants[1];
                    box6.Add(Field[i34]);
                    column4.Add(Field[i34]);
                    row8.Add(Field[i34]);
                }
            }
            else//Count 1
            {
                List<int> lockBlockers = new()
                {
                    nominants.Intersect(row7).First(),
                    nominants.Intersect(row8).First(),
                    nominants.Intersect(row9).First()
                };

                if (GameMath.HeadsOrTails())
                {
                    Field[i33] = lockBlockers[1];
                    box6.Add(lockBlockers[1]);
                    column4.Add(lockBlockers[1]);
                    row7.Add(lockBlockers[1]);
                    Field[i34] = lockBlockers[2];
                    box6.Add(lockBlockers[2]);
                    column4.Add(lockBlockers[2]);
                    row8.Add(lockBlockers[2]);
                    Field[i35] = lockBlockers[0];
                    box6.Add(lockBlockers[0]);
                    column4.Add(lockBlockers[0]);
                    row9.Add(lockBlockers[0]);
                }
                else
                {
                    Field[i33] = lockBlockers[2];
                    box6.Add(lockBlockers[2]);
                    column4.Add(lockBlockers[2]);
                    row7.Add(lockBlockers[2]);
                    Field[i34] = lockBlockers[0];
                    box6.Add(lockBlockers[0]);
                    column4.Add(lockBlockers[0]);
                    row8.Add(lockBlockers[0]);
                    Field[i35] = lockBlockers[1];
                    box6.Add(lockBlockers[1]);
                    column4.Add(lockBlockers[1]);
                    row9.Add(lockBlockers[1]);
                }
            }
        }
        void FindHiddenLocks_SetToIndex38__39_40_41__42_43_44()
        {
            List<int> nominants = Block.Revert(box4).ToList();

            Debug.Log("nominants:"+nominants[0]+""+nominants[1]+""+nominants[2]+""+nominants[3]);
            foreach (var nominant in nominants)
            {
                if(row3.Contains(nominant))
                    continue;
                
                Debug.Log("nominant:"+nominant);
                int nominantIndex38 = nominant;
                List<int> tempColumn = column5.ToList();
                tempColumn.Add(nominantIndex38);
                List<int> tempUpBox = box4.ToList();
                tempUpBox.Add(nominantIndex38);
                List<int> nominantBox5 = Block.Revert(tempUpBox).ToList();
                Debug.Log("requaire:"+nominantBox5[0]+""+nominantBox5[1]+""+nominantBox5[2]+" Count:"+nominantBox5.Count);
                List<int> box5Pool = Block.Revert(box5).ToList();
                
                foreach (var value in nominantBox5)
                    box5Pool.Remove(value);
                foreach (var value in tempColumn)
                    box5Pool.Remove(value);
                foreach (var value in box5)
                    nominantBox5.Remove(value);
                while(nominantBox5.Count < 3)
                    nominantBox5.Add(box5Pool.PullRandom());
                
                Debug.Log("nominantBox5:"+nominantBox5[0]+""+nominantBox5[1]+""+nominantBox5[2]+" Count:"+nominantBox5.Count);

                List<int> nominantBox6 = tempColumn.ToList();
                nominantBox6.AddRange(nominantBox5);
                nominantBox6 = Block.Revert(nominantBox6);
                Debug.Log("nominantBox6:"+nominantBox6[0]+""+nominantBox6[1]+""+nominantBox6[2]+" Count:"+nominantBox6.Count);
                
                int row4blokeratorCount = nominantBox5.Intersect(row4).Count();
                int row5blokeratorCount = nominantBox5.Intersect(row5).Count();
                int row6blokeratorCount = nominantBox5.Intersect(row6).Count();
                Debug.Log("blockerator456:"+row4blokeratorCount+""+row5blokeratorCount+""+row6blokeratorCount);

                if (row4blokeratorCount == 3 || row5blokeratorCount == 3 || row6blokeratorCount == 3)
                {
                    continue;
                }
                if (row4blokeratorCount == 2 || row5blokeratorCount == 2 || row6blokeratorCount == 2)
                {
                    int number;
                    if (row4blokeratorCount == 2)
                    {
                        number = nominantBox5.Except(row4).First();
                        List<int> listNominants = nominantBox5.ToList();
                        nominantBox5[0] = number;
                        listNominants.Remove(number);
                        listNominants.Shuffle();
                        nominantBox5[1] = listNominants[0];
                        nominantBox5[2] = listNominants[1];
                    }
                    else if (row5blokeratorCount == 2)
                    {
                        number = nominantBox5.Except(row5).First();
                        List<int> listNominants = nominantBox5.ToList();
                        nominantBox5[1] = number;
                        listNominants.Remove(number);
                        listNominants.Shuffle();
                        nominantBox5[0] = listNominants[0];
                        nominantBox5[2] = listNominants[1];
                    }
                    else //row6blokeratorCount == 2
                    {
                        number = nominantBox5.Except(row6).First();
                        List<int> listNominants = nominantBox5.ToList();
                        nominantBox5[2] = number;
                        listNominants.Remove(number);
                        listNominants.Shuffle();
                        nominantBox5[0] = listNominants[0];
                        nominantBox5[1] = listNominants[1];
                    }
                }
                else //Count 1
                {
                    List<int> lockBlockers = new()
                    {
                        nominantBox5.Intersect(row4).First(),
                        nominantBox5.Intersect(row5).First(),
                        nominantBox5.Intersect(row6).First()
                    };

                    if (GameMath.HeadsOrTails())
                    {
                        nominantBox5[0] = lockBlockers[1];
                        nominantBox5[1] = lockBlockers[2];
                        nominantBox5[2] = lockBlockers[0];
                    }
                    else
                    {
                        nominantBox5[0] = lockBlockers[2];
                        nominantBox5[1] = lockBlockers[0];
                        nominantBox5[2] = lockBlockers[1];
                    }
                }
                
                /////
                int row7blokeratorCount = nominantBox6.Intersect(row7).Count();
                int row8blokeratorCount = nominantBox6.Intersect(row8).Count();
                int row9blokeratorCount = nominantBox6.Intersect(row9).Count();

                Debug.Log("blockerator789:"+row7blokeratorCount+""+row8blokeratorCount+""+row9blokeratorCount);
                
                if (row7blokeratorCount == 3 || row8blokeratorCount == 3 || row9blokeratorCount == 3)
                {
                    continue;
                }
                if (row7blokeratorCount == 2 || row8blokeratorCount == 2 || row9blokeratorCount == 2)
                {
                    int number;
                    if (row7blokeratorCount == 2)
                    {
                        number = nominantBox6.Except(row7).First();
                        List<int> listNominants = nominantBox6.ToList();
                        nominantBox6[0] = number;
                        listNominants.Remove(number);
                        listNominants.Shuffle();
                        nominantBox6[1] = listNominants[0];
                        nominantBox6[2] = listNominants[1];
                    }
                    else if (row8blokeratorCount == 2)
                    {
                        number = nominantBox6.Except(row8).First();
                        List<int> listNominants = nominantBox6.ToList();
                        nominantBox6[1] = number;
                        listNominants.Remove(number);
                        listNominants.Shuffle();
                        nominantBox6[0] = listNominants[0];
                        nominantBox6[2] = listNominants[1];
                    }
                    else //row9blokeratorCount == 2
                    {
                        number = nominantBox6.Except(row9).First();
                        List<int> listNominants = nominantBox6.ToList();
                        nominantBox6[2] = number;
                        listNominants.Remove(number);
                        listNominants.Shuffle();
                        nominantBox6[0] = listNominants[0];
                        nominantBox6[1] = listNominants[1];
                    }
                }
                else //Count 1
                {
                    List<int> lockBlockers = new()
                    {
                        nominantBox6.Intersect(row7).First(),
                        nominantBox6.Intersect(row8).First(),
                        nominantBox6.Intersect(row9).First()
                    };

                    if (GameMath.HeadsOrTails())
                    {
                        nominantBox6[0] = lockBlockers[1];
                        nominantBox6[1] = lockBlockers[2];
                        nominantBox6[2] = lockBlockers[0];
                    }
                    else
                    {
                        nominantBox6[0] = lockBlockers[2];
                        nominantBox6[1] = lockBlockers[0];
                        nominantBox6[2] = lockBlockers[1];
                    }
                }
                
                
                int i38 = ColumnMajorIndex_To_RowMajorIndex(38);
                Field[i38] = nominantIndex38;
                box4.Add(Field[i38]);
                column5.Add(Field[i38]);
                row3.Add(Field[i38]);
                
                Debug.Log("nominantBox5Set:"+nominantBox5[0]+""+nominantBox5[1]+""+nominantBox5[2]);
                int i39 = ColumnMajorIndex_To_RowMajorIndex(39);
                Field[i39] = nominantBox5[0];
                box5.Add(Field[i39]);
                column5.Add(Field[i39]);
                row4.Add(Field[i39]);
                int i40 = ColumnMajorIndex_To_RowMajorIndex(40);
                Field[i40] = nominantBox5[1];
                box5.Add(Field[i40]);
                column5.Add(Field[i40]);
                row5.Add(Field[i40]);
                int i41 = ColumnMajorIndex_To_RowMajorIndex(41);
                Field[i41] = nominantBox5[2];
                box5.Add(Field[i41]);
                column5.Add(Field[i41]);
                row6.Add(Field[i41]);
                
                Debug.Log("nominantBox6Set:"+nominantBox6[0]+""+nominantBox6[1]+""+nominantBox6[2]);
                int i42 = ColumnMajorIndex_To_RowMajorIndex(42);
                Field[i42] = nominantBox6[0];
                box6.Add(Field[i42]);
                column5.Add(Field[i42]);
                row7.Add(Field[i42]);
                int i43 = ColumnMajorIndex_To_RowMajorIndex(43);
                Field[i43] = nominantBox6[1];
                box6.Add(Field[i43]);
                column5.Add(Field[i43]);
                row8.Add(Field[i43]);
                int i44 = ColumnMajorIndex_To_RowMajorIndex(44);
                Field[i44] = nominantBox6[2];
                box6.Add(Field[i44]);
                column5.Add(Field[i44]);
                row9.Add(Field[i44]);
                
                break;
            }
        }
    }

    private static int ColumnMajorIndex_To_RowMajorIndex(int i)
    {
        return (i % 9) * 9 + (i / 9);
    }

    private void PrintView()
    {
        for (int i = 0; i < Field.Length; i++)
        {
            Tiles[i].Main.text = Field[i].ToString();
        }
    }
}
