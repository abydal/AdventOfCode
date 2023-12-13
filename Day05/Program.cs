namespace Day05;

static class Program {

    enum MapType
    {
        Empty,
        Seed,
        Soil,
        Fertilizer,
        Water,
        Light,
        Temperature,
        Humidity,
        Location,
    }

    struct Mapping
    {
        public readonly MapType FromType;
        public readonly MapType ToType;
        public readonly List<(int From, int To)> Ids = new();

        public Mapping(MapType fromType, MapType toType)
        {
            FromType = fromType;
            ToType = toType;
        }
    }

    public static async Task Main() 
    {
        
        var file = new FileInfo("C:\\Users\\asbjo\\Documents\\repos\\AdventOfCode\\Day05\\test-case.txt");
        var lines = File.ReadAllLines(file.FullName);
        bool parsingMapping = false;
        Mapping mapping = default;
        Queue<Mapping> mappings = new();
        foreach (var line in lines)
        {
            if(line.Contains("map:"))
            {
                var mappingDescription = line.Split(" ")[0].Split("-to-");
                mapping = new Mapping((MapType)Enum.Parse(typeof(MapType), mappingDescription[0], true), 
                                        (MapType)Enum.Parse(typeof(MapType), mappingDescription[1], true));

                parsingMapping = true;
                continue;

            }

            if (parsingMapping)
            {
                if (string.IsNullOrEmpty(line))
                {
                    mappings.Enqueue(mapping);
                    parsingMapping = false;
                    continue;
                }
                Console.WriteLine(line);

                var range = int.Parse(line.Split(" ")[2]);
                int fromStart = int.Parse(line.Split(" ")[0]);
                int toStart = int.Parse(line.Split(" ")[1]);
                foreach(var i in Enumerable.Range(fromStart,range))
                {
                    mapping.Ids.Add((fromStart+i,toStart+i));
                }
            }


            if (line.Contains("seeds:"))
            {

            }
        }

        Console.WriteLine("End");
    }
}