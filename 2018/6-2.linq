<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Linq.dll</Reference>
  <Namespace>System.Linq</Namespace>
</Query>

void Main()
{
    string inputString = @"137, 282;229, 214;289, 292;249, 305;90, 289;259, 316;134, 103;96, 219;92, 308;269, 59;141, 132;71, 200;337, 350;40, 256;236, 105;314, 219;295, 332;114, 217;43, 202;160, 164;245, 303;339, 277;310, 316;164, 44;196, 335;228, 345;41, 49;84, 298;43, 51;158, 347;121, 51;176, 187;213, 120;174, 133;259, 263;210, 205;303, 233;265, 98;359, 332;186, 340;132, 99;174, 153;206, 142;341, 162;180, 166;152, 249;221, 118;95, 227;152, 186;72, 330";
    int region = 10000;
    int withinRegion = 0;
    int gridSize = 500;
    int[,] grid = new int[gridSize, gridSize];

    Dictionary<int, Tuple<int, int>> coords = new Dictionary<int, System.Tuple<int, int>>();
    int row = 1;
    foreach (var input in inputString.Split(';'))
    {
        int x = int.Parse(input.Split(new[] { ", " }, StringSplitOptions.None)[0]);
        int y = int.Parse(input.Split(new[] { ", " }, StringSplitOptions.None)[1]);
        coords.Add(row, new Tuple<int, int>(x, y));
        grid[y, x] = row;
        row++;
    }

    for (int i = 0; i < gridSize; i++)
    {
        for (int j = 0; j < gridSize; j++)
        {
            double totalDistance = 0;
            foreach (var coord in coords)
            {
                double distance = Math.Abs(j - coord.Value.Item2) + Math.Abs(i - coord.Value.Item1);
                totalDistance += distance;
            }
            if (totalDistance < region)
            {
                withinRegion++;
            }
        }
    }
    
    withinRegion.Dump();
}