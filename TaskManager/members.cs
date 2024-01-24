using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskManager
{
    internal class members
    {
        private Random rnd = new Random();
        public void displayMembers(List<string> _members)
        {
            Console.Write("Active Members:\n");
            for (int i = 1; i < _members.Count; i++)
                Console.WriteLine($"* {_members[i]}");
            Console.WriteLine("\n-----------------------------------------------------------------------------------------------------");
        }
        public void working()
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
    }
}
