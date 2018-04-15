

namespace FileReader.Entities
{
    public class Tweet
    {
        public Tweet(string userName, string tweet)
        {
            UserName = userName;
            Text = tweet;
        }

        public string UserName { get; private set; }

        public string Text { get; private set; }        
    }
}
