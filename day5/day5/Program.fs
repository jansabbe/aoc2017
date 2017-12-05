// Learn more about F# at http://fsharp.org

open System
open System.IO

type Position = InsideList of int | OutsideList
type JumpList = {jumps : Map<int,int>; currentPosition: Position}

let alwaysIncreaseByOne (index:int) (jumps:Map<int,int>) =
    let newValue = jumps.[index] + 1
    jumps |> Map.add index newValue

let increaseOrDecrease (index:int) (jumps:Map<int,int>) =
    let oldValue = jumps.[index]
    let newValue = if oldValue >= 3 then oldValue - 1 else oldValue + 1
    jumps |> Map.add index newValue

let newPosition (jumps:Map<int,int>) (index:int)  =
    match (index + (jumps.[index])) with
    | x when Map.containsKey x jumps -> InsideList x
    | _ -> OutsideList

let jump offsetChanger ({jumps=jumps; currentPosition = position}) =
    match position with
    | OutsideList -> {jumps=jumps; currentPosition=position}
    | InsideList i -> {jumps = (jumps |> offsetChanger i); currentPosition=(newPosition jumps i)}

let isJumpedOutside ({jumps=_; currentPosition = position}) =
    position = OutsideList

let rec recJumpsNeeded (current:int) offsetChanger jumpList =
    if isJumpedOutside jumpList
    then current
    else recJumpsNeeded (current+1) offsetChanger (jump offsetChanger jumpList)

let jumpsNeeded = recJumpsNeeded 0

[<EntryPoint>]
let main argv =
    let stack = File.ReadLines("input.txt") |> Seq.map int |> List.ofSeq
    let map = stack |> List.zip [0 .. (stack.Length - 1)] |> Map.ofList
    printfn "Jumps needed: %A" <| jumpsNeeded alwaysIncreaseByOne {jumps = map; currentPosition = InsideList 0}
    printfn "Jumps needed weirdo offsetter: %A" <| jumpsNeeded increaseOrDecrease {jumps = map; currentPosition = InsideList 0}
    0 // return an integer exit code
