using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    internal class lead
    {
        private taskInfos tInfo = new taskInfos();
        public int assignMembers(string[,] _taskTable, List<string> _tasks, List<string> _members, string[] _status, int _assignCount)
        {
            string[] tempArr = new string[] { };
            string name = "Leila";
            Console.Write($"Hello, {name}\n" +
                          $"Assign members to tasks using the format: 'TASKNUMBER-MEMBER' ex: 1-Leila\n" +
                          $"--> ");
            tempArr = assignInput(tempArr);
            if (tempArr.Length > 1 && _members.Contains(tempArr[1]) && int.Parse(tempArr[0]) < _tasks.Count && int.Parse(tempArr[0]) > 0)
            {
                for (int x = 0; x < _taskTable.GetLength(0); x++)
                {
                    for (int y = 0; y < _taskTable.GetLength(1); y++)
                    {
                        if (_taskTable[x, y][0] == tempArr[0][0])
                        {
                            _taskTable[x, y + 1] = tempArr[1];
                            _taskTable[x, y + 2] = _status[2];
                            _assignCount--;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("System does not contain that member or task number.");
                Console.ReadKey();
            }
            return _assignCount;
        }
        private string[] assignInput(string[] tempArr)
        {
            string input = "";
            input = Console.ReadLine().ToUpper();
            tempArr = input.Split('-');

            if (tempArr.Length != 2 || !input.Contains('-') || tempArr[0] == "" || tempArr[1] == "")
                tempArr = new string[] { "" };

            return tempArr;
        }
        public void updateStatus(string statusType, string[,] _taskTable, string[] _status)
        {
            string[] updateArr = new string[] { };

            if (statusType == "complete")
            {
                for (int y = 0; y < _taskTable.GetLength(1); y++)
                {
                    for (int x = 0; x < _taskTable.GetLength(0); x++)
                    {
                        if (x > 0 && y == 2)
                        {
                            if (_taskTable[x, y] == _status[2] || _taskTable[x, y] == _status[4])
                                _taskTable[x, y] = _status[3]; //sets status from assigned - for veri (completed)
                        }
                    }
                }
            }
            if (statusType == "verify")
            {
                int a = 0;
                Console.WriteLine("All Assigned Tasks Have Been Completed! Press any key to continue. . .");
                Console.ReadKey();

                do
                {
                    updateArr = verifyTasks(_taskTable);
                    for (int x = 0; x < _taskTable.GetLength(0); x++)
                    {
                        for (int y = 0; y < _taskTable.GetLength(1); y++)
                        {
                            if (int.TryParse(updateArr[0], out a))
                            {
                                if (x == int.Parse(updateArr[0]) && y == 2 && updateArr[1] == _status[5]) //closed status
                                    _taskTable[x, y] = updateArr[1];

                                else if (x == int.Parse(updateArr[0]) && y == 2 && updateArr[1] == _status[4]) //for revision status
                                    _taskTable[x, y] = updateArr[1];
                            }
                            else
                            {
                                Console.WriteLine("System does not contain that member or task number.");
                                Console.ReadKey();
                                x = _taskTable.GetLength(0);
                                y = _taskTable.GetLength(1);
                                //these two act as a break statement
                            }
                        }
                    }
                }
                while (taskChecker("check2", _taskTable, _status));

            }
        }
        private string[] verifyTasks(string[,] _taskTable)
        {
            string[] tempArr = new string[] { };
            tInfo.displayTasks(_taskTable);
            Console.Write($"TASK VERIFICATION\n" +
                          $"Update the status of a task using the: 'TASKNUMBER-STATUS' ex: 1-Closed\n" +
                          $"--> ");
            tempArr = assignInput(tempArr);

            return tempArr;
        }
        public bool taskChecker(string checkType, string[,] _taskTable, string[] _status)
        {
            bool allClosed = false;

            for (int y = 0; y < _taskTable.GetLength(1); y++)
            {
                for (int x = 0; x < _taskTable.GetLength(0); x++)
                {
                    if (x > 0 && y == 2)
                    {
                        if (checkType == "check1")
                        {
                            if (_taskTable[x, y] == _status[4]) //checks if there is for revision, then keep working
                            {
                                allClosed = true;
                                break;
                            }
                        }

                        if (checkType == "check2")
                        {
                            if (_taskTable[x, y] == _status[3]) //checks if there is for veri left, then keep asking to change status
                            {
                                allClosed = true;
                                break;
                            }
                        }
                    }
                }
            }

            return allClosed;
        }
    }
}
