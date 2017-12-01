open System
open System.IO

let pairedWithElementAtOffset (offset:int) (input : int list) =
    let shifted = (List.skip offset input) @ (List.take offset input)
    List.zip input shifted

let pairedWithNextElement = pairedWithElementAtOffset 1

let pairedWithTheHalfwayElement input =
    pairedWithElementAtOffset (List.length input / 2) input

let reverseCaptcha (pairingAlgorithm: int list -> (int*int) list) (input : int list) =
    input
        |> pairingAlgorithm
        |> List.filter (fun (a,b) -> a = b)
        |> List.sumBy fst

let reverseCaptchaWithNextElement = reverseCaptcha pairedWithNextElement

let reverseCaptchaWithHalfwayElement = reverseCaptcha pairedWithTheHalfwayElement

let splitIntoListOfNumbers (input : string) =
    List.ofSeq input
        |> List.filter Char.IsDigit
        |> List.map (Char.GetNumericValue >> int)

[<EntryPoint>]
let main argv =
    let input =
        File.ReadAllText("input.txt")
        |> splitIntoListOfNumbers

    printfn "Result paired with next element: %i" (reverseCaptchaWithNextElement input)
    printfn "Result paired with halfway element: %i" (reverseCaptchaWithHalfwayElement input)
    0 // return an integer exit code
