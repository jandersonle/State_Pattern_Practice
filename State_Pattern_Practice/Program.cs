using State_Pattern_Practice;
using State_Pattern_Practice.Models;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;

namespace State_Design_Pattern
{

    public class Program
    {
        public static async Task Main()
        {
            // Single Threaded non-api calls 
            //var runner = new Runner();
            // runner.Initilaize(5);


            // Mult-threaded non-api calls
            //var runner = new ThreadedRunner();
            //runner.Init(50);


            // Single threaded with api calls
            //var runner = new Runner();
            // await runner.ApiInitilaize(10);


            // multi - threaded with api calls
            var runner = new ThreadedRunner();
            await runner.ApiInit(10);
        }
    }
}
