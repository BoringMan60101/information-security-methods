/*
 Данная утилита реализиует шифрование / расшифровку сообщения
 с помощью алгоритма ГОСТ 28149-89

 Стандартный ввод преобразуется в зависимости от режима (шифрование / расшифрование)
 и результат отправляется на стандартный вывод.
*/

#include "gostSourceCode.CPP"

int main(const int argc, const char ** argv)
{
    if(argc != 2)
    {
        fprintf(stdout, "Need 1 necessary argument: \'mode\' for GOST\n");
        fprintf(stdout, "\'mode\' can be either \'e\' or \'d\'\n");
        fprintf(stdout, "\'e\' - encryption, \'d\' - decryption\n");
        return 1;
    }

    // Считывание с потока стандартного ввода
    const int maxLen = 4096;
    char input[maxLen], output[maxLen];
    output[0] = '\0';
    fgets(input, maxLen-1, stdin); // !!! Она запоминает символ переноса строки !!!
    const int len = strlen(input) - 1;
    const int bN = len / 8; // Размер блока в ГОСТ равен 8 символовам blocksNumber

    // Определение переданного режима (о режиме см. gostSourceCode.CPP)
    int mode = 0;
    switch(argv[1][0])
    {
        case 'e': { mode = 1; break; } // см. прототип функции GOST()
        case 'd': { mode = 0; break; } // см. прототип функции GOST()
        default:
        {
            fprintf(stdout, "Need 1 necessary argument: \'mode\' for GOST\n");
            fprintf(stdout, "\'mode\' can be either \'e\' or \'d\'\n");
            fprintf(stdout, "\'e\' - encryption, \'d\' - decryption\n");
            return 2;
        }
    }

    // Сборка вывода (часть сообщения, содержащая полные блоки по 8 байт)
    for(int b = 0; b < bN; b++)
    {
        bool * block = charsToBlock(input, 8*b);
        bool * transformedBlock = GOST(block, mode);
        char * transformedChars = blockToChars(transformedBlock);
        for(int c = 0; c < 8; c++)
            output[8*b + c] = transformedChars[c];
        delete [] block;
        delete [] transformedBlock;
        delete [] transformedChars;
    }

    // Дополнение последнего блока до 8 символов
    // для этого используются символы пробелов ' '

    if(len % 8)
    {
        char tail[8];
        for(int i = 0; i < 8; i++)
            tail[i] = (i < len % 8) ? input[8*bN + i] : ' ';

        bool * block = charsToBlock(tail, 0);
        bool * transformedBlock = GOST(block, mode);
        char * transformedChars = blockToChars(transformedBlock);
        for(int c = 0; c < 8; c++)
            output[8*bN + c] = transformedChars[c];
        delete [] block;
        delete [] transformedBlock;
        delete [] transformedChars;

        output[8*bN + 8] = '\0'; // !!! Строка по Си стандрту !!!
    }
    else
        output[8*bN] = '\0'; // !!! Строка по Си стандрту !!!

    // Вывод преобразованного набора данных в поток стандартного вывода
    fprintf(stdout, "%s\n", output);

    return 0;
}
