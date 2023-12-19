/*

 Исходный код, который реализует упрощенный вариант
 блочного симметричного алгоритма шифрования ГОСТ 28139-89 (режим простой замены)
 смотри: https://ru.wikipedia.org/wiki/%D0%93%D0%9E%D0%A1%D0%A2_28147-89

*/


#include <cstdio>
#include <cstring>

#define GOST_ENC true
#define GOST_DEC false

// Переводит набор символов (char'ов) в 64 бита, начиная с символа с индексом 'begin'
bool * charsToBlock(char const * const chars, int begin)
{
    bool * block = new bool [64];
    for(int byte = 0; byte < 8; byte++)
    {
        char symbol = chars[begin + byte];
        for(int bit = 7; bit >= 0; bit--) // Биты записываются от младшего к старшему
        {
            block[8*byte + bit] = symbol % 2;
            symbol = symbol>>1;
        }

        /* //Вывод для отладки (порядок битов как положено, самый правый - самый младший)
        for(int bit = 0; bit < 8; bit++)
            printf("%d", block[8*byte + bit]);
        printf("\n");
        */
    }
    return block;
}


// Переводит 64 бита (блок) в массив из 8 символов (char'ов), нультерминатор ('\0') не ставится
char * blockToChars(bool const * const block)
{
    char * chars = new char [8];
    for(int byte = 0; byte < 8; byte++)
    {
        char symbol = 0;
        for(int bit = 7; bit >= 0; bit--)
            symbol += (block[8*byte + bit])<<(7 - bit); // От младшего к старшему
        chars[byte] = symbol;
        //printf("%c", symbol); // Вывод перевёденного символа для отладки
    }
    return chars;
}


// Функция GOST реализует алгоритм ГОСТ (режим простой замены) 
// Шифруется / расшифровывается 1 (ОДИН) переданный блок 
// Он основан на сети Фейстеля как для шифрования, так и для расшифрования
// см. https://ru.wikipedia.org/wiki/%D0%93%D0%9E%D0%A1%D0%A2_28147-89

// Для шифрования порядок подключей и половин блока прямой
// Для расшифрования порядок ключей обратный
// Также при расшифровке на первом и последнем этапах половины блока переставляются

bool * GOST(bool const * const block, bool mode)
{
    // Параметры алгоритма (ключ и таблица замен) берутся из файла "GOST_Parametres.C"
    #include "GOST_Parameters.CPP" // Ключ[8][32] и таблица замен T[16][4]
    bool * block1 = new bool [64];
    bool * block2 = new bool [64];
    if(mode == GOST_DEC) // Перестановка половин блоков при расшифровке
        for(int bit = 0; bit < 32; bit++)
        {
            block1[bit] = block2[bit] = block[32 + bit];
            block1[32 + bit] = block2[32 + bit] = block[bit];
        }
    else 
        for(int bit = 0; bit < 64; bit++)
            block1[bit] = block2[bit] = block[bit];
    
    /* // Отладочный вывод (проверка порядка следования половин блока)
    for(int byte = 0; byte < 8; byte++)
    {
        for(int bit = 7; bit >= 0; bit--)
            printf("%d", block1[8*byte + bit]);
        printf("\n");
    }
    */

    
    // <<<===== Сеть Фейстеля  

    // L[r] = R[r-1];   R[r] = L[r-1] XoR F(R[r-1], K[r-1]);  r - номер раунда
    // Здесь L[r-1], R[r-1] это части массива block1
    // Здесь L[r], R[r] это части массива block2
    int rounds = 8;
    for(int r = 0; r < rounds; r++) // r < 8!!!
    {
        // Преобразование левого подблока (L[r] = R[r-1])
        for(int bit = 0; bit < 32; bit++)
            block2[bit] = block1[32 + bit];



        // <<<===== Преобразование правого подблока ( F(R[r-1], K[r-1]) )
        // Учёт обратного порядка ключей при расшифровке
        int subKey_idx = (mode == GOST_DEC) ? rounds - 1 - r : r; 

        // Шаг I. Сложение по модулю 2^(32): R[r-1] '+' K[r-1] 
        //(фактически отбрасывание переполнения в последнем разряде)
        for(int bit = 0; bit < 32; bit++)
        {
            bool overflow = false;
            block2[32 + bit] = block2[32 + bit] ^ K[subKey_idx][bit] ^ overflow;
            overflow = ((block2[32 + bit] + K[subKey_idx][bit] + overflow) > 1) ? true : false;
        }

        // Шаг II. Табличная замена (для преобразованного правого под блока R*[r-1])
        for(int subBlock = 0; subBlock < 8; subBlock++)
        {
            int row_idx = block2[32 + 4*subBlock + 3] * 8 
                        + block2[32 + 4*subBlock + 2] * 4
                        + block2[32 + 4*subBlock + 1] * 2
                        + block2[32 + 4*subBlock + 0] * 1;
            for(int bit = 0; bit < 4; bit++) // Замена на заданную строку
                block2[32 + 4*subBlock + bit] = T[row_idx][bit];
        }

        // Шаг III. Циклический сдвиг на 11 битов в сторону страшего разряда
        // (для преобразованного правого под блока R*[r-1])
        // ! Тут нужна копия правой части блока !
        bool block2_R_copy[32];
        for(int bit = 0; bit < 32; bit++)
            block2_R_copy[bit] = block2[32 + bit];
        for(int bit = 0; bit < 32; bit++)
        {
            bool buf = block2_R_copy[32 + bit];
            if(bit <= 20) //[0 : 20] ---> [11: 31] (индексы "правой половины"
            {
                block2[32 + bit] = block2_R_copy[bit + 11];
                block2[32 + bit + 11] = buf; 
            }
            else //[21 : 31] ---> [0 : 10]
            {
                block2[32 + bit] = block2_R_copy[11 - (31 - bit)];
                block2[32 + 10 - (31 - bit)] = buf; 
            }
        }
        // Преобразование правого подблока ( F(R[r-1], K[r-1]) ) 

        // Последний этап преобразования правого подблока (L[r-1] Xor F(R[r-1], K[r-1]) )
        for(int bit = 0; bit < 32; bit++)
            block2[32 + bit] = block1[bit] ^ block2[32 + bit]; 
        


        // Обновление данных перед следующим раундом
        for(int bit = 0; bit < 64; bit++)
            block1[bit] = block2[bit];
    }
    // Сеть Фейстеля =====>>>
    
    // Последние действие при расшифровании
    if(mode == GOST_DEC)
        for(int bit = 0; bit < 32; bit++)
        {
            bool buf = block2[bit]; // bitМожно просто использовать block1
            block2[bit] = block2[32 + bit];
            block2[32 + bit] = buf;
        }

    delete [] block1;
    return block2;
}
