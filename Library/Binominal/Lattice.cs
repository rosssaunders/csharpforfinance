using System;
using System.Collections.Generic;
using System.Text;
using Library.Data;

namespace Library.Binominal
{
    public class Lattice<T> where T : new()
    { 
        // Generic lattice class; The data type T is a value type

        // Input
        //
        // N .. depth of lattice (numnber of rows less one)
        // type .. the degree of the lattice, e.g. binomial, trinomial
        //

        // Implement as a built-in .NET class
        private T[][] _data;

        // Redundant data
        private readonly int _type;            // What kind of lattice (number of nodes); binomial == 2, trinomial == 3

        private void Init(T value)
        {
            _data = new T[MaxIndex + 1][];
            var currentBranch = 1;  // There is always one single root

            for (var j = 0; j <= MaxIndex; j++)
            {
                _data[j] = new T[currentBranch];
                currentBranch += (_type - 1);

                for (var i = 0; i < 1 + j * (_type - 1); i++)
                {
                    _data[j][i] = value;
                }
            }
        }

        // Constructors  destructor
        public Lattice()
        { 
            // Default constructor, binomial lattice of fixed length
            _type = 2;
            MaxIndex = 10;

            var myDefault = new T();
            Init(myDefault);
        }

        public Lattice(int nRows, int numberBranches)
        { 
            // Number of rows and branch factor
            _type = numberBranches;
            MaxIndex = nRows;

            var myDefault = new T();
            Init(myDefault);
        }

        public Lattice(int nRows, int numberBranches, T val)
        { 
            // + value at nodes
            _type = numberBranches;
            MaxIndex = nRows;

            Init(val);
        }

        // Iterating in a Lattice; we need forward and backward versions
        // Return the minimum index of the outer 'row array'
        public int MinIndex => 0;

        // Return the maximum index of outer 'row array'
        public int MaxIndex { get; }

        // Number of columnms at a given row
        public int NumberColumns(int row)
        {
            return 1 + row * (_type - 1);
        }

        // Accessing the elements of the lattice at a row and columns
        // No exception handling (this is the responsibility of client)
        public T Get(int row, int column)
        {
            return _data[row][column];
        }

        public void Set(int row, int column, T newValue)
        {
            _data[row][column] = newValue;
        }

        // Making it possible to use operator overload [i,j]; N.B. (i,j) not possible
        public T this[int row, int column]
        {
            // Get the element at position    
            get => _data[row][column];
            // Set the element at position
            set => _data[row][column] = value;
        }

        public Vector<T> PyramidVector(int row)
        { 
            // Generate the array at a given 'row'
            var startIndex = MinIndex;

            var result = new Vector<T>(1 + row * (_type - 1), startIndex);

            for (var i = result.MinIndex; i < 1 + row * (_type - 1); i++)
            {
                result[i] = _data[row][i];
            }

            return result;
        }

        public Vector<T> BasePyramidVector()
        { 
            // Generate the array at the large end of the lattice
            var startIndex = MinIndex;
            var maxRow = MaxIndex;

            var result = new Vector<T>(1 + MaxIndex * (_type - 1), startIndex);

            for (var i = result.MinIndex; i <= result.MaxIndex; i++)
            {
                result[i] = _data[maxRow][i];
            }

            return result;
        }
    }
}
