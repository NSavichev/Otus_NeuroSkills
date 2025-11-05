namespace SchulteTable.Core.Services;

public interface ITableGenerationService // интерфейс генерации таблиц
{
    int[,] GenerateTable(int size);
    int[,] ShuffleTable(int[,] table);
}
