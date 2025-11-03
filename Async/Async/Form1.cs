using System.ComponentModel;
using System.Threading;

namespace Async
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Üperpüfung ob GUI während Berechnung einfriert oder nicht
            label2.Text = textBox2.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "...calculating...";
            int upto = int.Parse(textBox1.Text);
            int result;
            Sieve(upto, out result); // blockiert das GUI
            label1.Text = result.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "...calculating...";
            int upto = int.Parse(textBox1.Text);
            int result = 0;
            Thread t = new Thread(() => Sieve(upto, out result));
            t.Start();
            t.Join(); // Warten auf Ergebnis blockiert auch das GUI
            label1.Text = result.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = "...calculating...";
            int upto = int.Parse(textBox1.Text);

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                int r;
                Sieve(upto, out r);
                e.Result = r;
            };
            worker.RunWorkerCompleted += (s, e) =>
            {
                label1.Text = e.Result.ToString();
            };
            worker.RunWorkerAsync();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            label1.Text = "...calculating...";
            int upto = int.Parse(textBox1.Text);
            int result = 0;
            await Task.Run(() => Sieve(upto, out result)); // nicht blockierend
            label1.Text = result.ToString();
        }

        // Sieve of Eratosthenes
        public void Sieve(int n, out int result)
        {
            bool[] isPrime = new bool[n + 1];
            for (int i = 2; i <= n; i++)
            {
                isPrime[i] = true;
            }

            for (int i = 2; i * i <= n; i++)
            {
                if (isPrime[i])
                {
                    for (int j = i * i; j <= n; j += i)
                    {
                        isPrime[j] = false;
                    }
                }
            }

            result = isPrime.Count(x => x);
        }
    }
}
