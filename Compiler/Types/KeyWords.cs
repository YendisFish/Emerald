namespace Emerald.Types;

public enum Keywords
{
    //bracket followed values
    NAMESPACE, //defines a space where a ruleset operates
    RULESET, //set of rules defined for the compiler
    CLASS, //this can operate as a struct or class... depends on the memory location restraints (MLRs)
    //object types are in the VarType.cs file

    //we will now define the ruleset recognized keywords

    //MLRs
    STACKPREFERRED, //values are by default allocated on the stack but can be/if needed will be allocated on the heap (this is default)
    STACKREQUIRED, //values are only allocated on the stack
    HEAPPREFERRED, //values are by default heap allocated but are capable of sitting on the stack
    HEAPREQUIRED, //values are only allocated in the heap

    //GC notifiers MACs
    SELFALLOC, //GC doesn't help us at all
    /*
        with the next two keywords the compiler automatically knows to import the memory management library
    */
    UNSAFE, //tolerates pointers
    UNMANAGED, //doesn't mess with memory (only valid in unsafe context)

    //Next we have contextual keywords
    DEFAULT,
    FIELDS
}