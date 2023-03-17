using System;
using System.Collections.Generic;

namespace RaceTo21UI
{
    public class Game
    {
        public int numberOfPlayers; // number of players in current game
        public List<Player> players = new List<Player>(); // list of objects containing player data
        public CardTable cardTable; // object in charge of displaying game information
        public Deck deck = new Deck(); // deck of cards
        public int currentPlayer = 0; // current player on list
        public GameTask nextTask; // keeps track of game state
        private bool cheating = false; // lets you cheat for testing purposes if true

        public Game(CardTable c)
        {
            cardTable = c;
            deck.Shuffle();
            deck.ShowAllCards();
            nextTask = GameTask.GetNames;
            for (int i = 0; i < 8; i++)
            {
                AddPlayer("");
            }
            players[0].name = "Player#1";
            players[1].name = "Player#2";
        }

        /* Adds a player to the current game
         * Called by DoNextTask() method
         */
        public void AddPlayer(string n)
        {
            players.Add(new Player(n));
        }

        /* Figures out what task to do next in game
         * as represented by field nextTask
         * Calls methods required to complete task
         * then sets nextTask.
         */
        public void DoNextTask()
        {
            Console.WriteLine("================================"); // this line should be elsewhere right?
            if (nextTask == GameTask.GetNumberOfPlayers)
            {
                numberOfPlayers = cardTable.GetNumberOfPlayers();
                nextTask = GameTask.GetNames;
            }
            else if (nextTask == GameTask.GetNames)
            {
                for (var count = 1; count <= numberOfPlayers; count++)
                {
                    var name = cardTable.GetPlayerName(count);
                    AddPlayer(name); // NOTE: player list will start from 0 index even though we use 1 for our count here to make the player numbering more human-friendly
                }
                nextTask = GameTask.IntroducePlayers;
            }
            else if (nextTask == GameTask.IntroducePlayers)
            {
                cardTable.ShowPlayers(players);
                nextTask = GameTask.PlayerTurn;
            }
            else if (nextTask == GameTask.PlayerTurn)
            {
                cardTable.ShowHands(players);
                Player player = players[currentPlayer];
                if (player.status == PlayerStatus.active)
                {
                    int number = cardTable.GetNumberOfCards(player); //the player want to draw how many cards this loop will work related times. 

                    if (number == 0)
                    {
                        player.status = PlayerStatus.stay;
                    }
                    else
                    {
                        for (int i = 0; i < number; i++)
                        {

                            Card card = deck.DealTopCard();
                            player.cards.Add(card);
                            player.score = ScoreHand(player);
                        }
                        if (player.score > 21)
                        {
                            player.status = PlayerStatus.bust;
                        }
                        else if (player.score == 21)
                        {
                            player.status = PlayerStatus.win;
                        }
                    }
                }

                cardTable.ShowHand(player);
                nextTask = GameTask.CheckForEnd;
            }
            else if (nextTask == GameTask.CheckForEnd)
            {
                if (!CheckActivePlayers())
                {
                    Player winner = DoFinalScoring();
                    if (winner != null)
                    {
                        cardTable.AnnounceWinner(winner);
                        nextTask = GameTask.CheckForNewRound;
                    }
                    else
                    {
                        cardTable.AnnounceWinner(winner);
                        nextTask = GameTask.CheckForNewRound;
                    }
                }
                else
                {
                    currentPlayer++;
                    if (currentPlayer > players.Count - 1)
                    {
                        currentPlayer = 0; // back to the first player...
                    }
                    nextTask = GameTask.PlayerTurn;
                }
            }
            //check if the condition satisfy a new round. if someone want to leave, remove the player from the list
            else if (nextTask == GameTask.CheckForNewRound)
            {
                List<Player> tmpPlayers = new List<Player>(players);
                foreach (Player player in players)
                {
                    if (!cardTable.ToBeContinue(player))
                    {
                        tmpPlayers.Remove(player);
                    }
                    
                }
                players = tmpPlayers;

                if (players.Count > 1) //if players more than one, the game will restart.
                {
                    ShufflePlayer();//the players will have a new sequence
                    foreach (Player player in players)
                    {
                        player.Reset();
                    }
                    nextTask = GameTask.IntroducePlayers;
                }
                else
                {
                    if (players.Count == 1)// if only one player left 
                    {
                        Console.WriteLine(players[0].name + " wins!");//win directly.
                    }
                    else
                    {
                        Console.WriteLine("No more players!");//players all left
                    }
                    
                    Console.Write("Press <Enter> to exit... ");
                    while (Console.ReadKey().Key != ConsoleKey.Enter) { }
                    nextTask = GameTask.GameOver;
                }
            }
            else // we shouldn't get here...
            {
                Console.WriteLine("I'm sorry, I don't know what to do now!");
                nextTask = GameTask.GameOver;
            }
        }


