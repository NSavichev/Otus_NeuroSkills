using System.Security.Cryptography;

namespace SchulteTable.Core.Services;

// Генерирует таблицы с числами
public class TableGenerationService : ITableGenerationService
{
    public int[,] GenerateTable(int size)
    {
        var totalNumbers = size * size;
        var numbers = Enumerable.Range(1, totalNumbers).ToArray();
        
        // перемешиваем числа
        for (int i = numbers.Length - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(0, i + 1);
            (numbers[i], numbers[j]) = (numbers[j], numbers[i]);
        }

        var table = new int[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                table[i, j] = numbers[i * size + j];
            }
        }

        return table;
    }

    public int[,] ShuffleTable(int[,] table)
    {
        var size = table.GetLength(0);
        var totalNumbers = size * size;
        var numbers = new int[totalNumbers];

        // извлекаем числа из таблицы
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                numbers[i * size + j] = table[i, j];
            }
        }

        // перемешиваем
        for (int i = numbers.Length - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(0, i + 1);
            (numbers[i], numbers[j]) = (numbers[j], numbers[i]);
        }

        // заполняем таблицу заново
        var shuffledTable = new int[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                shuffledTable[i, j] = numbers[i * size + j];
            }
        }

        return shuffledTable;
    }
}
