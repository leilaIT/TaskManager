using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Prog will read 2 external files: (1) Members file; and (2) Task to be done, then store in their own lists [DONE]
            //Show creation of tasks with taskDesc and timeCreated (kahit sa last na)
            //taskTable will have tasks but no members assigned to each one yet, and ALL TASK STATUS will be OPEN [DONE]
            //Team lead will assign tasks, then prog will store the assigned member and task info in taskList 
            //Once team lead assigns a task, the TASK STATUS will be TASK ASSIGNED
            //TABLE COLUMNS
            //1 - Task
            //2 - Member
            //3 - Status
            //thread.sleep tasks by setting task duration formula then set status to for verification
            //ask team lead whether members' works are okay or not (note: might use loop for this)
            //if okay, set task status to CLOSED
            //if not okay yet, member will work again then prog will loop back to asking team lead
            //if all tasks are closed, end the program

            List<string> tasks = new List<string>();
            List<string> members = new List<string>();
            string[] status = new string[] { "STATUS", "Open", "Assigned", "For Verification", "For Revision", "Closed"};
            string input = "";

            //read tasks and members from files
            tasks = readfromFiles("ToDo.csv");
            members = readfromFiles("Members.csv");

            //set table size to number of tasks
            string[,] taskTable = new string[tasks.Count, 3];

            //displays initialized table
            displayTasks(taskTableIni(taskTable, tasks, members, status));
            Console.WriteLine();

            //displays members ready to work
            displayMembers(members);

            //get user input of team lead
            input = assignMembers(members);

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
        static string[,] taskTableIni(string[,] taskTable, List<string> tasks, List<string> members, string[] status)
        {
            //initializes and inputs data in the table
            int xCount = taskTable.GetLength(0);
            int yCount = taskTable.GetLength(1);
            for (int y = 0; y < taskTable.GetLength(1); y++)
            {
                int z = 3;
                for (int x = 0; x < taskTable.GetLength(0); x++)
                {
                    if (x == yCount - z && y == 0)
                        taskTable[x, y] = tasks[x];
                    else if (x == yCount - z && y == 1)
                    {
                        if (x == 0)
                            taskTable[x, y] = members[0];
                        else
                            taskTable[x, y] = "-";
                    }
                    else if (x == yCount - z && y == 2)
                    {
                        if (x == 0)
                            taskTable[x, y] = status[0];
                        else
                            taskTable[x, y] = status[1];
                    }
                    z--;
                }
            }
            return taskTable;
        }
        static void displayTasks(string[,] taskTable)
        {
            int numSpace = 0;
            string spaces = "";

            int maxSpace = longestWord(taskTable);

            //display
            for (int x = 0; x < taskTable.GetLength(0); x++)
            {
                for (int y = 0; y < taskTable.GetLength(1); y++)
                {
                    numSpace = maxSpace - taskTable[x, y].Length;
                    if (numSpace < 0)
                        numSpace = 0;
                    spaces = new string(' ', numSpace);
                    Console.Write($"{taskTable[x, y]}{spaces} ");
                }
                Console.WriteLine();
            }
        }
        static int longestWord(string[,] taskTable)
        {
            int maxSpace = 0;
            string wordChar = "";

            //get longest char
            for (int x = 0; x < taskTable.GetLength(0); x++)
            {
                for (int y = 0; y < taskTable.GetLength(1); y++)
                {
                    wordChar = taskTable[x, y];
                    if (maxSpace < wordChar.Length)
                        maxSpace = wordChar.Length;
                }
            }
            return maxSpace;
        }
        static string assignMembers(List<string> members)
        {
            string input = "";
            string name = "Leila";
            Console.Write($"Hello, {name}\n" +
                          $"Assign members to tasks using format \n" +
                          $"--> ");
            return input;
        }
        static void displayMembers(List<string> members)
        {
            Console.WriteLine("---------------------------------------------------");
            Console.Write("Idle Members:\n");
            for (int i = 1; i < members.Count; i++)
                Console.WriteLine($"* {members[i]}");
            Console.WriteLine("\n---------------------------------------------------");
        }
    }
}
