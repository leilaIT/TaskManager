using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManager
{
    internal class Program
    {
        static string[,] _taskTable = new string[,] { };
        static List<string> _tasks = new List<string>();
        static List<string> _members = new List<string>();
        static string[] _status = new string[] { "STATUS", "Open", "Assigned", "For Verification", "For Revision", "Closed" };
        static Random rnd = new Random();

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
            //thread.sleep tasks by setting task duration formula then set status to for verification
            //ask team lead whether members' works are okay or not (note: might use loop for this)
            //if okay, set task status to CLOSED
            //if not okay yet, member will work again then prog will loop back to asking team lead
            //if all tasks are closed, end the program

            //read tasks and members from files
            _tasks = readfromFiles("ToDo.csv");
            _members = readfromFiles("Members.csv");

            //set table size to number of tasks
            _taskTable = new string[_tasks.Count, 3];
            taskTableIni();

            //assigning
            while(_members.Count != 1)
            {
                //displays table
                displayTasks();
                Console.WriteLine();

                //displays members ready to work
                displayMembers();

                //get user input of team lead for assigning members
                assignMembers();

                Console.Clear();
            }

            //members will do their tasks
            displayTasks();
            Console.WriteLine("\n---------------------------------------------------");
            int workTime = rnd.Next(20, 50) * 100;
            Thread.Sleep(workTime);
            Console.WriteLine("");
            displayTasks();

            Console.ReadKey();
        }
        static List<string> readfromFiles(string fileName)
        {
            string[] tempArr = new string[] { };
            string tempWord = "";
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
            int xCount = _taskTable.GetLength(0);
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
        static void assignMembers()
        {
            string[] tempArr = new string[] { };
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
                            for (int i = 0; i < _members.Count; i++)
                            {
                                if (_members[i] == tempArr[1])
                                    _members.RemoveAt(i);
                            }
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
            string name = "Leila";
            Console.Write($"Hello, {name}\n" +
                          $"Assign members to tasks using the format: 'TASKNUMBER-MEMBER' ex: 1-Leila\n" +
                          $"--> ");
            input = Console.ReadLine().ToUpper();
            tempArr = input.Split('-');

            if (tempArr.Length != 2 && !input.Contains('-'))
                tempArr = new string [] { "" };

                return tempArr;
        }
        static void displayMembers()
        {
            Console.WriteLine("---------------------------------------------------");
            Console.Write("Idle Members:\n");
            for (int i = 1; i < _members.Count; i++)
                Console.WriteLine($"* {_members[i]}");
            Console.WriteLine("\n---------------------------------------------------");
        }
    }
}
