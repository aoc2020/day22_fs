module day22.Types

open System

type Deck (player:int, cards:int[]) as self =
    override this.ToString () = sprintf "Deck(player:%d cards=%A)" player cards 
    member this.Player = player
    member this.Cards = cards 