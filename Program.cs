// See https://aka.ms/new-console-template for more information


// await Day01.Program.Main();
// await Day02.Program.Main();
await Day05.Program.Main();



enum Digits {
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
}

struct Input {
    string Source;
    int Position;

    public Result<char> NextChar(){
        return new Result<char> {
            HasValue = Position < Source.Length,
            Value = Source[Position++],
            Remainder = this
        };
    }
}

struct Result<T> {
    public bool HasValue;
    public T Value;
    public Input Remainder;
    public static Result<T> Empty(Input remainder) => new Result<T>  {Value = default, Remainder = remainder };
}


static class Helpers {
    delegate Result<T> Parser<T>(Input input);

    static Parser<List<T>> Many<T>(this Parser<T> item) 
    {
        return input => 
        {
            var many = new List<T>();
            var next = item(input);
            while(next.HasValue)
            {
                many.Add(next.Value);
                next = item(next.Remainder);
            }
            return new Result<List<T>> {Value = many, Remainder = next.Remainder};
        };
    }

    static Parser<U> Then<T, U>(this Parser<T> first, Func<T, Parser<U>> makeSecond)
    {
        return input => 
        {
            var rf = first(input);
            if(!rf.HasValue)
            {
                return Result<U>.Empty(rf.Remainder);
            }
            return makeSecond(rf.Value)(rf.Remainder);
        };
    }

    static Parser<T> Return<T>(T value)
    {
        return input  => new Result<T> {Value = value, Remainder = input};
    }

    static Parser<char> Char(char c){
        return input => 
        {
            var next = input.NextChar();
            if(!next.HasValue || next.Value != c)
                return Result<char>.Empty(input);
            
            return next;
        };
    }

    static Parser<int> Digit(int digit)
    {
        return input => 
        {
            var next = input.NextChar();
            if(!next.HasValue || !char.IsDigit(next.Value) || int.Parse(next.Value.ToString()) != digit)
                return Result<int>.Empty(input);
            
            return new Result<int> {Value = int.Parse(next.Value.ToString()), Remainder = input };
        };
    }
}