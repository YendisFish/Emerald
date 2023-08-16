
//here we create a custom ruleset just to emphasize some points of the language
ruleset
{
    stackpreferred;
    unsafe; //tolerate pointers, however, since we do not have the "unmanaged" flag, they will be managed pointers
    //we cant actually use selfalloc here because that is a heap specific value
}

void Main()
{
    char *str = "Hello World!";
    print(str);
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
|                                                   |
|                                                   |_init var const_1
                                                      |______ type int
                                                              |_____ set value
                                                                    |___________ 12
                              
*/                                      
