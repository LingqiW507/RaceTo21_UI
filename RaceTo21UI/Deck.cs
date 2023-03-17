using System;
using System.Collections.Generic;
using System.Linq; // currently only needed if we use alternate shuffle method

namespace RaceTo21UI
{
    public class Deck
    {
        List<Card> cards = new List<Card>();
        //create a dictionary that associates each card “ID” (short name) with one of the card image.
        public Dictionary<string, string> cardImages = new Dictionary<string, string>();

        public Deck()
        {
            Console.WriteLine("*********** Building deck...");
            string[] suits = { "Spades", "Hearts", "Clubs", "Diamonds" };

            for (int cardVal = 1; cardVal <= 13; cardVal++)
            {
                foreach (string cardSuit in suits)
                {
                    string cardName;
                    string cardLongName;
                    string cardImgName;
                    switch (cardVal)
                    {
                        case 1:
                            cardName = "A";
                            cardLongName = "Ace";
                            cardImgName = "A";
                            break;
                        case 11:
                            cardName = "J";
                            cardLongName = "Jack";
                            cardImgName = "J";
                            break;
                        case 12:
                            cardName = "Q";
                            cardLongName = "Queen";
                            cardImgName = "Q";
                            break;
                        case 13:
                            cardName = "K";
                            cardLongName = "King";
                            cardImgName = "K";
                            break;
                        default:
                            cardImgName = cardVal.ToString().PadLeft(2, '0');
                            cardName = cardVal.ToString();
                            cardLongName = cardName;

                            break;
                    }
                    cards.Add(new Card((cardName + cardSuit.First<char>()),(cardLongName + " of " + cardSuit)));
                    cardImages.Add(cardName + cardSuit.First<char>(), "card_" + cardSuit.ToLower() + "_" + cardImgName + ".png");

                }
            }
        }

        public void Shuffle()
        {
            Console.WriteLine("Shuffling Cards...");

            Random rng = new Random();

            // one-line method that uses Linq:
            // cards = cards.OrderBy(a => rng.Next()).ToList();

            // multi-line method that uses Array notation on a list!
            // (this should be easier to understand)
            for (int i=0; i<cards.Count; i++)
            {
                Card tmp = cards[i];
                int swapindex = rng.Next(cards.Count);
                cards[i] = cards[swapindex];
                cards[swapindex] = tmp;
            }
        }

        /* Maybe we can make a variation on this that's more useful,
         * but at the moment it's just really to confirm that our 
         * shuffling method(s) worked! And normally we want our card 
         * table to do all of the displaying, don't we?!
         */

        public void ShowAllCards()
        {
            for (int i=0; i<cards.Count; i++)
            {
                Console.Write(i+":"+cards[i].id); // a list property can look like an Array!
                if (i < cards.Count -1)
                {
                    Console.Write(" ");
                } else
                {
                    Console.WriteLine("");
                }
            }
        }

        public Card DealTopCard()
        {
            Card card = new Card(cards[cards.Count - 1].id, cards[cards.Count - 1].fullName);
            cards.RemoveAt(cards.Count - 1);
            // Console.WriteLine("I'm giving you " + card);
            return card;
        }
    }
}

