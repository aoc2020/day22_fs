module day22.IO

open System
open System.IO

open day22.Deck 
open day22.Game1
open day22.Game2

type RawDeck (data:List<String>) as self =
    override this.ToString () = sprintf "RawDeck(%A)" data
    member this.Data = data
    member this.toGameDeck () =
        let player = data.Head.[7..data.Head.Length-2] |> int
        let cards = data.Tail
                    |> List.map (int)
                    |> Seq.toArray 
        Deck(player,cards)

let private readFile (filePath:String) = seq {
    use sr = new StreamReader(filePath)
    while not sr.EndOfStream do
        yield sr.ReadLine ()
}

let readDecks (filePath:String) = 
    let input = readFile(filePath)
    let acc (lists: List<List<String>>) (s:String) : List<List<String>> =
        match lists, s with
        | [],s -> [[s]]        
        | list, "" -> []::list
        | h::tail,i -> (i::h)::tail
    let split = input |> Seq.fold acc [[]]   
    let split = split
                |> List.filter (fun (s:List<String>) -> [] <> s)
                |> List.map (fun (s:List<String>) -> s |> List.rev)
                |> List.map (RawDeck)
                |> List.map (fun (rd:RawDeck) -> rd.toGameDeck ())
                |> List.rev
                |> List.toArray
    Decks (split.[0],split.[1])


let readAsGame1 (filePath:String) : Game1 =
    let decks = readDecks filePath   
    Game1(decks.Deck1,decks.Deck2)
    
let readAsGame2 (filePath:String) : Game2 =
    let decks = readDecks filePath 
    Game2(decks.Deck1,decks.Deck2)
