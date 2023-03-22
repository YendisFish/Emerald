
//PROTOTYPE

namespace Colors
{
    ruleset default;

    class Red
    {
        int r;
        int g;
        int b;

        Red(default)
        {
            fields = default;
        }
    }
}

//CURRENT

namespace Colors
{
    rulset;

    class Red
    {
        int r;
        int g;
        int b;

        Red(int red, int green, int blue)
        {
            r = red;
            g = green;
            b = blue;
        }
    }
}