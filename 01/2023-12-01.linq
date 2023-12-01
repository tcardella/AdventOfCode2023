<Query Kind="Statements" />

//var inputs = await File.ReadAllLinesAsync(@"C:\Users\deadl\Documents\GitHub\AdventOfCode2023\01\input01.txt");

var inputs = new[] {
//"1abc2",
//"pqr3stu8vwx",
//"a1b2c3d4e5f",
//"treb7uchet"

"two1nine",
"eightwothree",
"abcone2threexyz",
"xtwone3four",
"4nineeightseven2",
"zoneight234",
"7pqrstsixteen"
};

var sum = 0;

var regex = new Regex("(?:1|2|3|4|5|6|7|8|9|one|two|three|four|five|six|seven|eight|nine)");
var tokens = new Dictionary<string, int>() { { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 }, { "five", 5 }, { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 } };

foreach (var input in inputs)
{



	var x = regex.Matches(input).Select(e => e.Value);

	var first = x.First();
	var last = x.Last();

	if (tokens.ContainsKey(first))
		first = tokens[first].ToString();

	if (tokens.ContainsKey(last))
		last = tokens[last].ToString();

	var value = int.Parse(first + last);
	//value.Dump();
	
	new Tuple<string, IEnumerable<string>, int>(input, x, value).Dump();
	
	sum += value;
}




sum.Dump();