// Learn more about F# at http://fsharp.org

open System
open System.IO
open System.Threading

let sortLetters : string -> string = List.ofSeq >> List.map string >> List.sort >> String.concat ""

let isAnagram (word1:string) (word2:string) : bool =
    (sortLetters word1) = (sortLetters word2)

let rec allCombinations list =
  match list with
    | [] -> []
    | x::xs -> (List.map (fun t -> x,t) xs) @ allCombinations xs

let phraseToListOfWords (phrase:string) =  phrase.Split(" ") |> List.ofArray

let hasAnagram (phrase:string) : bool =
    phrase
        |> phraseToListOfWords
        |> allCombinations
        |> List.exists (fun (word1,word2) -> isAnagram word1 word2)


let hasDoubleWord (phrase:string) : bool =
    phrase
        |> phraseToListOfWords
        |> List.groupBy id
        |> List.exists (fun (_,wordList) -> List.length wordList > 1)

let isValid = (not << hasDoubleWord)

[<EntryPoint>]
let main argv =
    let phrases = File.ReadLines "input.txt"
    let phrasesWithoutDoubleWords =
        phrases
            |> Seq.sumBy (fun phrase -> if isValid phrase then 1 else 0)
    let phrasesWithoutAnagrams =
        phrases
            |> Seq.sumBy (fun phrase -> if not <| hasAnagram phrase then 1 else 0)

    printfn "Phrases without double words: %A" phrasesWithoutDoubleWords
    printfn "Phrases without anagras: %A" phrasesWithoutAnagrams
    0 // return an integer exit code
