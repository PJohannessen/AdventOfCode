<Query Kind="Program" />

void Main()
{
    int totalRows = 400000;
    string input = "^.^^^.^..^....^^....^^^^.^^.^...^^.^.^^.^^.^^..^.^...^.^..^.^^.^..^.....^^^.^.^^^..^^...^^^...^...^.";
    
    List<string> rows = new List<string> { input };
    int remainingRows = totalRows - 1;
    while (remainingRows > 0)
    {
        string previousRow = rows.Last();
        StringBuilder sb = new StringBuilder();
        
        for (int i = 0; i < previousRow.Length; i++)
        {
            char space = '.';
            bool l = false;
            bool c = previousRow[i] == '^';
            bool r = false;
            
            if (i != 0) l = previousRow[i-1] == '^';
            if (i != previousRow.Length - 1) r = previousRow[i+1] == '^';
            
            if ((l && c && !r) ||
                (!l && c && r) ||
                (l && !c && !r) ||
                (!l && !c && r))
                {
                    space = '^';
                }
            
            sb.Append(space);
        }
        rows.Add(sb.ToString());
        remainingRows--;
    }
    
    int safeTiles = rows.SelectMany(s => s.ToCharArray()).Count(c => c == '.');
    safeTiles.Dump();
}