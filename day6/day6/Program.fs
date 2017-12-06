// Learn more about F# at http://fsharp.org

open System
open System.Reflection.Metadata
type MemoryBank = int list

let setValueAtIndex (newValue:int->int) (index:int) (memoryBank:MemoryBank) : MemoryBank =
    memoryBank |> List.mapi (fun i value -> if i = index then newValue value else value)

let removeBlocksAtIndex = setValueAtIndex (fun _ -> 0)

let rec spreadOutValue (value:int) (index:int) (memoryBank:MemoryBank) =
    let nextIndex = (index+1) % (List.length memoryBank)
    match value with
    | 0 -> memoryBank
    | n -> memoryBank
        |> setValueAtIndex ((+) 1) nextIndex
        |> spreadOutValue (n - 1) nextIndex

let rebalance (memoryBank:MemoryBank) =
    let maxValue = List.max memoryBank
    let firstOcurringIndexOfMax = List.findIndex ((=) maxValue) memoryBank
    memoryBank
        |> removeBlocksAtIndex firstOcurringIndexOfMax
        |> spreadOutValue maxValue (firstOcurringIndexOfMax)



let findMemoryBankWhichWillLoop (memoryBank:MemoryBank) =
    let rec recFindMemoryBankWhichWillLoop (step:int) (memoryBanks : MemoryBank list) =
        match memoryBanks with
        | (x::xs) when List.contains x xs -> (step, x)
        | (x::_) -> recFindMemoryBankWhichWillLoop (step + 1) ((rebalance x)::memoryBanks)
        | [] -> (step, [])
    recFindMemoryBankWhichWillLoop 0 [memoryBank]

[<EntryPoint>]
let main argv =
    let input = [11;11;13;7;0;15;5;5;4;4;1;1;7;1;15;11]
    let (step, memoryBank) = findMemoryBankWhichWillLoop input
    printfn "Count Max rebalances: %A" <| step
    printfn "Count Max rebalances2: %A" <| (findMemoryBankWhichWillLoop memoryBank |> fst)
    0 // return an integer exit code
