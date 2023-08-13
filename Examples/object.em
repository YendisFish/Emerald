ruleset;

class Color
{
    int r;
    int g;
    int b;

    Color(default);
}

class FieldsClass
{
    int x;
    char y;
    long *z;

    FieldsClass()
    {
        fields = void; //as long as the fields are basic types Emerald will auto fill them with their empty value
    }
}