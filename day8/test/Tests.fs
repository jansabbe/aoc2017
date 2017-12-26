module Tests

open System
open Xunit
open Program
open FsUnit.Xunit

let parsingExamples : obj array seq = seq {
    yield [|
        "b inc 5 if a > 1";
        (Increment (Register "b", 5), GreaterThan (Register "a", 1))
    |]
    yield [|
        "a inc 1 if b < 5";
        (Increment (Register "a", 1), LessThan (Register "b", 5))
    |]
    yield [|
        "c dec -10 if a >= 1";
        (Decrement (Register "c", -10), GreaterOrEquals (Register "a", 1))
    |]
    yield [|
        "c inc -20 if c == 10";
        (Increment (Register "c", -20), Equals (Register "c", 10))
    |]
}

[<Theory; MemberData("parsingExamples")>]
let ``Can parse string into statements`` (input:string, expected:Statement) =
    input |> parseStatement |> should equal (Some expected)


[<Fact>]
let ``Can execute a statement on empty registers``() =
    let statement = (Increment (Register "c", 20), Equals (Register "c", 0))
    let registerMap : Map<Register,int> = Map.empty
    let expectedMap : Map<Register,int> = [(Register "c", 20)] |> Map.ofList
    runStatement  registerMap statement |> should equal expectedMap

[<Fact>]
let ``Can run sample``() =
    let statements = [
        (Increment (Register "b", 5), GreaterThan (Register "a", 1));
        (Increment (Register "a", 1), LessThan (Register "b", 5));
        (Decrement (Register "c", -10), GreaterOrEquals (Register "a", 1));
        (Increment (Register "c", -20), Equals (Register "c", 10))
    ]
    let expectedMap : Map<Register,int> = [(Register "a", 1); (Register "c", -10)] |> Map.ofList
    runStatements statements |> should equal expectedMap