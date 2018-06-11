using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze_Generator
{
    public partial class Form1 : Form
    {
        private MyPanel[,] panels;
        private int rows, cols;
        private MyPanel cur;
        private int totalVisited = 0;
        private Stack<MyPanel> stack;
        List<MyPanel> n = new List<MyPanel>();
        public Form1()
        {
            InitializeComponent();
            rows = table.RowCount;
            cols = table.ColumnCount;
            Setup();
        }

        private void Setup()
        {
            stack = new Stack<MyPanel>();
            panels = new MyPanel[rows,cols];
            for (int i = 0; i<cols; i++)
            {
                for(int j = 0; j<rows; j++)
                {
                    panels[i, j] = new MyPanel();
                    panels[i,j].Size = new Size(table.Height / table.ColumnCount, table.Width / table.RowCount);
                    panels[i, j].BackColor = Color.Black;
                    table.Controls.Add(panels[i, j]);
                    table.SetCellPosition(panels[i, j], new TableLayoutPanelCellPosition(i, j));
                    panels[i, j].Margin = new Padding(1);
                    panels[i, j].col = i;
                    panels[i, j].row = j;

                }
            }
            cur = panels[0, 0];
            cur.visit();
            totalVisited++;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            cur.BackColor = Color.Green;
            if (totalVisited < cols * rows)
            {
                Neighbours();
                if (n.Count > 0)
                {
                    Random rnd = new Random();
                    int r = rnd.Next(n.Count);
                    MyPanel chosen = n[r];
                    stack.Push(cur);
                    DeleteWall(chosen);
                    cur = chosen;
                    cur.visit();
                    totalVisited++;
                }
                else
                {
                    cur = stack.Pop();
                    cur.BackColor = Color.Red;
                }
            }
        }

        private void DeleteWall(MyPanel p)
        {
            if (cur.col == p.col)
            {
                if (cur.row < p.row)
                { 
                    p.Margin = new Padding(p.Margin.Left, 0, p.Margin.Right, p.Margin.Bottom);
                    cur.Margin = new Padding(cur.Margin.Left, cur.Margin.Top, cur.Margin.Right, 0);
                }
                else
                {
                    p.Margin = new Padding(p.Margin.Left, p.Margin.Top, p.Margin.Right, 0);
                    cur.Margin = new Padding(cur.Margin.Left, 0, cur.Margin.Right, cur.Margin.Bottom);
                }
            }
            else
            {
                if (cur.col < p.col)
                {
                    p.Margin = new Padding(0, p.Margin.Top, p.Margin.Right, p.Margin.Bottom);
                    cur.Margin = new Padding(cur.Margin.Left, cur.Margin.Top, 0, cur.Margin.Bottom);
                }
                else
                {
                    p.Margin = new Padding(p.Margin.Left, p.Margin.Top, 0, p.Margin.Bottom);
                    cur.Margin = new Padding(0, cur.Margin.Top, cur.Margin.Right, cur.Margin.Bottom);
                }
            }
        }


        private void Neighbours()
        {
            n.Clear();
            if(cur.col!=0 && cur.col != cols - 1)
            {
                if (!panels[cur.col + 1, cur.row].visited)
                    n.Add(panels[cur.col + 1, cur.row]);
                if (!panels[cur.col - 1, cur.row].visited)
                    n.Add(panels[cur.col - 1, cur.row]);
            }
            else
            {
                if (cur.col == 0)
                {
                    if (!panels[cur.col + 1, cur.row].visited)
                        n.Add(panels[cur.col + 1, cur.row]);
                }
                else
                {
                    if (!panels[cur.col - 1, cur.row].visited)
                        n.Add(panels[cur.col - 1, cur.row]);
                }
            }
            if(cur.row!=0 && cur.row != rows - 1)
            {
                if (!panels[cur.col, cur.row+1].visited)
                    n.Add(panels[cur.col, cur.row+1]);
                if (!panels[cur.col, cur.row - 1].visited)
                    n.Add(panels[cur.col, cur.row - 1]);
            }
            else
            {
                if (cur.row == 0)
                {
                    if (!panels[cur.col, cur.row + 1].visited)
                        n.Add(panels[cur.col, cur.row + 1]);
                }
                else
                {
                    if (!panels[cur.col, cur.row - 1].visited)
                        n.Add(panels[cur.col, cur.row - 1]);
                }
            }
        }
    }
    public class MyPanel : Panel
    {
        public bool visited = false;
        public int row;
        public int col;
        public void visit()
        {
            visited = true;
            this.BackColor = Color.Red;
        }
    }
}
