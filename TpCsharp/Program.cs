var c1 = new PointC { X = 1 };
var c2 = c1;
c2.X = 9;
Console.WriteLine($"classe : c1.X = {c1.X}");

var s1 = new PointS { X = 1 };
var s2 = s1;
s2.X = 9;
Console.WriteLine($"struct : s1.X = {s1.X}");

string a = "info";
string b = string.Concat("in", "fo");
Console.WriteLine(a == b);

Console.WriteLine(0.1 + 0.2 == 0.3);
Console.WriteLine(0.1m + 0.2m == 0.3m);

class PointC { public int X; }
struct PointS { public int X; }