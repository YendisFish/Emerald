//this is the only code that is supported right now... unmanaged and unsafe contexts... due to no garbage collector atm

reference standard;

ruleset;

int SomeFunction()
{
    while(1)
    {
        Alloc(sizeof(char) * 10);
    }
}