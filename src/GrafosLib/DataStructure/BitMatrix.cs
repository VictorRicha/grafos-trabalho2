using System;
using System.Collections;
using System.Collections.Generic;

namespace GrafosLib.DataStructure
{
    public class BitMatrix
    {
        public BitArray[] Data; // an array of arrays
        public int Dim; // dimension

        public BitMatrix(int n)
        {
            Data = new BitArray[n];
            for (var i = 0; i < Data.Length; ++i)
            {
                Data[i] = new BitArray(n);
            }
            Dim = n;
        }

        public bool GetValue(int row, int col)
        {
            return Data[row][col];
        }

        public void SetValue(int row, int col, bool value)
        {
            Data[row][col] = value;
        }

        public override string ToString()
        {
            var s = "";
            for (var i = 0; i < Data.Length; ++i)
            {
                for (var j = 0; j < Data[i].Length; ++j)
                {
                    if (Data[i][j]) s += "1 ";
                    else s += "0 ";
                }
                s += Environment.NewLine;
            }
            return s;
        }

        public IEnumerable<int> GetArray(int i)
        {
            var a = new List<int>();
            for (var j = 0; j < Data[i].Length; j++)
            {
                if (GetValue(i, j))
                    a.Add(j);
            }
            return a;
        }
    }
}