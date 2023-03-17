using System;
using System.Collections.Generic;

namespace RaceTo21UI
{
    public class CardTable
    {
        public CardTable()
        {
            Console.WriteLine("Setting Up Table...");
        }

        /* Shows the name of each player and introduces them by table position.
         * Is called by Game object.
         * Game object provides list of players.
         * Calls Introduce method on each player object.
         */
        public void ShowPlayers(List<Player> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Introduce(i + 1); // List is 0-indexed but user-friendly player positions would start with 1...
            }
        }

        /* Gets the user input for number of players.
         * Is called by Game object.
         * Returns number of players to Game object.
         */
        public int GetNumberOfPlayers()
        {
            Console.Write("How many players? ");
            string response = Console.ReadLine();
            int numberOfPlayers;
            while (int.TryParse(response, out numberOfPlayers) == false
                || numberOfPlayers < 2 || numberOfPlayers > 8)
            {
                Console.WriteLine("Invalid number of players.");
                Console.Write("How many players?");
                response = Console.ReadLine();
            }
            return numberOfPlayers;
        }

        /* Gets the name of a player
         * Is called by Game object
         * Game object provides player number
         * Returns name of a player to Game object
         */
        public string GetPlayerName(int playerNum)
        {
            Console.Write("What is the name of player# " + playerNum + "? ");
            string response = Console.ReadLine();
            while (response.Length < 1)
            {
                Console.WriteLine("Invalid name.");
                Console.Write("What is the name of player# " + playerNum + "? ");
                response = Console.ReadLine();
            }
            return response;
        }
        /*A player can choose to draw up to 3*/
        public int GetNumberOfCards(Player player)
        {
            Console.Write(player.name + " Enter the number of cards to pick(0/1/2/3): ");
            string response = Console.ReadLine();
            //if (int.TryParse(response, out numberOfCards)== false
            //   || numberOfCards < 0 || numberOfCards > 3)
            //
            //   response = Console.ReadLine();
            // }
            while (true)
            {
                if (response == "3")
                {
                    return 3;
                }
                else if (response == "2")
                {
                    return 2;
                }
                else if (response == "1")
                {
                    return 1;
                }
                else if (response == "0")
                {
                    return 0;
                }
                else
                {
                    Console.WriteLine(player.name + " Please give me a valid number!");
                    Console.Write(player.name + " Enter the number of cards to pick(0/1/2/3): ");
                    response = Console.ReadLine();
                }
            }


        }

        /*public bool OfferACard(Player player)
        {
            while (true)
            {
                Console.Write(player.name + ", do you want a card? (Y/N)");
                string response = Console.ReadLine();
                if (response.ToUpper().StartsWith("Y"))
                {
                    return true;
                }
                else if (response.ToUpper().StartsWith("N"))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Please answer Y(es) or N(o)!");
                }
            }
        }*/

        public void ShowHand(Player player)
        {
            if (player.cards.Count > 0)
            {
                Console.Write(player.name + " has: ");

                string selfCards = "";

                foreach (Card card in player.cards)
                {
                    //Console.Write(card + " ");
                    selfCards = selfCards + card.fullName + ",";
                }
                // show full name of cards.
                Console.Write(selfCards.Remove(selfCards.Length - 2) + " = " + player.score + "/21 ");
                if (player.status != PlayerStatus.active)
                {
                    Console.Write("(" + player.status.ToString().ToUpper() + ")");
                }
                Console.WriteLine();
            }
        }

        public void ShowHands(List<Player> players)
        {
            foreach (Player player in players)
            {
                ShowHand(player);
            }
        }


        public void AnnounceWinner(Player player)
        {
            if (player != null)
            {
                Console.WriteLine(player.name + " wins!");
            }
            else
            {
                Console.WriteLine("No one get card!");
            }
        }

        public bool ToBeContinue(Player player)
        {
            while (true)
            {
                Console.Write(player.name + ", do you want to play again? (Y/N)");
                string response = Console.ReadLine();
                if (response.ToUpper().StartsWith("Y"))
                {
                    return true;
                }
                else if (response.ToUpper().StartsWith("N"))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Please answer Y(es) or N(o)!");
                }

            }
        }
    }


}
