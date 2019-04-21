using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zettalith
{
    class ConvertibleGrid<T>
    {
        public T[] array;
        public int width, height;

        public T this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        public T this[int x, int y]
        {
            get => array[x + y *  height];
            set => array[x + y * height] = value;
        }

        public ConvertibleGrid(int width, int height)
        {
            array = new T[width * height];
        }
    }
}
