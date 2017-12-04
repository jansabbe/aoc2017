open System.IO

let sortLetters = Seq.sort >> Seq.map string >> String.concat ""

let hasDuplicate list = (List.distinct list |> List.length) <> (List.length list)

let phraseToListOfWords (phrase:string) =  phrase.Split(" ") |> List.ofArray

let hasDoubleWord (phrase:string) : bool =
    phrase
        |> phraseToListOfWords
        |> hasDuplicate

let doesNotHaveDoubleWord = (not << hasDoubleWord)

let hasAnagram (phrase:string) : bool =
    phrase
        |> phraseToListOfWords
        |> List.map sortLetters
        |> hasDuplicate

let doesNotHaveAnagram = (not << hasAnagram)

[<EntryPoint>]
let main argv =
    let phrases = File.ReadLines "input.txt"
    let phrasesWithoutDoubleWords =
        phrases
            |> Seq.filter doesNotHaveDoubleWord
            |> Seq.length

    let phrasesWithoutAnagrams =
        phrases
            |> Seq.filter doesNotHaveAnagram
            |> Seq.length

    printfn "Phrases without double words: %A" phrasesWithoutDoubleWords
    printfn "Phrases without anagras: %A" phrasesWithoutAnagrams
    0 // return an integer exit code
