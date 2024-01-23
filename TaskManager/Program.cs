using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TaskManager
{
    internal class Program
    {
        static string[,] _taskTable = new string[,] { };
        static List<string> _tasks = new List<string>();
        static List<string> _members = new List<string>();
        static string[] _status = new string[] { "STATUS", "OPEN", "ASSIGNED", "FOR VERIFICATION", "FOR REVISION", "CLOSED" };
        static Random rnd = new Random();
        static int _assignCount = 0;

        static void Main(string[] args)
        {
            //Prog will read 2 external files: (1) Members file; and (2) Task to be done, then store in their own lists [DONE]
            //Show creation of tasks with taskDesc and timeCreated (kahit sa last na, also might put this in output file instead)
            //taskTable will have tasks but no members assigned to each one yet, and ALL TASK STATUS will be OPEN [DONE]
            //Team lead will assign tasks, then prog will store the assigned member and task info in taskTable [DONE]
            //Once team lead assigns a task, the TASK STATUS will be ASSIGNED [DONE]
            //TABLE COLUMNS
            //1 - Task
            //2 - Member
            //3 - Status
            //thread.sleep tasks by setting task duration formula then set status to for verification [DONE]
            //ask team lead whether members' works are okay or not (note: might use loop for this) [DONE]
            //if okay, set task status to CLOSED [DONE]
            //if not okay yet, member will work again then prog will loop back to asking team lead [DONE]
            //if all tasks are closed, end the program [DONE]

            //read tasks and members from files
            _tasks = readfromFiles("ToDo.csv");
            _members = readfromFiles("Members.csv");

            //set table size to number of tasks
            _taskTable = new string[_tasks.Count, 3];
            _assignCount = _tasks.Count - 1;
            taskTableIni();

            //assigning
            while(_assignCount != 0)
            {
                //displays table
                displayTasks();
                Console.WriteLine();

                //displays members ready to work
                displayMembers();

                //get user input of team lead for assigning members
                assignMembers();
            }

            //members will do their tasks
            displayTasks();
            working();
            //updateStatus("complete");
            //displayTasks();

            while (true)
            {
                //check for revisions
                updateStatus("complete");
                displayTasks();
                updateStatus("verify");
                displayTasks();
                if (taskChecker("check1"))
                    working();
                else
                    break;
            }
            Console.WriteLine("I have left the while loop!");
            Console.ReadKey();
        }
        static List<string> readfromFiles(string fileName)
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
        static void taskTableIni()
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
        static void displayTasks()
        {
            int numSpace = 0;
            string spaces = "";

            int maxSpace = longestWord();

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
        static int longestWord()
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
        static void working()
        {
            int workTime = rnd.Next(20, 40) * 100;
            Console.Write("Members are working on their tasks ");
            Thread.Sleep(2000);
            //for(int x = 0; x < 20; x++)
            //{
            //    Console.Write(". ");
            //    Thread.Sleep(workTime);
            //}
            Console.WriteLine();
        }
        static void assignMembers()
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
        }
        static string[] assignInput(string[] tempArr)
        {
            string input = "";
            input = Console.ReadLine().ToUpper();
            tempArr = input.Split('-');

            if (tempArr.Length != 2 || !input.Contains('-') || tempArr[0] == "" || tempArr[1] == "")
                tempArr = new string [] { "" };

            return tempArr;
        }
        static string[] verifyTasks()
        {
            string[] tempArr = new string[] { };
            displayTasks();
            Console.Write($"TASK VERIFICATION\n" +
                          $"Update the status of a task using the: 'TASKNUMBER-STATUS' ex: 1-Closed\n" +
                          $"--> ");
            tempArr = assignInput(tempArr);
            
            return tempArr;
        }
        static void updateStatus(string statusType)
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
                            if(_taskTable[x, y] == _status[2] || _taskTable[x, y] == _status[4])
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
                    updateArr = verifyTasks();
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
                while (taskChecker("check2"));

            }
        }
        static bool taskChecker(string checkType)
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
        static void displayMembers()
        {
            Console.Write("Active Members:\n");
            for (int i = 1; i < _members.Count; i++)
                Console.WriteLine($"* {_members[i]}");
            Console.WriteLine("\n-----------------------------------------------------------------------------------------------------");
        }
    }
}
