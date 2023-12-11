using System.Drawing;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace Day02;

static class Program {

    enum Color {
        Red,
        Green,
        Blue
    }

    struct DiceSet {
        public DiceSet(ushort red, ushort green, ushort blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
        public readonly ushort Red = 0;
        public readonly ushort Green = 0;
        public readonly ushort Blue = 0;
    }

    struct Game {

        public Game(ushort id, DiceSet[] diceSets)
        {
            Id = id;
            DiceSets = diceSets;
        }
        public readonly ushort Id;
        public readonly DiceSet[] DiceSets;
    }


    public static async Task Main() 
    {
        
        var file = new FileInfo("C:\\Users\\asbjo\\Documents\\repos\\AdventOfCode\\Day02\\input.txt");
        var lines = File.ReadLines(file.FullName).ToArray();
        
        var games = GetGames(lines);
        
        //Part1(games);

        Part2(games);
    }

    private static void Part2(Game[] games)
    {
        var sum = games.Sum(g => 
            g.DiceSets.Max(d => d.Red)
            * g.DiceSets.Max(d => d.Green)
            * g.DiceSets.Max(d => d.Blue));

        Console.WriteLine($"Sum of power of sets: {sum}");
    }

    private static void Part1(Game[] games)
    {
        var constraints = new DiceSet(12, 13, 14);
        var sum = 0;
        foreach(var game in games)
        {            
            if(game.DiceSets.Any(d => d.Red > constraints.Red || d.Green > constraints.Green || d.Blue > constraints.Blue))
            {
                Console.WriteLine($"Game {game.Id} not possible");
            }
            else
            {
                sum+= game.Id;
            }
        }
        Console.WriteLine($"Sum of ids: {sum}");
    }

    private static  Game[] GetGames(string[] lines){
        return lines.Select(line => 
            new Game(GetGameId(line), GetDiceSets(line).Select(ParseDiceSet).ToArray())
        ).ToArray();
    }


    private static string[] GetDiceSets(string game)
    {
        return game.Split(":")[1].Split(";");
    }

    private static DiceSet ParseDiceSet(string diceSet)
    {
        return new DiceSet(ParseDiceCount(diceSet, Color.Red),ParseDiceCount(diceSet, Color.Green),ParseDiceCount(diceSet, Color.Blue));
    }

    private static ushort ParseDiceCount(string diceSet, Color color)
    {
        var count = diceSet.Split(", ").FirstOrDefault(d => d.Contains(color.ToString().ToLower()))?.Trim().Split(" ")[0];
        return count == null ? ushort.MinValue : ushort.Parse(count);
    }

    private static ushort GetGameId(string line)
    {
        return ushort.Parse(line.Split(":")[0].Replace("Game ", ""));
    }



}