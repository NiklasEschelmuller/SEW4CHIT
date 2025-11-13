using System;
using System.Diagnostics.CodeAnalysis;
namespace KranFroms;

public partial class Form1 : Form
{
private readonly MaschieneA maschineA = new();
        private readonly MaschieneB maschineB = new();
        private readonly Crane crane;

        public int werkstueckX = 50; // X-Position Werkstück
        public int werkstueckY = 150; // Y-Position Werkstück
        public int kranX = 50;        // X-Position Kran
        private int kranY = 80;        // Y-Position Kran
        private string currentStep = "Warte...";
        private bool werkstueckVisible;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Width = 800;
            Height = 300;
            Text = "Fabrik Animation mit Kran";

            crane = new Crane(maschineA, maschineB, this);

            Button startButton = new Button { Text = "Start", Location = new Point(10, 10) };
            startButton.Click += StartButton_Click;
            Controls.Add(startButton);
        }

        [AllowNull] public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
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
            g.DrawString("Lager 1", Font, Brushes.White, 50, 180);

            // Maschine A
            g.FillRectangle(Brushes.Blue, 250, 200, 80, 50);
            g.DrawString("Maschine A", Font, Brushes.White, 250, 180);

            // Maschine B
            g.FillRectangle(Brushes.Green, 450, 200, 80, 50);
            g.DrawString("Maschine B", Font, Brushes.White, 450, 180);

            // Lager 2
            g.FillRectangle(Brushes.Gray, 650, 200, 80, 50);
            g.DrawString("Lager 2", Font, Brushes.White, 650, 180);

            // Kran
            g.FillRectangle(Brushes.Gold, kranX, kranY, 100, 20);

            // Werkstück
            if (werkstueckVisible)
            {
                g.FillRectangle(Brushes.Red, werkstueckX, werkstueckY, 30, 30);
            }

            // Statusanzeige
            g.DrawString(currentStep, this.Font, Brushes.Black, 100, 10);
        }

        // Animation-Methode für Werkstück + Kran
        public void AnimateWerkstueck(int startX, int endX, string step)
        {
            werkstueckVisible = true;
            currentStep = step;

            int direction = startX < endX ? 1 : -1;
            while ((direction == 1 && werkstueckX < endX) || (direction == -1 && werkstueckX > endX))
            {
                werkstueckX += direction * 5;
                kranX = werkstueckX - 35; // Kran leicht vor das Werkstück setzen
                Invoke(Invalidate);
                Thread.Sleep(30);
            }

            werkstueckX = endX;
            kranX = werkstueckX - 35;
            Invoke(Invalidate);
        }

        public void SetStepText(string text)
        {
            currentStep = text;
            Invoke(Invalidate);
        }
}