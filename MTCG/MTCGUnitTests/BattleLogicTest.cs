using MTCG;

namespace MTCGUnitTests
{
    
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        //[Test]
        //public void Test1()
        //{
        //    Assert.Pass();
        //}

        [Test]
        public void Test_cvcWithoutElements()
        {
            //arrange
            var card1 = new MonsterCard("card1", "Ork", 20);
            var card2 = new MonsterCard("card2", "Goblin", 10);

            var expected = card2; //loser is expected
            var testBattle = new Battle();

            //Act
            var actual = testBattle.cvcWithoutElements(card1, card2);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_loserDeck_loserCard_in_Deck()
        {
            //arrange
            var loserCardInDeck = new SpellCard("loser", "WaterSpell", 1);

            List<Card> deck = new List<Card>();
            deck.Add(new MonsterCard("card1", "FireOrk", 5));
            deck.Add(new SpellCard("card2", "RegularSpell", 5));
            deck.Add(new MonsterCard("card3", "Monster", 5));
            deck.Add(new SpellCard("loser", "WaterSpell", 1));

            var expectedWhenInDeck = true;
            
            var testBattle = new Battle();

            //Act
            var actualIfTrue = testBattle.loserDeck(loserCardInDeck, deck);

            //Assert
            Assert.That(expectedWhenInDeck, Is.EqualTo(actualIfTrue));
        }

        [Test]
        public void Test_loserDeck_loserCard_not_in_Deck()
        {
            var loserCardNotInDeck = new SpellCard("otherID", "FireDragon", 3);

            List<Card> deck = new List<Card>();
            deck.Add(new MonsterCard("card1", "FireOrk", 5));
            deck.Add(new SpellCard("card2", "RegularSpell", 5));
            deck.Add(new MonsterCard("card3", "Monster", 5));
            deck.Add(new SpellCard("loser", "WaterSpell", 1));

            var expectedWhenNotInDeck = false;

            var testBattle = new Battle();

            var actualIfFalse = testBattle.loserDeck(loserCardNotInDeck, deck);

            Assert.That(expectedWhenNotInDeck, Is.EqualTo(actualIfFalse));
        }

        [Test]
        public void Test_loserDeck_NoCardInDeck()
        {
            List<Card> emptyDeck = new List<Card>();

            var expectedWhenEmptyDeck = true;
            var testBattle = new Battle();

            var actualIfNoCardInDeck = testBattle.loserDeck(null, emptyDeck);

            Assert.That(expectedWhenEmptyDeck, Is.EqualTo(actualIfNoCardInDeck));
        }

        [Test]
        public void Test_cvcWithElements_water_fire()
        {
            //arrange
            //water -> fire
            var card1 = new SpellCard("card1", "FireSpell", 20);
            var card2 = new MonsterCard("card2", "WaterGoblin", 10);
            
            var expected_water_fire = card1; //loser is expected
            
            var testBattle = new Battle();

            //Act
            var actual_water_fire = testBattle.cvcWithElements(card1, card2);
            
            // Assert
            Assert.That(expected_water_fire, Is.EqualTo(actual_water_fire));
            
            
        }

        [Test]
        public void Test_cvcWithElements_fire_normal()
        {
            //fire -> normal
            var card2_1 = new SpellCard("card1", "FireSpell", 10);
            var card2_2 = new MonsterCard("card2", "Dragon", 30);

            var expected_fire_normal = card2_2; //loser is expected

            var testBattle = new Battle();

            var actual_fire_normal = testBattle.cvcWithElements(card2_1, card2_2);

            Assert.That(expected_fire_normal, Is.EqualTo(actual_fire_normal));
        }

        [Test]
        public void Test_cvcWithElements_normal_water()
        {
            //normal -> water
            var card3_1 = new SpellCard("card1", "RegularSpell", 10);
            var card3_2 = new MonsterCard("card2", "WaterKraken", 30);

            var expected_normal_water = card3_2; //loser is expected

            var testBattle = new Battle();

            var actual_normal_water = testBattle.cvcWithElements(card3_1, card3_2);

            Assert.That(expected_normal_water, Is.EqualTo(actual_normal_water));
        }

        [Test]
        public void Test_spellCardExists_OutcomeTrue()
        {
            //arrange
            var typeSpell = "SpellCard";
            var typeMonster = "MonsterCard";

            var expected = true;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.spellCardExists(typeSpell, typeMonster);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_spellCardExists_OutcomeFalse()
        {
            //arrange
            var type1 = "MonsterCard";
            var type2 = "MonsterCard";

            var expected = false;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.spellCardExists(type1, type2);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_waterVSfire_player1_is_water()
        {
            //arrange
            var p1Element = "Water";
            var p2Element = "Fire";

            var expected = 1;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.waterVSfire(p1Element, p2Element);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }
        
        [Test]
        public void Test_waterVSfire_player2_is_water()
        {
            //arrange
            var p1Element = "Fire";
            var p2Element = "Water";

            var expected = 2;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.waterVSfire(p1Element, p2Element);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_fireVSnormal_player1_is_fire()
        {
            //arrange
            var p1Element = "Fire";
            var p2Element = "Normal";

            var expected = 1;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.fireVSnormal(p1Element, p2Element);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_fireVSnormal_player2_is_fire()
        {
            //arrange
            var p1Element = "Normal";
            var p2Element = "Fire";

            var expected = 2;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.fireVSnormal(p1Element, p2Element);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_normalVSwater_player1_is_normal()
        {
            //arrange
            var p1Element = "Normal";
            var p2Element = "Water";

            var expected = 1;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.normalVSwater(p1Element, p2Element);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_normalVSwater_player2_is_normal()
        {
            //arrange
            var p1Element = "Water";
            var p2Element = "Normal";

            var expected = 2;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.normalVSwater(p1Element, p2Element);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_goblinVSdragon_Dragon_is_card1()
        {
            //arrange
            var CardName1 = "FireDragon";
            var CardName2 = "WaterGoblin";

            var expected = 1;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.goblinVSdragon(CardName1, CardName2);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_goblinVSdragon_Dragon_is_card2()
        {
            //arrange
            var CardName1 = "WaterGoblin";
            var CardName2= "FireDragon";

            var expected = 2;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.goblinVSdragon(CardName1, CardName2);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_wizardVSorks_Wizard_is_card1()
        {
            //arrange
            var CardName1 = "FireWizard";
            var CardName2 = "WaterOrk";

            var expected = 1;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.wizardVSorks(CardName1, CardName2);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_wizardVSorks_Wizard_is_card2()
        {
            //arrange
            var CardName1 = "WaterOrk";
            var CardName2 = "FireWizard";

            var expected = 2;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.wizardVSorks(CardName1, CardName2);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_knightVSwaterspell_WaterSpell_is_card1()
        {
            //arrange
            var CardName1 = "WaterSpell";
            var CardName2 = "WaterKnight";

            var expected = 1;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.knightVSwaterspell(CardName1, CardName2);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_knightVSwaterspell_WaterSpell_is_card2()
        {
            //arrange
            var CardName1 = "WaterKnight";
            var CardName2 = "WaterSpell";

            var expected = 2;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.knightVSwaterspell(CardName1, CardName2);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_krakenVSspell_Kraken_is_card1()
        {
            //arrange
            var CardName1 = "Kraken";
            var CardName2 = "WaterSpell";

            var expected = 1;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.krakenVSspell(CardName1, CardName2);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_krakenVSspell_Kraken_is_card2()
        {
            //arrange
            var CardName1 = "WaterSpell";
            var CardName2 = "FireKraken";

            var expected = 2;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.krakenVSspell(CardName1, CardName2);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_fireelvesVSdragon_FireElve_is_card1()
        {
            //arrange
            var CardName1 = "FireElve";
            var CardName2 = "Dragon";

            var expected = 1;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.fireelvesVSdragon(CardName1, CardName2);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void Test_fireelvesVSdragon_FireElve_is_card2()
        {
            //arrange
            var CardName1 = "Dragon";
            var CardName2 = "FireElve";

            var expected = 2;
            var testBattle = new Battle();

            //Act
            var actual = testBattle.fireelvesVSdragon(CardName1, CardName2);

            // Assert
            Assert.That(expected, Is.EqualTo(actual));
        }
    }
}