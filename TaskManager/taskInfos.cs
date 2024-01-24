using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    internal class taskInfos
    {
        public List<string> readfromFiles(string fileName)
        {
            List<string> tableInfo = new List<string>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    tableInfo.Add(line);
                }
            }
            return tableInfo;
        }
        public void taskTableIni(string[,] _taskTable, List<string> _tasks, List<string> _members, string[] _status)
        {
            //initializes and inputs data in the table
            int yCount = _taskTable.GetLength(1);
            for (int y = 0; y < _taskTable.GetLength(1); y++)
            {
                int z = 3;
                for (int x = 0; x < _taskTable.GetLength(0); x++)
                {
                    if (x == yCount - z && y == 0)
                        _taskTable[x, y] = _tasks[x];
                    else if (x == yCount - z && y == 1)
                    {
                        if (x == 0)
                            _taskTable[x, y] = _members[0];
                        else
                            _taskTable[x, y] = "-";
                    }
                    else if (x == yCount - z && y == 2)
                    {
                        if (x == 0)
                            _taskTable[x, y] = _status[0];
                        else
                            _taskTable[x, y] = _status[1];
                    }
                    z--;
                }
            }
        }
        public void displayTasks(string[,] _taskTable)
        {
            int numSpace = 0;
            string spaces = "";

            int maxSpace = longestWord(_taskTable);

            Console.Clear();
            //display
            for (int x = 0; x < _taskTable.GetLength(0); x++)
            {
                for (int y = 0; y < _taskTable.GetLength(1); y++)
                {
                    numSpace = maxSpace - _taskTable[x, y].Length;
                    if (numSpace < 0)
                        numSpace = 0;
                    spaces = new string(' ', numSpace);
                    Console.Write($"{_taskTable[x, y]}{spaces} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n-----------------------------------------------------------------------------------------------------");
        }
        private int longestWord(string[,] _taskTable)
        {
            int maxSpace = 0;
            string wordChar = "";

            //get longest char
            for (int x = 0; x < _taskTable.GetLength(0); x++)
            {
                for (int y = 0; y < _taskTable.GetLength(1); y++)
                {
                    wordChar = _taskTable[x, y];
                    if (maxSpace < wordChar.Length)
                        maxSpace = wordChar.Length;
                }
            }
            return maxSpace;
        }
    }
}
