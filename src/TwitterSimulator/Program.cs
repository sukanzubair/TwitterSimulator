using System;
using System.Reflection;
using Autofac;
using log4net;
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
                
                foreach (var user in tweetManager.GetTweetsOrderedByUser())
                {
                    Console.WriteLine(user.Key);                    
                    foreach (var tweet in user.Value)
                    {
                        Console.WriteLine(tweet);
                    }                    
                }
               Console.ReadLine();
            }
        }
    }
}
;