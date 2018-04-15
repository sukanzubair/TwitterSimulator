using System.Collections.Generic;

namespace TwitterManager.Interfaces
{
    public interface ITweetManager
    {
        IDictionary<string, List<string>> GetTweetsOrderedByUser();
    }
}
