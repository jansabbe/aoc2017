// Learn more about F# at http://fsharp.org

open System
open System.IO

let isInvalid (phrase:string) : bool =
    phrase.Split(" ")
        |> List.ofArray
        |> List.groupBy id
        |> List.exists (fun (_,wordList) -> List.length wordList > 1)

let isValid = (not << isInvalid)

[<EntryPoint>]
let main argv =
    let result =
        File.ReadLines "input.txt"
            |> Seq.sumBy (fun phrase -> if isValid phrase then 1 else 0)

    printfn "Valid phrases: %A" result
    0 // return an integer exit code
