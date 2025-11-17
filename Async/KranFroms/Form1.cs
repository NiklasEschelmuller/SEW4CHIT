using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace KranFroms;

     public partial class Form1 : Form
    {
        // Werkstück
        public int werkstueckX = 50;
        public int werkstueckY = 150;
        public Color werkstueckColor = Color.Red;

        // Kran
        public int kranX = 50;
        public int kranY = 80;

        // Maschinen
        public int maschieneAY = 200;
        public int maschieneBY = 200;

        private readonly MaschieneA maschieneA = new();
        private readonly MaschieneB maschieneB = new();
        private readonly Crane crane;

        private string currentStep = "Warte...";
        private bool werkstueckVisible = true;

        public SemaphoreSlim semaphore = new(1,1); // nur ein Thread darf das Werkstück

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Width = 800;
            this.Height = 300;
            this.Text = "Fabrik Animation Kran & Maschinen";

            crane = new Crane(maschieneA, maschieneB, this);

            Button startButton = new Button { Text = "Start", Location = new Point(10, 10) };
            startButton.Click += StartButton_Click;
            Controls.Add(startButton);
        }

        private void StartButton_Click(object? sender, EventArgs e)
        {
            Task.Run(() => crane.Run());
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Lager 1
            g.FillRectangle(Brushes.Gray, 50, 200, 80, 50);
            g.DrawString("Lager 1", this.Font, Brushes.White, 50, 180);

            // MaschieneA
            g.FillRectangle(Brushes.Blue, 250, maschieneAY, 80, 50);
            g.DrawString("MaschieneA", this.Font, Brushes.White, 250, maschieneAY - 20);

            // MaschieneB
            g.FillRectangle(Brushes.Green, 450, maschieneBY, 80, 50);
            g.DrawString("MaschieneB", this.Font, Brushes.White, 450, maschieneBY - 20);

            // Lager 2
            g.FillRectangle(Brushes.Gray, 650, 200, 80, 50);
            g.DrawString("Lager 2", this.Font, Brushes.White, 650, 180);

            // Kran
            g.FillRectangle(Brushes.Gold, kranX, kranY, 100, 20);

            // Werkstück
            if (werkstueckVisible)
            {
                using (Brush b = new SolidBrush(werkstueckColor))
                    g.FillRectangle(b, werkstueckX, werkstueckY, 30, 30);
            }

            // Status
            g.FillRectangle(Brushes.White, 5, 45, 400, 30);
            g.DrawString(currentStep, this.Font, Brushes.Black, 10, 50);
        }

        public void AnimateKranX(int targetX, string step)
        {
            currentStep = step;
            int direction = targetX > kranX ? 1 : -1;
            while ((direction == 1 && kranX < targetX) || (direction == -1 && kranX > targetX))
            {
                kranX += direction * 5;
                werkstueckX = kranX + 35;
                Invoke(new Action(Invalidate));
                Thread.Sleep(30);
            }
            kranX = targetX;
            werkstueckX = kranX + 35;
            Invoke(new Action(Invalidate));
        }

        public void AnimateKranY(int targetY, string step)
        {
            currentStep = step;
            int direction = targetY > kranY ? 1 : -1;
            while ((direction == 1 && kranY < targetY) || (direction == -1 && kranY > targetY))
            {
                kranY += direction * 5;
                werkstueckY = kranY + 35;
                Invoke(new Action(Invalidate));
                Thread.Sleep(30);
            }
            kranY = targetY;
            werkstueckY = kranY + 35;
            Invoke(new Action(Invalidate));
        }

        public void AnimateMaschineY(ref int maschineY, int targetY, string step)
        {
            currentStep = step;
            int direction = targetY > maschineY ? 1 : -1;
            while ((direction == 1 && maschineY < targetY) || (direction == -1 && maschineY > targetY))
            {
                maschineY += direction * 5;
                Invoke(new Action(Invalidate));
                Thread.Sleep(30);
            }
            maschineY = targetY;
            Invoke(new Action(Invalidate));
        }

        public void SetStepText(string text)
        {
            currentStep = text;
            Invoke(new Action(Invalidate));
        }

        public void SetWerkstueckColor(Color color)
        {
            werkstueckColor = color;
            Invoke(new Action(Invalidate));
        }
    }