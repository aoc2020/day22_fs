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
    member this.isEmpty = cards.Length = 0
    member this.hasMoreCards = cards.Length > 1
    member this.topCard = cards.[0]
    member this.removeTopCard = Deck(player, cards |> Array.tail)
    member this.score () : uint64 =
        let cardScore (i:int) (c:Card) = ((i |> uint64) + 1UL) * (c |> uint64) 
        cards |> Seq.rev |> Seq.mapi cardScore |> Seq.sum  
    
type GameState (player1:Deck, player2: Deck) as self =
    override this.ToString () = sprintf "GameState\n  Player1: %A\n  Player2:  %A" player1 player2
    member this.Player1 = player1
    member this.Player2 = player2
    member this.isGameOver = player1.isEmpty || player2.isEmpty
    member this.getWinner () =
        if player1.isEmpty then player2
        else player1 
    member this.playRound () =
        let card1 = player1.topCard
        let player1 = player1.removeTopCard
        let card2 = player2.topCard
        let player2 = player2.removeTopCard
        if card1 > card2 then
            let player1 = player1.addCards [|card1;card2|]
            GameState (player1,player2)
        else
            let player2 = player2.addCards [|card2;card1|]
            GameState (player1,player2)            
    member this.playGame () =
        if this.isGameOver then self
        else this.playRound().playGame() 
        
        