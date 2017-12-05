// Learn more about F# at http://fsharp.org

open System
open System.IO

type Position = InsideList of int | OutsideList
type JumpList = {jumps : int list; currentPosition: Position}

let increaseAt (index:int) (jumps:int list) =
    jumps
        |> List.mapi (fun i value -> if i = index then value + 1 else value)

let newPosition (jumps:int list) (index:int)  =
    match (index + (jumps.[index])) with
    | x when x < 0 -> OutsideList
    | x when x >= List.length jumps -> OutsideList
    | x -> InsideList x


let jump ({jumps=jumps; currentPosition = position}) =
    match position with
    | OutsideList -> {jumps=jumps; currentPosition=position}
    | InsideList i -> {jumps = (jumps |> increaseAt i); currentPosition=(newPosition jumps i)}

let isJumpedOutside ({jumps=_; currentPosition = position}) =
    position = OutsideList


let jumpsNeeded jumpList =
    Seq.unfold (fun state -> Some(state, jump state)) jumpList
        |> Seq.findIndex isJumpedOutside

[<EntryPoint>]
let main argv =
    let stack = File.ReadLines("input.txt") |> Seq.map int |> List.ofSeq
    let result = jumpsNeeded {jumps = stack; currentPosition = InsideList 0}
    printfn "Jumps needed: %A" result
    0 // return an integer exit code
