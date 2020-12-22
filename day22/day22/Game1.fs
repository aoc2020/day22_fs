module day22.Game1 

open day22.Deck

type Card = int 
    
type Game1 (player1:Deck, player2: Deck) as self =
    new (decks: Decks) = Game1 (decks.Deck1, decks.Deck2)
    override this.ToString () = sprintf "GameState\n  Player1: %A\n  Player2:  %A" player1 player2
    member private this.Player1 = player1
    member private this.Player2 = player2
    member this.isGameOver = player1.isEmpty || player2.isEmpty
    member this.getWinner () =
        if player1.isEmpty then player2
        else player1 
    member this.playRound () =
        let card1 = player1.topCard ()
        let player1 = player1.removeTopCard ()
        let card2 = player2.topCard ()
        let player2 = player2.removeTopCard ()
        if card1 > card2 then
            let player1 = player1.addCards  card1 card2
            Game1 (player1,player2)
        else
            let player2 = player2.addCards card2 card1
            Game1 (player1,player2)            
    member this.playGame () =
        if this.isGameOver then self
        else this.playRound().playGame() 
        
        