namespace MyExampleHelloWorld
{
    //here we create a custom ruleset just to emphasize some points of the language
    ruleset
    {
        stackpreferred;
        unsafe; //tolerate pointers, however, since we do not have the "unmanaged" flag, they will be managed pointers
        //we cant actually use selfalloc here because that is a heap specific value
    }

    void Main()
    {
        string out = allocate_str(12); //returns a char** but the compiler will take care of conversions for us
        out = "Hello World!"; //this can be applied to string and char**

        print("Hello World!"); //can accept pointers or values
    }
}

/*

AS C:

int main()
{
    char *out = "Hello World!";
    printf("%s", out);
    return 0;
}

AS AST:

        .main
    ____|______ init var out
___ |           |__________ type string
|                           |____________ set value
|                                       |___________ call func
|                                                   |___________ allocate_str(const_1)
|
|
|__ init var const_1
    |________________ type int
                     |________ set value
                              |__________ 12      
*/                                      
