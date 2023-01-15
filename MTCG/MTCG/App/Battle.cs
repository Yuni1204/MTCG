using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG
{
    internal class Battle
    {
        Random rng = new Random();
        public List<string> log = new List<string>();

        public (int, List<string>) cardBattle(List<Card> deckP1, List<Card> deckP2)
        {
            int i = 0;
            for (; i < 100; i++)
            {
                if ((deckP1.Count == 0) || (deckP2.Count == 0))
                {
                    break;
                }
                //CardVsCard -> 1 for p1, 2 for p2, 0 for draw
                Card loser = CardVsCard(getCardFromDeck(deckP1), getCardFromDeck(deckP2));
                if (loser == null) //if draw
                {

                }
                else
                {
                    //var test = loserDeck(loser, deckP1);
                    if (loserDeck(loser, deckP1))
                    {
                        deckP1.Remove(loser);
                        deckP2.Add(loser);
                    }
                    else if (loserDeck(loser, deckP2))
                    {
                        deckP2.Remove(loser);
                        deckP1.Add(loser);
                    }
                }
            }
            if (deckP1.Count == 0)
            { //p1 no cards so winner is p2
                return (2, log);
            }
            else if (deckP2.Count == 0)
            { //p2 no cards so winner is p1
                return (1, log);
            }
            else
            {//draw
                return (0, log);
            }
        }

        private bool loserDeck(Card loser, List<Card> deck)
        {
            if (deck.Count == 0)
            {
                return true;
            }
            else
            {
                foreach (Card card in deck)
                {
                    if (card.Id == loser.Id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Card CardVsCard(Card card1, Card card2)
        {
            var specialcasewinner = specialCase(card1, card2);
            if (spellCardExists(card1.Type, card2.Type))
            {
                if (specialcasewinner != null)
                {
                    log.Add(card1.Name + " vs " + card2.Name + "...\nWinner: " + specialcasewinner.Name + "\n");
                    return specialcasewinner;
                }
                var winner = cvcWithElements(card1, card2);
                if (winner == null)
                {
                    log.Add(card1.Name + " vs " + card2.Name + "...\nDraw\n");
                }
                else
                {
                    log.Add(card1.Name + " vs " + card2.Name + "...\nWinner: " + winner.Name + "\n");
                }
                return winner;
            }
            else 
            {
                if (specialcasewinner != null)
                {
                    log.Add(card1.Name + " vs " + card2.Name + "...\nWinner: " + specialcasewinner.Name + "\n");
                    return specialcasewinner;
                }
                var winner = cvcWithoutElements(card1, card2);
                if (winner == null)
                {
                    log.Add(card1.Name + " vs " + card2.Name + "...\nDraw\n");
                }
                else
                {
                    log.Add(card1.Name + " vs " + card2.Name + "...\nWinner: " + winner.Name + "\n");
                }
                return winner;
            }
        }

        private bool spellCardExists(string typeC1, string typeC2)
        {
            if (typeC1 == "SpellCard" || typeC2 == "SpellCard")
            {
                return true;
            }
            return false;
        }

        public Card getCardFromDeck(List<Card> deck)
        {
            return deck.ElementAt(rngInRange(deck.Count));
        }

        private int rngInRange(int range)
        {
            return rng.Next(range);
        }

        public Card cvcWithElements(Card c1, Card c2)
        {
            float eDmg1 = c1.Dmg;
            float eDmg2 = c2.Dmg;
            (eDmg1, eDmg2) = getEffectiveDmg(c1, c2);
            if (eDmg1 > eDmg2)
            {
                return c2;
            }
            else if (eDmg2 > eDmg1)
            {
                return c1;
            }
            else
            {
                return null;
            }
        }

        public Card cvcWithoutElements(Card c1, Card c2)
        {
            if (c1.Dmg > c2.Dmg)
            {
                return c2;
            }
            else if (c1.Dmg < c2.Dmg)
            {
                return c1;
            }
            else
            {
                return null;
            }
        }

        private (float, float) getEffectiveDmg(Card c1, Card c2)
        {
            if (waterVSfire(c1.Element, c2.Element) > 0)
            {
                if (waterVSfire(c1.Element, c2.Element) == 1)
                {
                    return (c1.Dmg * 2, c2.Dmg / 2);
                } 
                return (c1.Dmg / 2, c2.Dmg * 2);
            }
            else if (fireVSnormal(c1.Element, c2.Element) > 0)
            {
                if (fireVSnormal(c1.Element, c2.Element) == 1)
                {
                    return (c1.Dmg * 2, c2.Dmg / 2);
                }
                return (c1.Dmg / 2, c2.Dmg * 2);
            }
            else if (normalVSwater(c1.Element, c2.Element) > 0)
            {
                if (normalVSwater(c1.Element, c2.Element) == 1)
                {
                    return (c1.Dmg * 2, c2.Dmg / 2);
                }
                return (c1.Dmg / 2, c2.Dmg * 2);
            }
            else
            {
                return (c1.Dmg, c2.Dmg);
            }
        }

        private int waterVSfire(string Element1, string Element2)
        {
            if (Element1 == "Water" && Element2 == "Fire")
            {
                return 1;
            }
            else if (Element1 == "Fire" && Element2 == "Water")
            {
                return 2;
            }
            else { return 0; }
        }
        private int fireVSnormal(string Element1, string Element2)
        {
            if (Element1 == "Fire" && Element2 == "Normal")
            {
                return 1;
            }
            else if (Element1 == "Normal" && Element2 == "Fire")
            {
                return 2;
            }
            else { return 0; }
        }
        private int normalVSwater(string Element1, string Element2)
        {
            if (Element1 == "Normal" && Element2 == "Water")
            {
                return 1;
            }
            else if (Element1 == "Water" && Element2 == "Normal")
            {
                return 2;
            }
            else { return 0; }
        }


        private Card specialCase(Card c1, Card c2)
        {
            int winner = findSpecialCase(c1.Name, c2.Name);
            if (winner == 1)
            {
                return c1;
            }
            else if (winner == 2)
            {
                return c2;
            }
            return null;
        }

        private int findSpecialCase(string cName1, string cName2)
        {
            if (goblinVSdragon(cName1, cName2) > 0)
            {
                return goblinVSdragon(cName1, cName2);
            }
            else if (wizardVSorks(cName1, cName2) > 0)
            {
                return wizardVSorks(cName1, cName2);
            }
            else if (knightVSwaterspell(cName1, cName2) > 0)
            {
                return knightVSwaterspell(cName1, cName2);
            }
            else if (krakenVSspell(cName1, cName2) > 0)
            {
                return krakenVSspell(cName1, cName2);
            }
            else if (fireelvesVSdragon(cName1, cName2) > 0)
            {
                return fireelvesVSdragon(cName1, cName2);
            }
            else
            {
                return (-1);
            }
        }

        private int goblinVSdragon(string cName1, string cName2)
        {
            if (cName1.Contains("Dragon") && cName2.Contains("Goblin"))
            {
                return 1;
            }
            else if (cName1.Contains("Goblin") && cName2.Contains("Dragon"))
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }
        private int wizardVSorks(string cName1, string cName2)
        {
            if (cName1.Contains("Wizard") && cName2.Contains("Ork"))
            {
                return 1;
            }
            else if (cName1.Contains("Ork") && cName2.Contains("Wizard"))
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }
        private int knightVSwaterspell(string cName1, string cName2)
        {
            if (cName1.Contains("WaterSpell") && cName2.Contains("Knight"))
            {
                return 1;
            }
            else if (cName1.Contains("Knight") && cName2.Contains("WaterSpell"))
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }
        private int krakenVSspell(string cName1, string cName2)
        {
            if (cName1.Contains("Kraken") && cName2.Contains("Spell"))
            {
                return 1;
            }
            else if (cName1.Contains("Spell") && cName2.Contains("Kraken"))
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }
        private int fireelvesVSdragon(string cName1, string cName2)
        {
            if (cName1.Contains("FireElve") && cName2.Contains("Dragon"))
            {
                return 1;
            }
            else if (cName1.Contains("Dragon") && cName2.Contains("FireElve"))
            {
                return 2;
            }
            else
            {
                return -1;
            }
        }

        
    }
}
