using System;
namespace RaceTo21UI
{
    public class Card
    {
        public string id;
        public string fullName;
        public Card(string shortName, string longName)
        {
            id = shortName;
            fullName = longName;
        }
    }
}
