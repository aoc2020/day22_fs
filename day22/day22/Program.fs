open day22.Game1
open day22.Game2
open day22.IO

let task1 (game:Game1) =
    let game = game.playGame ()
    let winner = game.getWinner ()
    printfn "Answer 1: Player %d, Score: %d" winner.Player (winner.score ())

let task2 (game:Game2) =      
    let game = game.playGame ()
    let winner = game.getWinner ()
    printfn "Answer 2: Player %d, Score: %d" winner.Player (winner.score ())

[<EntryPoint>]
let main argv =
    let decks = readDecks "/Users/xeno/projects/aoc2020/day22_fs/input.txt"
    let game1 = Game1(decks)
    let game2 = Game2(decks)
    task2 (game2) 
    0