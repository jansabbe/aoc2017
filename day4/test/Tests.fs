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
