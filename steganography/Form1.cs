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

        // Число символов для записи не превосходит кол-ва пикселей изображения
        // (на самом деле верхняя граница в 4 раза больше)
        int textLen = 0, pixelsNum = 0;
        char[] symbolsToEncr;
        char[] decryptedSymbols;

        // Исходные компоненты цветов сохраняются, чтобы потом можно было ввостановить текст через операцию XOR
        int[] origA, origR, origG, origB;

        // Инициализация объекта openFileDialog1 (с установкой фильтра для картинок)
        public Form1()
        {
            InitializeComponent();
            ofd = openFileDialog1;
            ofd.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";
        }

        // Вывод справки о программе
        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Это приложение показывает простейший пример стеганографии." +
                            "Стеганография - занимается сокрытием факта передачи сообщения." +
                            "В данном случае сначала нужно загрузить любую картинку." +
                            "Затем нужно вписать текст. Далее этот текст будет внедрён в пиксели какртинки." +
                            "Визуально разница будет не заментна, но из модифицированной картники можно восстановить текст." +
                            "!!! Писать текст без русских букв !!!");
        }

        // Загрузка картинки
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
                MessageBox.Show("Ошибка открытия изображения");
            }
        }

        // Зашифрование текста в пискели картинки
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || loadFileRes == false)
            {
                encrButtonClicked = false;
                MessageBox.Show("Нужно загрузить изображение и ввести текст для шифрования");
                return;
            }

            // Зашифровать можно только сообщение длиной <= pixelsNum
            encrButtonClicked = true;
            textLen = textBox1.TextLength > pixelsNum ? pixelsNum : textBox1.TextLength;
            symbolsToEncr = new char[textLen];
            symbolsToEncr = textBox1.Text.Substring(0, textLen).ToCharArray();

            // Сохранение оригинальных компонент цвета, для дешифровки данных из пикселей изображения
            origR = new int[pixelsNum];
            origG = new int[pixelsNum];
            origB = new int[pixelsNum];
            origA = new int[pixelsNum];

            int counter = 0; // Счётчик записанных символов (модифицированных пикселей)
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
                        // Данные текста 'добавляются' в alpha канал цвета изображения
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

        // Расшифровка текста из пикселей модифицированного изображения
        // (исходные компоненты пикселей удаляются операцией XOR и остаются только коды символов) 
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || loadFileRes == false || encrButtonClicked == false)
            {
                MessageBox.Show("Нужно загрузить изображение, ввести текст для шифрования и " +
                                "зашифровать его");
                return;
            }

            // Отображение изображения с 'добавленным' в него текстом
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Image = modyfiedImg;

            decryptedSymbols = new char[textLen];

            int counter = 0; // Счётчик расшифрованнных символов (из пикселей)
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