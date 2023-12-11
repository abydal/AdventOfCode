namespace Day01;

static class Program {

    public static async Task Main() 
    {
        string[] digits = ["one","two","three","four","five","six","seven","eight","nine"];

        var file = new FileInfo("C:\\Users\\asbjo\\Documents\\repos\\AdventOfCode\\Day01\\input.txt");
        var reader = file.OpenText();
        Stack<int> numbers = new Stack<int>();
        int sum = 0;
        int counter = 0;
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            counter++;
            var number = string.Empty;
            var parseString = string.Empty;

            foreach (var c in line)
            {
                if(char.IsDigit(c))
                {
                    numbers.Push(int.Parse(c.ToString()));
                }
                else if(!char.IsDigit(c))
                {
                    parseString += c;

                    if(digits.Any(d => parseString.Contains(d)))
                    {
                        var digitToken = digits.Where(d => parseString.Contains(d)).First();
                        numbers.Push(digits.ToList().IndexOf(digitToken)+1);
                        parseString = parseString.Substring(parseString.IndexOf(digitToken)+1);
                    }
                }
            }
            parseString = string.Empty;

            int firstNumber = numbers.Pop();
            int lastNumber = firstNumber;
            var output = string.Empty;
            Console.Write($"Line {counter}: ");
            output += $" - Numbers found: {firstNumber}";

            while(numbers.Any())
            {
                lastNumber = numbers.Pop();
                output += $",{lastNumber}";
            }
            Console.Write($" {firstNumber},{lastNumber}");
            Console.Write(output);
            Console.Write($" | {line}");
            Console.WriteLine();
            sum += firstNumber+10*lastNumber;
        }

        Console.WriteLine($"Sum: {sum}");
    }

}