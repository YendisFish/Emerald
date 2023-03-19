//this is the only code that is supported right now... unmanaged and unsafe contexts... due to no garbage collector atm

reference standard;

namespace Convoluted
{
    ruleset
    {
        unsafe;
        unmanaged;
    }

    void Main()
    {
        char *out = "Hello World!";
        print(out);
    }
}