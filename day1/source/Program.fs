// Learn more about F# at http://fsharp.org

open System
open System.ComponentModel.DataAnnotations.Schema
open System.IO

let listWithTheirNextElement (input : int list) =
    let shifted = (List.skip 1 input) @ [input.[0]]
    List.zip input shifted

let reverseCaptcha (input : int list) =
    input
        |> listWithTheirNextElement
        |> List.filter (fun (a,b) -> a = b)
        |> List.sumBy fst


let splitIntoListOfNumbers (input : string) =
    [for c in input -> c]
        |> List.filter (fun t -> Char.IsDigit(t))
        |> List.map (Char.GetNumericValue >> int)

[<EntryPoint>]
let main argv =
    let result =
        File.ReadAllText("input.txt")
        |> splitIntoListOfNumbers
        |> reverseCaptcha

    printfn "Result: %i" result
    0 // return an integer exit code
