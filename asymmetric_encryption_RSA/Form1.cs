using System.Numerics;

namespace ___RSA___
{
    public partial class Form1 : Form
    {
        // Отрицательные значения 'e', 'd' это признаки того, что они ещё не инициализированы
        BigInteger p, q, n, phi, e = -1, d = -1;

        int[] codes_1; // Коды символов исходного сообщения
        BigInteger[] Codes; // Зашифрованные коды исходного сообщения
        int[] codes_2; // Результат расшифровки

        // Расширенный алгоритм Евклида, используется для нахождения числа 'd' в закрытом ключе
        // a = e, t = d, n = phi
        BigInteger Inverse(BigInteger e, BigInteger phi)
        {
            BigInteger d = 0;
            BigInteger new_d = 1;
            BigInteger r = phi;
            BigInteger new_r = e;

            while (new_r != 0)
            {
                BigInteger q = r / new_r;

                BigInteger old_d = new_d;
                new_d = d - q * new_d;
                d = old_d;

                BigInteger old_r = new_r;
                new_r = r - q * new_r;
                r = old_r;
            }

            if (r > 1)
            {
                MessageBox.Show("Ошибка при генерации ключей");
                return -1;
            }
            if (d < 0)
                d = d + phi;
            return d;
        }

        private void label1_Click(object sender, EventArgs E) { }
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs E)
        {
        }


        // Генерация ключей: (e, n) -- открытый, (d, n) -- закрытый
        private void button1_Click(object sender, EventArgs E)
        {
            // p = genPseduoPrime();
            // q = genPseudoPrime(); do while (p == q)
            p = 3571; //3571
            q = 3559; //3559
            n = p * q;
            phi = (p - 1) * (q - 1);

            e = 3;
            while (BigInteger.GreatestCommonDivisor(e, phi) != 1)
                e += 2;

            d = Inverse(e, phi); // Расширенный алгоритм Евклида

            // Вывод ключей (e, n) и (d, n)
            textBox4.Clear();
            textBox4.Text = "e = " + Convert.ToString(e) + Environment.NewLine + "n = " + Convert.ToString(n);
            textBox5.Clear();
            textBox5.Text = "d = " + Convert.ToString(d) + Environment.NewLine + "n = " + Convert.ToString(n);
        }


        // Шифрование
        private void button2_Click(object sender, EventArgs E)
        {
            if (e < 0 && d < 0 || textBox1.Text.Length == 0)
            {
                MessageBox.Show("Нужно сгенерировать ключи и написать текст для шифрования");
                return;
            }

            // Сообщение (исходный текст) в виде набора кодов
            int textLen = textBox1.Text.Length;
            codes_1 = new int[textLen];
            for (int i = 0; i < textLen; i++)
                codes_1[i] = Convert.ToInt16(textBox1.Text[i]);

            // Зашифрованное сообщение в виде C_i = (c_i)^e % n, где C_i зашифрованный i-й код (c_i)
            Codes = new BigInteger[textLen];
            for (int i = 0; i < textLen; i++)
                Codes[i] = BigInteger.ModPow(codes_1[i], e, n);

            // Вывод зашифрованного сообщения
            // Каждому символу отдельная строка
            textBox2.Clear();
            for (int i = 0; i < textLen; i++)
                textBox2.Text += codes_1[i].ToString() + "   [" + textBox1.Text[i] + "]" + Environment.NewLine;
        }


        // Расшифрование
        private void button3_Click(object sender, EventArgs E)
        {
            if (e < 0 && d < 0 || textBox1.Text.Length == 0 || textBox2.Text.Length == 0)
            {
                MessageBox.Show("Нужно сгенерировать ключи, написать текст для шифрования и зашифровать его");
                return;
            }

            // Расшифровка и вывод сообщения посимвольно
            int textLen = textBox1.Text.Length;
            codes_2 = new int[textLen];
            textBox3.Clear();
            for (int i = 0; i < textLen; i++)
            {
                codes_2[i] = (int)BigInteger.ModPow(Codes[i], d, n);
                textBox3.Text += Convert.ToString(Convert.ToChar(codes_2[i])); // Число в char, char в символ строки вывода
            }
        }
    }
}