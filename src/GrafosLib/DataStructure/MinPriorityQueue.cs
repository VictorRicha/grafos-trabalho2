using System;
using System.Collections;
using System.Collections.Generic;

namespace GrafosLib.DataStructure
{
    public class MinPriorityQueue<T> : IEnumerable<int>
        where T : IComparable<T>
    {
        private readonly int _nmax; //the maximum number of elements in the priority queue       
        private int _n; //the number of elements in the priority queue

        private readonly int[] _pq;
            //the array of items in the priority queue, _pq[i] is an integer that points to the index of _keys[]

        private readonly int[] _qp; //inverse of _pq - _qp[_pq[i]] = _pq[_qp[i]] = i
        private readonly T[] _keys; //the array of values in the priority queue

        public MinPriorityQueue(int nmax)
        {
            if (nmax < 0) throw new Exception();
            _nmax = nmax;
            //initialize the _keys, _pq, and _pq arrays
            _keys = new T[nmax + 1];
            _pq = new int[nmax + 1];
            _qp = new int[nmax + 1];
            //Initialize each value in _qp to -1
            for (var i = 0; i <= nmax; i++) _qp[i] = -1;
        }

        //return true if the priority queue is empty
        public bool IsEmpty()
        {
            return _n == 0;
        }

        //Check to see if i is an index in the priority queue
        public bool Contains(int i)
        {
            if (i < 0 || i >= _nmax) throw new Exception();
            return _qp[i] != -1;
        }

        //how many elements are in the priority queue
        public int Size()
        {
            return _n;
        }

        // insert an element into the priority queue
        public void Insert(int i, T key)
        {
            //check to make sure the index isn't already in the priority queue
            if (i < 0 || i >= _nmax) throw new Exception();
            if (Contains(i)) throw new Exception("index is already in the priority queue");
            //increase the element count by 1
            _n++;
            _qp[i] = _n;
            //Add i to _pq array which will point to key at index i
            _pq[_n] = i;
            _keys[i] = key;
            //Call swim to maintain the minimum property by moving it up
            Swim(_n);
        }

        /*
        * An important function in maintaining the minimum priority queue.
        * To make sure a key is in the proper position we want to move it up the "tree" by comparing it
        * to the parent key, if it is less then exchange it with it's parent.  We keep doing this iteratively until
        * the parent of the key is less than the key we are swimming.
        * The parent of a key at index _pq[i] is the key at index _pq[i/2].  As a note i/2 is always rounded down.
        * */

        private void Swim(int k)
        {
            while (k > 1 && Greater(k/2, k))
            {
                Exchange(k, k/2);
                k = k/2;
            }
        }

        /*
        * An important function in maintaining the minimum priority queue.
        * To make sure a key is in the proper position we may to move it down the "tree" by comparing it
        * to its right child, if it is greater then exchange it with the child.  
        * We keep doing this iteratively until the right child of the key is greater than the key we are sinking.
        * The child of a key at index _pq[i] is the key at index _pq[2i].  
        * */

        private void Sink(int k)
        {
            while (2*k <= _n)
            {
                var j = 2*k;
                if (j < _n && Greater(j, j + 1)) j++;
                if (!Greater(k, j)) break;
                Exchange(k, j);
                k = j;
            }
        }

        /*
        * Compare the _keys at index _pq[i] and index _pq[j], return true
        * if key at index _pq[i] is greater than at index _pq[j]
        * */

        private bool Greater(int i, int j)
        {
            return _keys[_pq[i]].CompareTo(_keys[_pq[j]]) > 0;
        }

        //Exchange _keys in the priority queue by exchanging their indexes in _pq
        private void Exchange(int i, int j)
        {
            var swap = _pq[i];
            _pq[i] = _pq[j];
            _pq[j] = swap;
            _qp[_pq[i]] = i;
            _qp[_pq[j]] = j;
        }

        //The minimum key in the queue (_keys) will always be at _pq[1]
        public T MinKey()
        {
            if (_n == 0) throw new Exception("Priority queue underflow");
            return _keys[_pq[1]];
        }

        //Delete _pq[1] and re-order the queue
        public int DeleteMin()
        {
            if (_n == 0) throw new Exception("Priority queue underflow");
            //get a reference to the minimum
            var min = _pq[1];
            //exchange _pq[1] with _pq[_n] and subtract 1 from _n
            Exchange(1, _n--);
            //_pq[_n] is not the minimum value so push it down
            Sink(1);
            /*
            * mark the item in the queue as deleted by setting _pq[min] to -1 and the corresponding
            * _keys value (which is now at the end of _keys[]) to default(key)
            * */
            _qp[min] = -1;
            _keys[_pq[_n + 1]] = default(T);
            _pq[_n + 1] = -1;
            return min;
        }

        /*
        * Change the Key value at index i
        * */

        public void ChangeKey(int i, T key)
        {
            if (i < 0 || i >= _nmax) throw new Exception();
            if (!Contains(i)) throw new Exception("index is not in the priority queue");
            //first, update the key value
            _keys[i] = key;
            /*
            * in order to ensure the minimum key is at the top of the que first move the key
            * to the top of the queue and then move it down
            * */
            Swim(_qp[i]);
            Sink(_qp[i]);
        }

        //set key at index i to a lower value and reorder the queue
        public void DecreaseKey(int i, T key)
        {
            if (i < 0 || i >= _nmax) throw new Exception();
            if (!Contains(i)) throw new Exception("index is not in the priority queue");
            if (_keys[i].CompareTo(key) <= 0)
                throw new Exception("Calling decreaseKey() with given argument would not strictly decrease the key");
            _keys[i] = key;
            Swim(_qp[i]);
        }

        //set the key at index i to a higher value and reorder the queue
        public void IncreaseKey(int i, T key)
        {
            if (i < 0 || i >= _nmax) throw new Exception();
            if (!Contains(i)) throw new Exception("index is not in the priority queue");
            if (_keys[i].CompareTo(key) >= 0)
                throw new Exception("Calling increaseKey() with given argument would not strictly increase the key");
            _keys[i] = key;
            Sink(_qp[i]);
        }

        //delete the key at index i
        public void Delete(int i)
        {
            if (i < 0 || i >= _nmax) throw new Exception();
            if (!Contains(i)) throw new Exception("index is not in the priority queue");
            var index = _qp[i];
            Exchange(index, _n--);
            Swim(index);
            Sink(index);
            _keys[i] = default(T);
            _qp[i] = -1;
        }

        /*
        * Implement a custom iterator
        * */

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<int> GetEnumerator()
        {
            var copy = new MinPriorityQueue<T>(_pq.Length - 1);

            for (var i = 1; i <= _n; i++)
            {
                copy.Insert(_pq[i], _keys[_pq[i]]);
            }
            for (var i = 1; i <= _n; i++)
            {
                yield return copy.DeleteMin();
            }
        }
    }
}