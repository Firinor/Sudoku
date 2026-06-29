using System.Collections.Generic;

namespace Sudoku
{
    public class Block
    {
        public List<int> Numbers = new List<int>{1,2,3,4,5,6,7,8,9};

        public static HashSet<int> NewHashSetFrom1To9()
        {
            return new HashSet<int>{1,2,3,4,5,6,7,8,9};
        }
        
        public void Shuffle()
        {
            Numbers.Shuffle();
        }

        public static List<int> Revert(List<int> ints)
        {
            List<int> result = new List<int>{1,2,3,4,5,6,7,8,9};
            foreach (var number in ints)
            {
                result.Remove(number);
            }
            return result;
        }
        public static HashSet<int> Revert(HashSet<int> ints)
        {
            HashSet<int> result = new HashSet<int>{1,2,3,4,5,6,7,8,9};
            foreach (var number in ints)
            {
                result.Remove(number);
            }
            return result;
        }
    }
}
