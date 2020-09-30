using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 

namespace lab03_3
{
    public partial class Form1 : Form
    {
        Graphics g1, g2;
        int x0, y0;
        Bitmap b;
        bool flag;
        

        public Form1()
        {
            InitializeComponent();
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g1 = this.pictureBox1.CreateGraphics();
            g2 = this.pictureBox2.CreateGraphics();
           
        }

        //
        public Bitmap DrawLine(Bitmap bitmap, int x0, int y0, int x1, int y1, Color color)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;
            for (; ; )
            {
                bitmap.SetPixel(x0, y0, color);
                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
            }

            return bitmap;
        }


        void Checker(MouseEventArgs e)
        {
            if (flag == false)
            {
                x0 = e.X;
                y0 = e.Y;
                flag = true;
            }
            else
            {
                flag = false;
                pictureBox1.Image = DrawLine(b, x0, y0, e.X, e.Y, Color.Green);
                WuLine(x0, y0, e.X, e.Y);
                

            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Pen pen = new Pen(Color.Red);
            SolidBrush solid = new SolidBrush(Color.Red);
            g1.FillEllipse(solid, e.X, e.Y, 5, 5);
            g1.DrawEllipse(pen, e.X, e.Y, 5, 5);

            Checker(e);

            solid.Dispose();
            pen.Dispose();
        }


        private void DrawPoint(bool steep, int x, int y, double grade)
        {
            grade = 1 - grade;
            Bitmap bmp = new Bitmap(1, 1);
            bmp.SetPixel(0, 0, Color.FromArgb((int)(255 * grade), (int)(255 * grade), (int)(255 * grade)));
            if (steep)
                g2.DrawImageUnscaled(bmp, y, x);
            else
                g2.DrawImageUnscaled(bmp, x, y);
        }
       
        private void WuLine(int x0, int y0, int x1, int y1)
        {
            var steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            DrawPoint(steep, x0, y0, 1); // Эта функция автоматом меняет координаты местами в зависимости от переменной steep
            DrawPoint(steep, x1, y1, 1); // Последний аргумент — интенсивность в долях единицы
            float dx = x1 - x0;
            float dy = y1 - y0;
            float gradient = dy / dx;
            float y = y0 + gradient;
            for (var x = x0 + 1; x <= x1 - 1; x++)
            {
                DrawPoint(steep, x, (int)y, 1 - (y - (int)y));
                DrawPoint(steep, x, (int)y + 1, y - (int)y);
                y += gradient;
            }
        }

        

        public static void Swap(ref int a, ref int b)
        {
            int c = a;
            a = b;
            b = c;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            g1.Clear(BackColor);
            g2.Clear(BackColor);

        }
    }
}
