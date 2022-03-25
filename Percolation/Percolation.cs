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

        /*
         * Constructeur
         * @params int size: >0, la taille de la grille
         */
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
         * @param int i, int j: les coordonnées
         * @return bool: l'état (ouverte ou fermée)
         */
        public bool IsOpen(int i, int j)
        {
            if (i >= 0 && i <= this._size && j >= 0 && j <= this._size)
                return this._open[i, j];
            return false;
        }
        public bool IsOpen(KeyValuePair<int, int> cell)
        {
            return this.IsOpen(cell.Key, cell.Value);
        }

        /*
         * Renvoie l'état de la case i, j du tableau open
         * Si i et j ne sont pas dans le tableau, renvoie false
         * @param int i, int j: les coordonnées
         * @return bool: l'état (pleine d'eau ou non)
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

        /*
         * Renvoie la liste des voisins proches
         */
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

        /*
         * Met à jour les voisins 
         * Si la cellule voisine est pleine d'eau et que nous somme sèche, nous remplie d'eau et met à jour nos voisines
         */
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


        /*
         * Ouvre la cellule aux coordonénes i, j
         */
        public void Open(int i, int j)
        {
            if (i >= 0 && i <= this._size && j >= 0 && j <= this._size)
            {
                if (this.IsOpen(i, j))
                    return;
                this._open[i, j] = true;
                if (i == 0)
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
            for (int c = 0; c < this._size; c++)
            {
                if (this._full[this._size - 1, c])
                    this._percolate = true;
            }
        }

        /*
         * Surcharge avec une coordonnée
         */
        public void Open(KeyValuePair<int, int> cell)
        {
            this.Open(cell.Key, cell.Value);
        }

        /*
         * Renvoie l'état de est-ce qu'on percolate
         */
        public bool Percolate()
        {
            return this._percolate;
        }

        /*
         * Display la grille
         * à gauche la grille contenant du vide ou des rochers
         * à droite la grille contenant l'eau ou du vide
         */
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
                Console.Write(' ');
                for (int j = 0; j < this._size; j++)
                {
                    if (this.IsFull(i, j))
                        Console.Write('o');
                    else
                        Console.Write('x');
                }
                Console.WriteLine(" ");
            }
        }
    }
}
