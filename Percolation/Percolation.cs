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
        private bool[,] _alreadyChecked;
        private bool _percolate;

        public Percolation(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, "Taille de la grille négative ou nulle.");
            }

            _open = new bool[size, size];
            _full = new bool[size, size];
            _alreadyChecked = new bool[size, size];
            _size = size;
        }

        /*
         * Renvoie l'état de la case i, j du tableau open
         * Si i et j ne sont pas dans le tableau, renvoie false
         */
        private bool IsOpen(int i, int j)
        {
            if (i >= 0 && i <= this._size && j >= 0 && j <= this._size)
                return this._open[i,j];
            return false;
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

        private void Open(int i, int j)
        {
            throw new NotImplementedException();
        }

        /*
         * Renvoie la liste des cellules voisines qui sont ouvertes ET qui ne sont pas déjà vérifiées
         */
        private List<KeyValuePair<int, int>> GetOpenNeighborsNotChecked(KeyValuePair<int, int> cell)
        {
            List<KeyValuePair<int, int>> resultat = new List<KeyValuePair<int, int>>();
            int i = cell.Key;
            int j = cell.Value;

            if (i >= 1 && this.IsOpen(i -1, j) && !this._alreadyChecked[i - 1, j])
                resultat.Add(new KeyValuePair<int, int>(i - 1, j));
            if (i < this._size - 1 && this.IsOpen(i + 1, j) && !this._alreadyChecked[i + 1, j])
                resultat.Add(new KeyValuePair<int, int>(i + 1, j));
            if (j >= 1 && this.IsOpen(i, j - 1) && !this._alreadyChecked[i , j - 1])
                resultat.Add(new KeyValuePair<int, int>(i, j - 1));
            if (j < this._size - 1 && this.IsOpen(i , j + 1) && !this._alreadyChecked[i, j + 1])
                resultat.Add(new KeyValuePair<int, int>(i, j + 1));

            return resultat;
        }

        /*
         * Check par récursion de chaque cellule, si on atteint le bas on renvoie true
         */
        private bool PercolateCell(KeyValuePair<int, int> cellule)
        {
            if (cellule.Key == this._size)
                return true;
            if (this._alreadyChecked[cellule.Key, cellule.Value])
                return false;
            List<KeyValuePair<int, int>> listOpenedNeighbors = this.GetOpenNeighborsNotChecked(cellule);
            // On marque la cellule comme étant vérifiée
            this._alreadyChecked[cellule.Key, cellule.Value] = true;

            foreach (KeyValuePair<int, int> cell in listOpenedNeighbors)
            {
                if (PercolateCell(cell))
                    return true;
            }

            return false;
        }

        private void UncheckAllCells()
        {
            for (int i = 0; i < this._size; i++)
            {
                for (int j = 0; j < this._size; j++)
                {
                    this._alreadyChecked[i, j] = false;
                }
            }
        }

        public void DebugOpenAllCells()
        {
            for (int i = 0; i < this._size; i++)
            {
                for (int j = 0; j < this._size; j++)
                {
                    this._open[i, j] = true;
                }
            }
        }

        public bool Percolate()
        {
            this.UncheckAllCells();

            return PercolateCell(new KeyValuePair<int, int>(0, 0));
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
