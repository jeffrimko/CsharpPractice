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
        private float[] _myNums;
        private int _index;

        public NumberEnumerator(params float[] nums)
        {
            _myNums = nums;
            Reset();
        }

        public void Reset()
        {
            _index = -1;
        }

        public bool MoveNext()
        {
            _index++;
            return (_index < _myNums.Length);
        }

        public object Current
        {
            get
            {
                return _myNums[_index];
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
