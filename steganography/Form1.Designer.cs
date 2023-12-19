namespace picture
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            openFileDialog1 = new OpenFileDialog();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            pictureBox2 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            button4 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 30);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(360, 240);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // button1
            // 
            button1.Location = new Point(396, 30);
            button1.Name = "button1";
            button1.Size = new Size(202, 62);
            button1.TabIndex = 1;
            button1.Text = "Загрузить картинку";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(731, 504);
            button2.Name = "button2";
            button2.Size = new Size(154, 56);
            button2.TabIndex = 2;
            button2.Text = "Зашифровать";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(891, 504);
            button3.Name = "button3";
            button3.Size = new Size(149, 56);
            button3.TabIndex = 3;
            button3.Text = "Расшифровать";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(731, 30);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(309, 468);
            textBox1.TabIndex = 4;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(1057, 30);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(309, 468);
            textBox2.TabIndex = 5;
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new Point(12, 306);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(360, 240);
            pictureBox2.TabIndex = 6;
            pictureBox2.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(138, 15);
            label1.TabIndex = 7;
            label1.Text = "Исходное изображение";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 288);
            label2.Name = "label2";
            label2.Size = new Size(198, 15);
            label2.TabIndex = 8;
            label2.Text = "Модифицированное изображение";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(731, 12);
            label3.Name = "label3";
            label3.Size = new Size(205, 15);
            label3.TabIndex = 9;
            label3.Text = "Текст для внедрения в изображение";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(1057, 9);
            label4.Name = "label4";
            label4.Size = new Size(204, 15);
            label4.TabIndex = 10;
            label4.Text = "Текст, полученный из изображения";
            // 
            // button4
            // 
            button4.Location = new Point(1057, 501);
            button4.Name = "button4";
            button4.Size = new Size(309, 62);
            button4.TabIndex = 11;
            button4.Text = "О программе";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1587, 577);
            Controls.Add(button4);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox2);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private OpenFileDialog openFileDialog1;
        private Button button1;
        private Button button2;
        private Button button3;
        private TextBox textBox1;
        private TextBox textBox2;
        private PictureBox pictureBox2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button button4;
    }
}