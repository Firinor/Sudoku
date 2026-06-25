using System.Collections.Generic;

namespace Sudoku
{
    public class Block
    {
        public List<int> Numbers = new List<int>{1,2,3,4,5,6,7,8,9};

        public void Shuffle()
        {
            Numbers.Shuffle();
        }
    }
}
