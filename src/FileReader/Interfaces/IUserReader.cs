using FileReader.Entities;
using System.Collections.Generic;

namespace FileReader.Interfaces
{
    public interface IUserReader
    {
        IDictionary<string, User> GetTwitterUsers();        
    }
}
