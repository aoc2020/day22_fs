module day22.IO

open System
open System.IO
open day22.Types

type RawDeck (data:List<String>) as self =
    override this.ToString () = sprintf "RawDeck(%A)" data
    member this.Data = data
    member this.toDeck () =
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
                |> List.map (fun (rd:RawDeck) -> rd.toDeck ())
                |> List.rev
                |> List.toArray 
    printfn "%A" split 
        
