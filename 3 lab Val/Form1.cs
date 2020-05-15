using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3_lab_Val
{
    public partial class Form1 : Form
    {
        private int q = 19;
        private int p = 17;
        private int A = 2;
        private int B = 2;
        private int[] G = new int[2] { 5, 1 };
        private int[] R;
        private int[] QA;
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = q.ToString();
            textBox2.Text = p.ToString();
            textBox8.Text = A.ToString();
            textBox9.Text = B.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int k = new Random().Next(1, q - 1);
            R = pointOnValue(G, k);
            int dA = new Random().Next(1, q - 1);
            QA = pointOnValue(G, dA);
            int r = R[0] % q;
            string stingIn = textBox3.Text.ToString();

            int eVal = 0;
            for (int i = 0; i < stingIn.Length; i++)
            {
                eVal += (int)stingIn[i];
            }
            eVal %= 256;

            int s = (modInverse(k, q) * (eVal + dA * r)) % q;

            textBox4.Text = r + "|" + s;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] rsArray = textBox6.Text.Split('|');
            int r = Convert.ToInt32(rsArray[0]);
            int s = Convert.ToInt32(rsArray[1]);
            string stingIn = textBox5.Text.ToString();

            int eVal = 0;
            for (int i = 0; i < stingIn.Length; i++)
            {
                eVal += (int)stingIn[i];
            }
            eVal %= 256;

            int w = modInverse(s, q);
            int u1 = (w * eVal) % q;
            int u2 = (w * r) % q;
            int[] uP = pointOnValue(G, u1);
            int[] uQ = pointOnValue(QA, u2);
            int[] point = pointPlusPoint(uP, uQ);
            int u = point[0] % q;

            if (u == r)
            {
                textBox7.Text = "Сообщение прошло проверку";
            }
            else
            {
                textBox7.Text = "Сообщение не прошло проверку";
            }
        }

        int modInverse(int a, int n)
        {
            int i = n, v = 0, d = 1;
            while (a > 0)
            {
                int t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }


        public int[] pointPlusPoint(int[] firstPoint, int[] secondPoint)
        {
            int m;
            if (firstPoint[0] == secondPoint[0] && firstPoint[1] == secondPoint[1])
            {
                m = (int)((3 * firstPoint[0] * firstPoint[0] + A) * (int)(BigInteger.ModPow((2 * firstPoint[1]), p - 2, p)));
            }
            else
            {
                m = ((secondPoint[1] - firstPoint[1]) * (int)(BigInteger.ModPow((secondPoint[0] - firstPoint[0]), p - 2, p)));
            }

            int x = (int)BigInteger.ModPow((int)(Math.Pow(m, 2) - firstPoint[0] - secondPoint[0]),1, p);

            int y = (int)BigInteger.ModPow(((-firstPoint[1] + m * (firstPoint[0] - x))),1, p);
            return new int[2] { x, y };

        }

        public int[] pointOnValue(int[] firstPoint, int value)
        {
            if (value == 1) return firstPoint;
            int[] secondPoint = pointPlusPoint(firstPoint, firstPoint);
            for (int i = 1; i < value - 1; i++)
            {
                int[] thPoint = pointPlusPoint(firstPoint, secondPoint);
                secondPoint = new int[2] { thPoint[0], thPoint[1] };
            }
            return secondPoint;
        }
    }
}
