using FileReader.Entities;
using System.Collections.Generic;

namespace FileReader.Interfaces
{
    public interface ITweetReader
    {      
        IEnumerable<Tweet> GetTwitterTweets();        
    }
}
