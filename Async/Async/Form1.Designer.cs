namespace Async
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Label label1;
        private Label label2;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // textBox1
            this.textBox1.Location = new System.Drawing.Point(50, 40);
            this.textBox1.Size = new System.Drawing.Size(150, 23);
            this.textBox1.Text = "10000000";

            // textBox2
            this.textBox2.Location = new System.Drawing.Point(50, 90);
            this.textBox2.Size = new System.Drawing.Size(150, 23);
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);

            // button1
            this.button1.Location = new System.Drawing.Point(250, 40);
            this.button1.Size = new System.Drawing.Size(120, 23);
            this.button1.Text = "Sieb ohne Thread";
            this.button1.Click += new System.EventHandler(this.button1_Click);

            // button2
            this.button2.Location = new System.Drawing.Point(250, 70);
            this.button2.Size = new System.Drawing.Size(120, 23);
            this.button2.Text = "Sieb mit Thread";
            this.button2.Click += new System.EventHandler(this.button2_Click);

            // button3
            this.button3.Location = new System.Drawing.Point(250, 100);
            this.button3.Size = new System.Drawing.Size(150, 23);
            this.button3.Text = "Sieb mit Backgroundworker";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            
            // button4
            this.button4.Location = new System.Drawing.Point(250, 130);
            this.button4.Size = new System.Drawing.Size(120, 23);
            this.button4.Text = "Asynchrones Sieb";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            
            // label1
            this.label1.Location = new System.Drawing.Point(420, 70);
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.Text = "label1";
             
            // label2
            this.label2.Location = new System.Drawing.Point(50, 130);
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.Text = "label2";
            
            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 300);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Name = "Form1";
            this.ResumeLayout(false);
        }
    }
}
