using System;
using System.IO;
using Autofac;
using TwitterManager.Interfaces;

namespace TwitterSimulator
{
    class Program
    {    
        static void Main(string[] args)
        {
            var container = Configuration.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                var tweetManager = scope.Resolve<ITweetManager>();

                try
                {
                    foreach (var user in tweetManager.GetTweetsOrderedByUser())
                    {
                        Console.WriteLine(user.Key);
                        foreach (var tweet in user.Value)
                        {
                            Console.WriteLine(tweet);
                        }
                    }
                }
                catch(FileNotFoundException ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("Application cannot continue");
                }
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}
;