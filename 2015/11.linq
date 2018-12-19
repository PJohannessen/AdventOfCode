<Query Kind="Program" />

void Main()
{
    FindNext("vzbxkghb");
    FindNext("vzbxxyzz");
}

void FindNext(string password)
{
    char[] passwordChars = password.ToArray();
    Increment(passwordChars, passwordChars.Length - 1);
    while (!IsValid(passwordChars))
    {
        Increment(passwordChars, passwordChars.Length - 1);
    }
    new string(passwordChars).Dump();
}


bool IsValid(char[] password)
{
    if (password.Any(c => c == 'i' | c == 'o' || c == 'l')) return false;
    bool sequential = false;
    for (int i = 0; i < password.Length - 2; i++)
    {
        if (password[i] == password[i + 1] - 1 && password[i] == password[i + 2] - 2)
        {
            sequential = true;
        }
    }
    if (!sequential) return false;
    int pairs = 0;
    for (int i = 0; i < password.Length - 1; i++)
    {
        if (password[i] == password[i + 1])
        {
            pairs++;
            i++;
        }
    }
    return pairs >= 2;
}

void Increment(char[] password, int index)
{
    char c = password[index];
    if (c == 'h' || 'c' == 'n' || c == 'k')
    {
        c = (char)(c + 2);
        password[index] = c;
    }
    else if (c == 'z')
    { 
        password[index] = 'a';
        Increment(password, index - 1);
    }
    else
    {
        password[index] = (char)(c + 1);
    }
}