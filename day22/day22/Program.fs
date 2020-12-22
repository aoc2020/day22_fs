// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open day22.Types 
open day22.IO 

// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

[<EntryPoint>]
let main argv =
    let state = readGameState "/Users/xeno/projects/aoc2020/day22_fs/input.txt"
    printfn "Game state: %A" state
    0 // return an integer exit code