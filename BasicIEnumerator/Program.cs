using System;
using System.Collections;

namespace BasicIEnumerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var nums = new NumberEnumerator(1.23f, 4.56f, 7.89f);
            foreach (var num in nums)
            {
                Console.WriteLine(num);
            }
            Console.ReadLine();
        }
    }

    class NumberEnumerator : IEnumerable, IEnumerator
    {
        private float[] myNums;
        private int index;

        public NumberEnumerator(params float[] nums)
        {
            myNums = nums;
            Reset();
        }

        public void Reset()
        {
            index = -1;
        }

        public bool MoveNext()
        {
            index++;
            return (index < myNums.Length);
        }

        public object Current
        {
            get
            {
                return myNums[index];
            }
        }

        public IEnumerator GetEnumerator()
        {
            while (MoveNext())
            {
                yield return Current;
            }
            Reset();
        }
    }
}
