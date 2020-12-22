module day22.Deck 

type Card = int 

type Deck (player:int, cards:Card[]) =
    override this.ToString () = sprintf "Deck(player:%d cards=%A)" player cards
    override this.Equals (other) =
        match other with
        | :? Deck as deck -> (player,cards) = (deck.Player,deck.Cards)
        | _ -> false 
    override this.GetHashCode () = hash(player,cards)
    member this.Player = player
    member this.Cards : Card[] = cards
    member this.addCard (c:Card) =
        Deck (player, Array.append cards [|c|])
    member this.addCards (card1:Card) (card2:Card)  =
        Deck (player, Array.append cards [|card1;card2|])
    member this.isEmpty = cards.Length = 0
    member this.hasMoreCards = cards.Length > 1
    member this.topCard () = cards.[0]
    member this.removeTopCard () = Deck(player, cards |> Array.tail)
    member this.score () : uint64 =
        let cardScore (i:int) (c:Card) = ((i |> uint64) + 1UL) * (c |> uint64) 
        cards |> Seq.rev |> Seq.mapi cardScore |> Seq.sum
    member this.hasEnoughCards () = this.topCard () < cards.Length
    member this.hasTooFewCards () = this.hasEnoughCards() |> not  
    member this.is (deck: Deck) = player = deck.Player
    member this.forSubGame () = 
        let cards = cards.[1..cards.[0]]
        Deck(player,cards)
    member this.asEmpty () = Deck (player, Array.empty)

type Decks (deck1:Deck, deck2:Deck) =
    member this.Deck1 = deck1
    member this.Deck2 = deck2 