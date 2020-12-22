// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open day22.Types 
open day22.IO 

// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

[<EntryPoint>]
let main argv =
    let decks = readDecks "/Users/xeno/projects/aoc2020/day22_fs/input.txt"
    let message = from "F#" // Call the function
    printfn "Hello world %s" message
    0 // return an integer exit code