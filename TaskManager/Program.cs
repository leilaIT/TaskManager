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
        static int _assignCount = 0;
        static taskInfos tInfo = new taskInfos();
        static void Main(string[] args)
        {
            //Prog will read 2 external files: (1) Members file; and (2) Task to be done, then store in their own lists [DONE]
            //Show creation of tasks with taskDesc and timeCreated (kahit sa last na, NOTE: might put this in output file instead)
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

            //FOR OUTPUT FILES
            //have output file for verifier info
            members m = new members();
            lead l = new lead();

            //read tasks and members from files
            _tasks = tInfo.readfromFiles("ToDo.csv");
            _members = tInfo.readfromFiles("Members.csv");

            //set table size to number of tasks
            _taskTable = new string[_tasks.Count, 3];
            _assignCount = _tasks.Count - 1;
            
            //initialization
            tInfo.taskTableIni(_taskTable, _tasks, _members, _status);

            //assigning
            while (_assignCount != 0)
            {
                //displays table
                tInfo.displayTasks(_taskTable);
                Console.WriteLine();

                //displays members ready to work
                m.displayMembers(_members);

                //get user input of team lead for assigning members
                _assignCount = l.assignMembers(_taskTable, _tasks, _members, _status, _assignCount);
            }

            //members will do their tasks
            tInfo.displayTasks(_taskTable);
            m.working();

            while (true)
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
            Console.WriteLine("All tasks have been closed! Press any key to exit. . .");
            Console.ReadKey();
        }
    }
}
