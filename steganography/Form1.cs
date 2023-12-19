using System.Drawing;
using System.Windows.Forms;

namespace picture
{
    public partial class Form1 : Form
    {
        OpenFileDialog ofd = new OpenFileDialog();
        Image img;
        Bitmap originalImg;
        Bitmap modyfiedImg;

        bool loadFileRes = false;
        bool encrButtonClicked = false;

        // ����� �������� ��� ������ �� ����������� ���-�� �������� �����������
        // (�� ����� ���� ������� ������� � 4 ���� ������)
        int textLen = 0, pixelsNum = 0;
        char[] symbolsToEncr;
        char[] decryptedSymbols;

        // �������� ���������� ������ �����������, ����� ����� ����� ���� ������������ ����� ����� �������� XOR
        int[] origA, origR, origG, origB;

        // ������������� ������� openFileDialog1 (� ���������� ������� ��� ��������)
        public Form1()
        {
            InitializeComponent();
            ofd = openFileDialog1;
            ofd.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";
        }

        // ����� ������� � ���������
        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("��� ���������� ���������� ���������� ������ �������������." +
                            "������������� - ���������� ��������� ����� �������� ���������." +
                            "� ������ ������ ������� ����� ��������� ����� ��������." +
                            "����� ����� ������� �����. ����� ���� ����� ����� ������ � ������� ���������." +
                            "��������� ������� ����� �� ��������, �� �� ���������������� �������� ����� ������������ �����." +
                            "!!! ������ ����� ��� ������� ���� !!!");
        }

        // �������� ��������
        private void button1_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                loadFileRes = true;
                img = Image.FromFile(ofd.FileName);
                originalImg = new Bitmap(img);
                modyfiedImg = new Bitmap(img);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = img;
                pixelsNum = originalImg.Width * originalImg.Height;
            }
            else
            {
                loadFileRes = false;
                MessageBox.Show("������ �������� �����������");
            }
        }

        // ������������ ������ � ������� ��������
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || loadFileRes == false)
            {
                encrButtonClicked = false;
                MessageBox.Show("����� ��������� ����������� � ������ ����� ��� ����������");
                return;
            }

            // ����������� ����� ������ ��������� ������ <= pixelsNum
            encrButtonClicked = true;
            textLen = textBox1.TextLength > pixelsNum ? pixelsNum : textBox1.TextLength;
            symbolsToEncr = new char[textLen];
            symbolsToEncr = textBox1.Text.Substring(0, textLen).ToCharArray();

            // ���������� ������������ ��������� �����, ��� ���������� ������ �� �������� �����������
            origR = new int[pixelsNum];
            origG = new int[pixelsNum];
            origB = new int[pixelsNum];
            origA = new int[pixelsNum];

            int counter = 0; // ������� ���������� �������� (���������������� ��������)
            for (int y = 0; y < originalImg.Height; y++)
            {
                for (int x = 0; x < originalImg.Width; x++)
                {
                    Color pixelColor = originalImg.GetPixel(x, y);
                    origR[counter] = pixelColor.R;
                    origG[counter] = pixelColor.G;
                    origB[counter] = pixelColor.B;
                    origA[counter] = pixelColor.A;

                    if (counter < textLen)
                    {
                        // ������ ������ '�����������' � alpha ����� ����� �����������
                        int code = (int)symbolsToEncr[counter];
                        Color newColor = Color.FromArgb(origA[counter] ^ code, origR[counter],
                                                        origG[counter], origB[counter]);
                        modyfiedImg.SetPixel(x, y, newColor);
                        counter += 1;
                    }
                    else
                        modyfiedImg.SetPixel(x, y, originalImg.GetPixel(x, y));
                }
            }
        }

        // ����������� ������ �� �������� ����������������� �����������
        // (�������� ���������� �������� ��������� ��������� XOR � �������� ������ ���� ��������) 
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || loadFileRes == false || encrButtonClicked == false)
            {
                MessageBox.Show("����� ��������� �����������, ������ ����� ��� ���������� � " +
                                "����������� ���");
                return;
            }

            // ����������� ����������� � '�����������' � ���� �������
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Image = modyfiedImg;

            decryptedSymbols = new char[textLen];

            int counter = 0; // ������� ��������������� �������� (�� ��������)
            for (int y = 0; y < modyfiedImg.Height; y++)
            {
                for (int x = 0; x < modyfiedImg.Width; x++)
                {
                    Color pixelColor = modyfiedImg.GetPixel(x, y);

                    if (counter < textLen)
                    {
                        decryptedSymbols[counter] = (char)(pixelColor.A ^ origA[counter]);
                        counter += 1;
                    }
                    else
                        break;
                }
            }

            string str = new string(decryptedSymbols);
            textBox2.Text = str;
        }
    }
}