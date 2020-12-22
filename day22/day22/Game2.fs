module day22.Game2

open day22.Deck 
type Card = int
type TrackingKey = Card[]*Card[]

type Tracker (state: Set<TrackingKey>) =
    new() = Tracker(Set.empty)
    member this.Add(key:TrackingKey) = Tracker(state.Add key)
    member this.Contains : TrackingKey -> bool = state.Contains 
    
type Game2 (player1:Deck, player2: Deck, previous: Tracker) as self =
    new (player1:Deck, player2:Deck) = Game2(player1,player2,Tracker())
    new (decks:Decks) = Game2(decks.Deck1, decks.Deck2)
    override this.ToString () = sprintf "GameState[2]\n  Player1: %A\n  Player2:  %A" player1 player2
    member this.Player1 = player1
    member this.Player2 = player2
    member this.Tracker : TrackingKey = player1.Cards,player2.Cards
    
    member this.newState (player1:Deck, player2:Deck, previous: Tracker) : Game2 =
        let previous = previous.Add this.Tracker
        Game2 (player1,player2,previous)
        
    member this.getWinner () =
        if player1.isEmpty then player2        
        else if player2.isEmpty then player1
        else player1 // game stopped since both cards have been played
    member this.isGameOver () =
        player1.isEmpty || player2.isEmpty
    member this.getWinnerFromSubGame () : Deck =
        let state = Game2(player1.forSubGame(), player2.forSubGame(), previous)
        let state = state.playGame ()
        state.getWinner ()
    member this.shouldPlaySubGame () : bool =
        let enough1 = player1.hasEnoughCards ()
        let enough2 = player2.hasEnoughCards ()
        enough1 && enough2
    member this.hasBeenSeenBefore () : bool =
        previous.Contains this.Tracker
    member this.topsToPlayer1 () : Game2 =
        let card1 = player1.topCard ()
        let card2 = player2.topCard ()
        let player1 = player1.removeTopCard ()
        let player2 = player2.removeTopCard ()
        let player1 = player1.addCards card1 card2
        this.newState(player1,player2,previous)
    member this.topsToPlayer2 () : Game2 =
        let card1 = player1.topCard ()
        let card2 = player2.topCard ()
        let player1 = player1.removeTopCard ()
        let player2 = player2.removeTopCard ()
        let player2 = player2.addCards card2 card1
        this.newState(player1,player2,previous)
    member this.clearPlayer2 () : Game2 =
        self.newState(player1,player2.asEmpty (),previous)       
    member this.playRound () : Game2 =
        if this.hasBeenSeenBefore() then
            self.clearPlayer2 ()
        else if this.shouldPlaySubGame() then
            let winner = self.getWinnerFromSubGame ()
            if winner.is player1 then
                this.topsToPlayer1 ()
            else
                this.topsToPlayer2 ()            
        else
            let card1 = player1.topCard ()
            let card2 = player2.topCard () 
            if card1 > card2 then
                self.topsToPlayer1 ()
            else
                self.topsToPlayer2 ()
    member this.playGame () : Game2 =
        if this.isGameOver () then self
        else this.playRound().playGame() 
        
        