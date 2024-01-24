using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TaskManager
{
    internal class Program
    {
        //Prog will read 2 external files: (1) Members file; and (2) Task to be done, then store in their own lists [DONE]
        //Show creation of tasks with taskDesc and timeCreated (kahit sa last na) [DONE]
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

        //TIME RECORDS NEEDED
        //creation - [DONE]
        //assignment - [DONE]
        //verification - [DONE]

        //FOR OUTPUT FILES
        //have output file for verifier info

        static DateTime dtnow = DateTime.Now;
        static string[,] _taskTable = new string[,] { };
        static List<string> _tasks = new List<string>();
        static List<string> _members = new List<string>();
        static string[] _status = new string[] { "STATUS", "OPEN", "ASSIGNED", "FOR VERIFICATION", "FOR REVISION", "CLOSED" };
        static int _assignCount = 0;
        static void Main(string[] args)
        {
            string name = "";
            taskInfos tInfo = new taskInfos();
            members m = new members();
            lead l = new lead();

            //read tasks and members from files
            _tasks = tInfo.readfromFiles("ToDo.csv");
            _members = tInfo.readfromFiles("Members.csv");

            //set table size to number of tasks
            _taskTable = new string[_tasks.Count, 3];
            _assignCount = _tasks.Count - 1;

            //initialization
            name = l.leadInfo();
            tInfo.createTasks(_tasks);
            tInfo.taskTableIni(_taskTable, _tasks, _members, _status);

            //assigning
            while (_assignCount != 0)
            {
                //displays table
                tInfo.displayTasks(_taskTable);

                //displays members ready to work
                m.displayMembers(_members);

                //get user input of team lead for assigning members
                _assignCount = l.assignMembers(_taskTable, _tasks, _members, _status, _assignCount, name);
            }

            //members will do their tasks
            tInfo.displayTasks(_taskTable);
            m.working();
            
            while (true) //verification
            {
                //check for revisions
                l.updateStatus("complete", _taskTable, _status);
                tInfo.displayTasks(_taskTable);
                l.updateStatus("verify", _taskTable, _status);
                tInfo.displayTasks(_taskTable);
                if (l.taskChecker("check1", _taskTable, _status))
                    m.working();
                else
                    break;
            }
            writeToFile(l.assignments, "Assignment Details.csv");
            writeToFile(l.verification, "Verification Details.csv");
            writeToFile(tInfo.createdTasks, "Tasks Details.csv");
            writeToFile(tInfo.createdTasks, "Verifier Details.csv");

            Console.WriteLine("All tasks have been closed! Press any key to exit. . .");
            l.verification.Add($"All tasks have been completed and closed at {dtnow}");
            Console.ReadKey();
        }
        static void writeToFile(List<string> list, string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName, true))
            {
                if(fileName == "Verifier Details.csv")
                {
                    sw.WriteLine("Name, Verifier ID");
                }
                for(int x = 0; x < list.Count; x++)
                {
                    sw.WriteLine(list[x]);
                }
            }            
        }
    }
}
