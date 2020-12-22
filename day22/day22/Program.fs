// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open day22.IO 

// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

let task1 () =
    let state = readAsGame1 "/Users/xeno/projects/aoc2020/day22_fs/input.txt"
    printfn "Game state: %A" state
    printfn "Round 1: %A" (state.playRound ())
    let final = state.playGame ()
    let winner = final.getWinner ()
    printfn "Final: %A" final
    printfn "Score: %d" (winner.score ())
    0 // return an integer exit code

let task2 () = 
    let state = readAsGame2 "/Users/xeno/projects/aoc2020/day22_fs/input.txt"
    let final = state.playGame ()
    let winner = final.getWinner ()
    printfn "Final: %A" final
    printfn "Score: %d" (winner.score ())
    0 // return an integer exit code

[<EntryPoint>]
let main argv =
    task2 () 
    0 // return an integer exit code