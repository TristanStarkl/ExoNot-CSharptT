using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percolation
{
    public class Percolation
    {
        private readonly bool[,] _open;
        private readonly bool[,] _full;
        private readonly int _size;
        private bool _percolate;

        public Percolation(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, "Taille de la grille négative ou nulle.");
            }

            _open = new bool[size, size];
            _full = new bool[size, size];
            _size = size;
        }

        /*
         * Renvoie l'état de la case i, j du tableau open
         * Si i et j ne sont pas dans le tableau, renvoie false
         */
        private bool IsOpen(int i, int j)
        {
            if (i >= 0 && i <= this._size && j >= 0 && j <= this._size)
                return this._open[i, j];
            return false;
        }
        private bool IsOpen(KeyValuePair<int, int> cell)
        {
            return this.IsOpen(cell.Key, cell.Value);
        }

        /*
         * Renvoie l'état de la case i, j du tableau open
         * Si i et j ne sont pas dans le tableau, renvoie false
         */
        private bool IsFull(int i, int j)
        {
            if (i >= 0 && i <= this._size && j >= 0 && j <= this._size)
                return this._full[i, j];
            return false;
        }

        /* 
         * Surcharge qui prend une cell
         */
        private bool IsFull(KeyValuePair<int, int> cell)
        {
            return this.IsFull(cell.Key, cell.Value);
        }

        private List<KeyValuePair<int, int>> CloseNeighbors(int i, int j)
        {
            List<KeyValuePair<int, int>> resultat = new List<KeyValuePair<int, int>>();

            if (i >= 1)
                resultat.Add(new KeyValuePair<int, int>(i - 1, j));
            if (i < this._size - 1)
                resultat.Add(new KeyValuePair<int, int>(i + 1, j));
            if (j >= 1)
                resultat.Add(new KeyValuePair<int, int>(i, j - 1));
            if (j < this._size - 1)
                resultat.Add(new KeyValuePair<int, int>(i, j + 1));

            return resultat;
        }

        private void UpdateNeighbors(int i, int j)
        {
            List<KeyValuePair<int, int>> listCells = this.CloseNeighbors(i, j);
            foreach (KeyValuePair<int, int> cell in listCells)
            {
                if (this.IsOpen(cell) && !this.IsFull(cell))
                {
                    this._full[cell.Key, cell.Value] = true;
                    this.UpdateNeighbors(cell.Key, cell.Value);
                }
            }
        }

        private void Open(int i, int j)
        {
            if (i >= 0 && i <= this._size && j >= 0 && j <= this._size)
            {
                if (this.IsOpen(i, j))
                    return;
                this._open[i, j] = true;
                if (j == this._size)
                    this._full[i, j] = true;

                // Check si les neighbourds sont plein d'eau
                List<KeyValuePair<int, int>> listCells = this.CloseNeighbors(i, j);
                foreach (KeyValuePair<int, int> cell in listCells)
                {
                    if (this.IsFull(cell))
                        this._full[i, j] = true;
                }

                if (this._full[i, j])
                    this.UpdateNeighbors(i, j);
            }
            else
            {
                throw new ArgumentException("I et j sont en dehors de la range");
            }
            if (this._full[i, j] && j == this._size - 1)
                this._percolate = true;
        }

        public bool Percolate()
        {
            return this._percolate;
        }

        public void Display()
        {
            for (int i = 0; i < this._size; i++)
            {
                for (int j = 0; j < this._size; j++)
                {
                    if (this.IsOpen(i, j))
                        Console.Write('o');
                    else
                        Console.Write('x');
                }
                Console.WriteLine(' ');
            }
        }
    }
}