        //calculate players scores in their hands.
        public int ScoreHand(Player player)
        {
            int score = 0;
            if (cheating == true && player.status == PlayerStatus.active)
            {
                string response = null;
                while (int.TryParse(response, out score) == false)
                {
                    Console.Write("OK, what should player " + player.name + "'s score be?");
                    response = Console.ReadLine();
                }
                return score;
            }
            else
            {
                foreach (Card card in player.cards)
                {
                    string faceValue = card.id.Remove(card.id.Length - 1);
                    switch (faceValue)
                    {
                        case "K":
                        case "Q":
                        case "J":
                            score = score + 10;
                            break;
                        case "A":
                            score = score + 1;
                            break;
                        default:
                            score = score + int.Parse(faceValue);
                            break;
                    }
                }
            }
            return score;
        }
        //check if there is a winner appear
        public bool CheckActivePlayers()
        {//if find the first winner,end the game
            for (int i = 0; i < numberOfPlayers; i++)
            {
                Player player = players[i];
                if (player.status == PlayerStatus.win)
                {
                    return false;
                }
            }
            
            int bustPlayer = 0;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                Player player = players[i];
                if (player.status == PlayerStatus.bust)
                {
                    bustPlayer++;
                }
            }
            if (bustPlayer == numberOfPlayers - 1)
            {
                return false;
            }

            for (int i = 0; i < numberOfPlayers; i++)
            {
                Player player = players[i];
                if (player.status == PlayerStatus.active)
                {
                    return true; // at least one player is still going!
                }
            }
            
            return false; // everyone has stayed!
        }

        //calculate players final scores and find the winner
        public Player DoFinalScoring()
        {
            int highScore = 0;
            int busted = 0;//count how many players busted
            for (int i = 0; i < numberOfPlayers; i++)
            {
                Player player = players[i];
                if (player.status == PlayerStatus.win) // someone hit 21
                {
                    return player;
                }
                if (player.status == PlayerStatus.stay) // still could win...
                {
                    if (player.score > highScore)
                    {
                        highScore = player.score;
                    }
                }

                // count how many busted players
                if (player.status == PlayerStatus.bust)
                {
                    busted++;
                }
            }
            if (busted == numberOfPlayers - 1)
            {
                // find the only player who isn't busted
                return players.Find(player => player.status != PlayerStatus.bust);
            }

            if (highScore > 0) // someone scored, anyway!
            {
                // find the FIRST player in list who meets win condition
                return players.Find(player => player.score == highScore);
            }

            // // we shouldn't get here because...
            return null; // everyone must have busted because nobody won!
        }

        //if left more than one player, the player list need shuffle.
        public void ShufflePlayer()
        {
            Console.WriteLine("Shuffling Players...");

            Random pyr = new Random();

            
            for (int i = 0; i < numberOfPlayers; i++)
            {
                Player pp = players[i];
                int swapindex = pyr.Next(numberOfPlayers);
                players[i] = players[swapindex];
                players[swapindex] = pp;
            }
        }


    }
}
    