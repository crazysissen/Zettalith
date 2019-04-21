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
            get => array[x + y *  width];
            set => array[x + y * width] = value;
        }

        public ConvertibleGrid(int width, int height)
        {
            this.width = width;
            this.height = height;

            array = new T[width * height];
        }
    }
}
