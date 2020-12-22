module day22.Game2

open day22.Game1

type Card = int
type CacheKey = Card[]*Card[]
    
type Game2Deck (player:int, cards:Card[]) as self =
    override this.ToString () = sprintf "Deck(player:%d cards=%A)" player cards 
    member this.Player = player
    member this.Cards : Card[] = cards
    member this.addCard (c:Card) =
        Game2Deck (player, Array.append cards [|c|])
    member this.addCards (card1:Card) (card2:Card)  =
        Game2Deck (player, Array.append cards [|card1;card2|])
    member this.isEmpty = cards.Length = 0
    member this.hasMoreCards = cards.Length > 1
    member this.topCard () = cards.[0]
    member this.removeTopCard () = Game2Deck(player, cards |> Array.tail)
    member this.score () : uint64 =
        let cardScore (i:int) (c:Card) = ((i |> uint64) + 1UL) * (c |> uint64) 
        cards |> Seq.rev |> Seq.mapi cardScore |> Seq.sum
    member this.hasEnoughCards () = this.topCard () < cards.Length
    member this.hasTooFewCards () = this.hasEnoughCards() |> not  
    member this.is (deck: Game2Deck) = player = deck.Player
    member this.forSubGame () = 
        let cards = cards.[1..cards.[0]]
        Game2Deck(player,cards)
    member this.asEmpty () = Game2Deck (player, Array.empty)

type Cache (state: Set<Card[]*Card[]>) as self =
    new() = Cache(Set.empty)
    member this.Add(key:CacheKey) = Cache(state.Add key)
    member this.Contains : CacheKey -> bool = state.Contains 
    
type Game2GameState (player1:Game2Deck, player2: Game2Deck, previous: Cache) as self =    
    override this.ToString () = sprintf "GameState[2]\n  Player1: %A\n  Player2:  %A" player1 player2
    member this.Player1 = player1
    member this.Player2 = player2
    member this.CacheKey : CacheKey = player1.Cards,player2.Cards
    
    member this.newState (player1:Game2Deck, player2:Game2Deck, previous: Cache) : Game2GameState =
        let previous = previous.Add this.CacheKey
        Game2GameState (player1,player2,previous)
        
    member this.getWinner () =
        if player1.isEmpty then player2        
        else if player2.isEmpty then player1
        else player1 // game stopped since both cards have been played
    member this.isGameOver () =
        if player1.isEmpty || player2.isEmpty then true
        else false 
        // else previous.Contains (player1.topCard(),player2.topCard())
    member this.getWinnerFromSubGame () : Game2Deck =
        let state = Game2GameState(player1.forSubGame(), player2.forSubGame(),previous)
        let state = state.playGame ()
        state.getWinner ()
    member this.shouldPlaySubGame () : bool =
        let enough1 = player1.hasEnoughCards ()
        let enough2 = player2.hasEnoughCards ()
        enough1 && enough2
    member this.hasBeenSeenBefore () : bool =
        previous.Contains this.CacheKey
    member this.topsToPlayer1 () : Game2GameState =
        let card1 = player1.topCard ()
        let card2 = player2.topCard ()
        let player1 = player1.removeTopCard ()
        let player2 = player2.removeTopCard ()
        let player1 = player1.addCards card1 card2
        this.newState(player1,player2,previous)
    member this.topsToPlayer2 () : Game2GameState =
        let card1 = player1.topCard ()
        let card2 = player2.topCard ()
        let player1 = player1.removeTopCard ()
        let player2 = player2.removeTopCard ()
        let player2 = player2.addCards card2 card1
        this.newState(player1,player2,previous)
    member this.clearPlayer2 () : Game2GameState =
        self.newState(player1,player2.asEmpty (),previous)       
    member this.playRound () : Game2GameState =
        // printfn ("Playround: %A") self 
        if this.hasBeenSeenBefore() then
            self.clearPlayer2 ()
        else if this.shouldPlaySubGame() then
            // printfn "Playing subgame ("
            let winner = self.getWinnerFromSubGame ()
            // printfn ")"
            if winner.is player1 then
                this.topsToPlayer1 ()
            else
                this.topsToPlayer2 ()            
        else
            // printfn "Playing normal"
            let card1 = player1.topCard ()
            let card2 = player2.topCard () 
            if card1 > card2 then
                // printfn " player 1 wins"
                self.topsToPlayer1 ()
            else
                // printfn " player 2 wins"
                self.topsToPlayer2 ()
    member this.playGame () : Game2GameState =
        if this.isGameOver () then self
        else this.playRound().playGame() 
        
        