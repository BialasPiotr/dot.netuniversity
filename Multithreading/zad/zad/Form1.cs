using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace zad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(RunThread1);
            thread1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread2 = new Thread(RunThread2);
            thread2.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread thread3 = new Thread(RunThread3);
            thread3.Start();
        }

        private long CalculateFactorial(int n)
        {
            long result = 1;
            for (int i = 1; i <= n; i++)
            {
                result *= i;
                Thread.Sleep(100); // Symulacja dłuższego czasu obliczeń
            }
            return result;
        }

        private long CalculateFibonacci(int n)
        {
            long a = 0;
            long b = 1;

            for (int i = 0; i < n; i++)
            {
                long temp = a;
                a = b;
                b = temp + b;
                Thread.Sleep(100); // Symulacja dłuższego czasu obliczeń
            }
            return a;
        }

        private long CalculateSumOfSquares(int n)
        {
            long sum = 0;
            for (int i = 1; i <= n; i++)
            {
                sum += i * i;
                Thread.Sleep(100); // Symulacja dłuższego czasu obliczeń
            }
            return sum;
        }

        private void RunThread1()
        {
            long result = CalculateFactorial(10);
            Invoke((Action)(() => label1.Text = $"Wątek 1: wynik {result}"));
        }

        private void RunThread2()
        {
            long result = CalculateFibonacci(20);
            Invoke((Action)(() => label2.Text = $"Wątek 2: wynik {result}"));
        }

        private void RunThread3()
        {
            long result = CalculateSumOfSquares(20);
            Invoke((Action)(() => label3.Text = $"Wątek 3: wynik {result}"));
        }
    }
}
