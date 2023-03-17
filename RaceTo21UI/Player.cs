using System;
using System.Collections.Generic;

namespace RaceTo21UI
{
	public class Player
	{
		public string name;
		public List<Card> cards = new List<Card>();
		public PlayerStatus status = PlayerStatus.active;
		public int score;

        public Player(string n)
		{
			name = n;
		}

		/* Introduces player by name
		 * Called by CardTable object
		 */
		public void Introduce(int playerNum)
		{
			Console.WriteLine("Hello, my name is " + name + " and I am player #" + playerNum);
		}

		public void Reset()
        {
			cards = new List<Card>();
			status = PlayerStatus.active;
			score = 0;
        }
	}

}