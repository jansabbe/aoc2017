module Tests

open System
open Xunit
open FsUnit.Xunit
open Program

let examplePassphrases : obj array seq = seq {
    yield [| "aa bb cc dd ee"; true |]
    yield [| "aa bb cc dd aa"; false |]
    yield [| "aa bb cc dd aaa"; true |]
}

[<Theory; MemberData("examplePassphrases")>]
let ``in a valid passphrase words are not duplicated`` (phrase:string, expectedIsValid:bool) =
    (isValid phrase) |> should equal expectedIsValid


let exampleAnagram : obj array seq = seq {
    yield [| "aa"; "bb"; false |]
    yield [| "abcde"; "ecdab"; true |]
    yield [| "abd"; "ab"; false |]
    yield [| "oiii"; "ioii"; true |]
}

[<Theory; MemberData("exampleAnagram")>]
let ``can check if two words are anagram`` (word1:string, word2:string, expectedIsAnagram:bool) =
    (isAnagram word1 word2) |> should equal expectedIsAnagram




let exampleAnagramPhrases : obj array seq = seq {
    yield [| "abcde fghij"; false |]
    yield [| "abcde xyz ecdab"; true |]
    yield [| "a ab abc abd abf abj"; false |]
    yield [| "iiii oiii ooii oooi oooo"; false |]
    yield [| "oiii ioii iioi iiio"; true |]
}

[<Theory; MemberData("exampleAnagramPhrases")>]
let ``can check if pass phrase has an anagram``(phrase:string, expected:bool) =
    hasAnagram phrase |> should equal expected