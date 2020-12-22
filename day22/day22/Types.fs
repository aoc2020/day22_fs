module day22.Types

open System

type Card = int 
type Deck (player:int, cards:Card[]) as self =
    override this.ToString () = sprintf "Deck(player:%d cards=%A)" player cards 
    member this.Player = player
    member this.Cards : Card[] = cards
    member this.addCard (c:Card) =
        this.addCards [|c|]
    member this.addCards (cs:Card[]) =
        Deck (player, Array.append cards cs)
    
type GameState (player1:Deck, player2: Deck) as self =
    override this.ToString () = sprintf "GameState\n  Player1: %A\n  Player2:  %A" player1 player2
    member this.Player1 = player1
    member this.Player2 = player2