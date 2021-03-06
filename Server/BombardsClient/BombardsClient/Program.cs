﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BombardsClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get a name
            Console.Write("Enter a name to use: ");
            string name = Console.ReadLine();

            // Get the ip
            Console.Write("Enter an ip address to connect to: ");
            string host = Console.ReadLine();

            // Set up the port and create the player
            int port = 8000;
            BG_Client user = new BG_Client(host, port, name);

            // connect and send messages
            user.Connect();

            // Permanantly check for new messages
            Thread thread = new Thread(new ThreadStart(user.CheckForNewMessages));
            thread.Start();

            // Main loop
            Thread.Sleep(7000);
            user.SendMessages("endturn");
            Console.ReadLine();
        }
    }
}
