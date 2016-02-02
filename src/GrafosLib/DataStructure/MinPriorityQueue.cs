using System;
using System.Collections;
using System.Collections.Generic;

namespace GrafosLib.DataStructure
{
    public class MinPriorityQueue<T> : IEnumerable<int>
        where T : IComparable<T>
    {
        /// <summary>
        /// numero maximo de elementos na fila
        /// </summary>
        private readonly int _nmax;

        /// <summary>
        /// numero de elementos na fila
        /// </summary>
        private int _n;

        /// <summary>
        /// array de itens na fila
        /// _pq[i] é um int que aponta para o indice de _keys[]
        /// </summary>
        private readonly int[] _pq;

        /// <summary>
        /// inverso de _pq
        /// _pq - _qp[_pq[i]] = _pq[_qp[i]] = i
        /// </summary>
        private readonly int[] _qp;

        /// <summary>
        /// array de valores na fila de prioridade
        /// </summary>
        private readonly T[] _keys;

        public MinPriorityQueue(int nmax)
        {
            if (nmax < 0) throw new Exception();
            _nmax = nmax;

            // inicializa os arrays _keys, _pq, e _pq
            _keys = new T[nmax + 1];
            _pq = new int[nmax + 1];
            _qp = new int[nmax + 1];

            // _qp -> -1
            for (var i = 0; i <= nmax; i++) _qp[i] = -1;
        }

        /// <summary>
        /// Verificar se a fila está vazia
        /// </summary>
        public bool IsEmpty()
        {
            return _n == 0;
        }

        /// <summary>
        /// verifica se i é um indice na fila
        /// </summary>
        public bool Contains(int i)
        {
            if (i < 0 || i >= _nmax) throw new Exception();
            return _qp[i] != -1;
        }

        /// <summary>
        /// Tamanho da fila
        /// </summary>
        public int Size()
        {
            return _n;
        }

        /// <summary>
        /// Insere um elemento na fila
        /// </summary>
        public void Insert(int i, T key)
        {
            //verifica que índice não está na fila
            if (i < 0 || i >= _nmax) throw new Exception();
            if (Contains(i)) throw new Exception("index is already in the priority queue");
            _n++;
            _qp[i] = _n;

            //Adiciona i ao array _pq  que irá apontar para o key no indice i
            _pq[_n] = i;
            _keys[i] = key;

            //up
            Up(_n);
        }
        
        private void Up(int k)
        {
            while (k > 1 && Greater(k/2, k))
            {
                Exchange(k, k/2);
                k = k/2;
            }
        }

        private void Down(int k)
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

        private bool Greater(int i, int j)
        {
            return _keys[_pq[i]].CompareTo(_keys[_pq[j]]) > 0;
        }

        private void Exchange(int i, int j)
        {
            var swap = _pq[i];
            _pq[i] = _pq[j];
            _pq[j] = swap;
            _qp[_pq[i]] = i;
            _qp[_pq[j]] = j;
        }

        public T MinKey()
        {
            if (_n == 0) throw new Exception("Priority queue underflow");
            return _keys[_pq[1]];
        }

        public int DeleteMin()
        {
            if (_n == 0) throw new Exception("Priority queue underflow");
            //get a reference to the minimum
            var min = _pq[1];
            //exchange _pq[1] with _pq[_n] and subtract 1 from _n
            Exchange(1, _n--);
            //_pq[_n] is not the minimum value so push it down
            Down(1);
            /*
            * mark the item in the queue as deleted by setting _pq[min] to -1 and the corresponding
            * _keys value (which is now at the end of _keys[]) to default(key)
            * */
            _qp[min] = -1;
            _keys[_pq[_n + 1]] = default(T);
            _pq[_n + 1] = -1;
            return min;
        }

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
            Up(_qp[i]);
            Down(_qp[i]);
        }

        public void DecreaseKey(int i, T key)
        {
            if (i < 0 || i >= _nmax) throw new Exception();
            if (!Contains(i)) throw new Exception("index is not in the priority queue");
            if (_keys[i].CompareTo(key) <= 0)
                throw new Exception("Calling decreaseKey() with given argument would not strictly decrease the key");
            _keys[i] = key;
            Up(_qp[i]);
        }

        public void IncreaseKey(int i, T key)
        {
            if (i < 0 || i >= _nmax) throw new Exception();
            if (!Contains(i)) throw new Exception("index is not in the priority queue");
            if (_keys[i].CompareTo(key) >= 0)
                throw new Exception("Calling increaseKey() with given argument would not strictly increase the key");
            _keys[i] = key;
            Down(_qp[i]);
        }

        public void Delete(int i)
        {
            if (i < 0 || i >= _nmax) throw new Exception();
            if (!Contains(i)) throw new Exception("index is not in the priority queue");
            var index = _qp[i];
            Exchange(index, _n--);
            Up(index);
            Down(index);
            _keys[i] = default(T);
            _qp[i] = -1;
        }

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