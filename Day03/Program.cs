using System.Data;
using System.Reflection;
using Superpower;

namespace Day03;

static class Program {

    struct Position {
        public Position(ushort x, ushort y)
        {
            X = x;
            Y = y;
        }
        public readonly ushort X;
        public readonly ushort Y;
    }

    struct PartNumber {
        public PartNumber(ushort number, Position position, ushort digitCount)
        {
            Number = number;
            Position = position;
            DigitCount = digitCount;
        }
        public readonly ushort Number = 0;
        public Position Position;
        public ushort DigitCount = 0;
    }

    struct Gear {

        public Gear(Position position)
        {
            Position = position;    
        }
        public readonly Position Position;
        public readonly List<PartNumber> PartNumbers = new();
    }

    enum Symbol {
        None,
        Digit,
        Gear,
        Other,
    }

    public static async Task Main() 
    {
        
        var file = new FileInfo("C:\\Users\\asbjo\\Documents\\repos\\AdventOfCode\\Day03\\input.txt");
        var lines = File.ReadAllLines(file.FullName);

        char[,] board = new char[lines.Length, lines[0].Length];

        for (int j = 0; j < lines.Length; j++)
        {
            for (int i = 0; i < lines[j].Length; i++)
            {
                board[j, i] = lines[j][i];
            }
        }

        var partNumbers = new List<PartNumber>();
        var notParts = new List<PartNumber>();
        var possibleGears = new List<Gear>();
        var digitParsing = string.Empty;
        for(ushort y = 0; y < board.GetLength(0); y++)
            for(ushort x = 0; x < board.GetLength(1) ; x++)
            {
                //Console.WriteLine($"{x},{y}");
                var c = board[y,x];
                var t = ParseChar(c);

                if(t == Symbol.Digit)
                {
                    digitParsing += c;
                }

                if(t == Symbol.Gear)
                {
                    possibleGears.Add(new Gear(new Position(x,y)));
                }

                
                if(!string.IsNullOrEmpty(digitParsing) && (t != Symbol.Digit || x == board.GetLength(1)-1))
                {
                    // since we are now at the end of the line we need to simulate that we have parsed the next char.
                    if(x == board.GetLength(1)-1 && t == Symbol.Digit)
                        x++;

                    var part = new PartNumber(ushort.Parse(digitParsing),new Position((ushort)(x - digitParsing.Length),y),(ushort)digitParsing.Length);

                    digitParsing = string.Empty;

                    if(IsRealPartNumber(part, board))
                        partNumbers.Add(part);
                    else
                        notParts.Add(part);

                }
            }

        possibleGears.ForEach(ng => ng.PartNumbers.AddRange(partNumbers.Where(n => PartNumberIsAdjacentToPosistion(n, ng.Position)).ToList()));
        var trueGears = possibleGears.Where(g => g.PartNumbers.Count() == 2).ToList();

        var notGears = possibleGears.Where(g => g.PartNumbers.Count() != 2).ToList();
        //Console.WriteLine(string.Join(",", parts.Select(p => p.Number)));
        Console.WriteLine($"Gear ratios sum: {trueGears.Sum(g => g.PartNumbers.Select(n => (int)n.Number).Aggregate((a,b) => a*b))}");
        Console.WriteLine($"Sum of parts is {partNumbers.Sum(p => p.Number)}");
    }

    private static Symbol ParseChar(char c) 
        => c switch
        {
            '.' => Symbol.None,
            '*' => Symbol.Gear,
            >= '0' and <= '9' => Symbol.Digit,
            _ => Symbol.Other
        };


    private static bool PartNumberIsAdjacentToPosistion(PartNumber partNumber, Position position){
        
        if(partNumber.Number == 522 && position.Y == 1 && position.X == 67)
            Console.WriteLine("bug");
        
        var start = partNumber.Position.X;
        var end = partNumber.Position.X+partNumber.DigitCount;
        var positionIsNotFarLeft = position.X >= (start == 0 ? 0 : start -1);
        var positionIsNotFarRight = position.X <= end;
        var positionIsNotFarAbove = partNumber.Position.Y >= (position.Y == 0 ? 0 : position.Y-1);
        var posisionIsNotFarUnder = partNumber.Position.Y <= position.Y+1;
        
        var isAdjacent = positionIsNotFarLeft
            && positionIsNotFarRight
            && positionIsNotFarAbove
            && posisionIsNotFarUnder;

        if(isAdjacent)
            Console.WriteLine($"Part {partNumber.Number} at position {partNumber.Position.X},{partNumber.Position.Y} is adjacent to position {position.X},{position.Y}");
        else
            Console.WriteLine($"Part {partNumber.Number} at position {partNumber.Position.X},{partNumber.Position.Y} is not adjacent to position {position.X},{position.Y}");
        
        return isAdjacent;
    }

    private static bool IsRealPartNumber(PartNumber part, char[,] board)
    {
        bool symbolFound = false;
        // check sides
        //Console.WriteLine($"Checking part {part.Number} at {part.Position.X},{part.Position.Y}");
        if(part.Number == 66 && part.Position.Y == 113)
            Console.WriteLine("Bug");
        
        if(part.Position.X - 1 > 0)
        {
            var left = board[part.Position.Y, part.Position.X-1];
            symbolFound = symbolFound || ParseChar(left) is Symbol.Other or Symbol.Gear;
        }

        if(part.Position.X + part.DigitCount < board.GetLength(1))
        {
            var right = board[part.Position.Y, part.Position.X + part.DigitCount];
            symbolFound = symbolFound || ParseChar(right) is Symbol.Other or Symbol.Gear;
        }

        // check top
        if(part.Position.Y > 0)
            for(int i = 0; i < part.DigitCount + 2; i++)
            {
                if(part.Position.X-1+i > board.GetLength(1)-1 || part.Position.X-1+i < 0)
                    continue;
                
                symbolFound = symbolFound || ParseChar(board[part.Position.Y-1, part.Position.X-1+i]) is Symbol.Other or Symbol.Gear;
            }
        
        if(part.Position.Y < board.GetLength(0)-1)
            for(int i = 0; i < part.DigitCount + 2; i++)
            {
                if(part.Position.X-1+i > board.GetLength(1)-1 || part.Position.X-1+i < 0)
                    continue;
                
                symbolFound = symbolFound || ParseChar(board[part.Position.Y+1, part.Position.X-1+i]) is Symbol.Other or Symbol.Gear;
            }
        
        if(symbolFound)
            Console.WriteLine($"Found part {part.Number} at postion {part.Position.X},{part.Position.Y}");
        else
            Console.WriteLine($"Did not find part {part.Number} at postion {part.Position.X},{part.Position.Y}");

        return symbolFound;
    }
}