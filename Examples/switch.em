reference io;

ruleset;

int Main()
{
    int x = 5;

    switch(x)
    {
        case 2:
        {
            break;
        }

        case 3:
        {
            print("Foo");
        }

        fallback:
        {
            print("Bar");
        }
    }
}