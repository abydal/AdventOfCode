using System.Collections.Frozen;

namespace Day04;

static class Program {

   struct Card {

        public Card(int id)
        {
            Id = id;
        }

        public readonly int Id;
        public readonly HashSet<int> Numbers = new();
        public readonly HashSet<int> WinningNumbers = new();
        public int Score = 0;
        public bool IsCalculated = false;
   }

    public static async Task Main() 
    {
        
        var file = new FileInfo("C:\\Repos\\AdventOfCode\\Day04\\input.txt");
        var lines = File.ReadAllLines(file.FullName);
       
        var cards = new List<Card>();
        foreach (var line in lines)
        {
            cards.Add(ParseCard(line));
        }

        var score = PartOneScore(cards);

        var partTwoScore = PartTwoScore(cards);

        Console.WriteLine($"Score: {score}");
    }

    private static List<Card> PartTwoScore(List<Card> cards)
    {
        var stack = new Stack<Card>();
        var orderedCards = cards.OrderByDescending(card => card.Id).ToList();
        var cardDictionary = orderedCards.ToFrozenDictionary(c => c.Id, c => c);

        var leftoverCards = new List<Card>();

        foreach (var c in orderedCards)
        {
            stack.Push(c);
        }
        
        while(stack.Count > 0)
        {
            leftoverCards.AddRange(ComputeCard(stack.Pop(), cardDictionary));
        }

        return leftoverCards;
    }

    private static List<Card> ComputeCard(Card card, FrozenDictionary<int, Card> cards)
    {
        if(!card.IsCalculated) { 
            var score = card.WinningNumbers.Intersect(card.Numbers).Count();
            card.Score = score;
        }
        //Console.WriteLine($"Card {card.Id} has score {score}");
        var newCards = new List<Card>() { card };

        if (card.Score == 0)
            return newCards;


        for(int i = 1; i <= card.Score; i++)
        {
            //Console.WriteLine($"CardId:{card.Id}, i:{i}");

            if(cards.ContainsKey(card.Id+i))
                newCards.AddRange(ComputeCard(cards[card.Id+i],cards));
        }

        return newCards;
    }

    private static Card GetCardWithId(int id, List<Card> cards)
    {
        return cards.FirstOrDefault(c => c.Id == id);
    }

    private static int PartOneScore(List<Card> cards)
    {

        return cards.Sum(c => GetCardScore(c));
    }

    private static int GetCardScore(Card card)
    {
        var matches = card.Numbers.Intersect(card.WinningNumbers).Count();
        if (matches > 1)
            return (int)Math.Pow(2, matches - 1);
        else
            return matches;

    }

    private static Card ParseCard(string line)
    {
        var cardId = line.Split(":")[0].Replace("Card ", "");
        var card = line.Split(":")[1];
        var numbers = card.Split("|")[0].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var winningNumbers = card.Split("|")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var c = new Card(int.Parse(cardId));
        numbers.ToList().ForEach(number => c.Numbers.Add(int.Parse(number)));
        winningNumbers.ToList().ForEach(number => c.WinningNumbers.Add(int.Parse(number)));
        return c;
    }
}