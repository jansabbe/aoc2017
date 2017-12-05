module Tests

open System
open Xunit
open FsUnit.Xunit
open Program

let listToMap l = l |> List.zip [0 .. (l.Length-1)] |> Map.ofList
let exampleJumps : obj array seq = seq {
    yield [|
        { jumps = [0;3;0;1;-3] |> listToMap ; currentPosition = InsideList 0 };
        { jumps = [1;3;0;1;-3] |> listToMap; currentPosition = InsideList 0 }
    |]
    yield [|
        { jumps = [1;3;0;1;-3]  |> listToMap; currentPosition = InsideList 0 };
        { jumps = [2;3;0;1;-3]  |> listToMap; currentPosition = InsideList 1 }
    |]
    yield [|
        { jumps = [2;3;0;1;-3]  |> listToMap; currentPosition = InsideList 1 };
        { jumps = [2;4;0;1;-3]  |> listToMap; currentPosition = InsideList 4 }
    |]
    yield [|
        { jumps = [2;4;0;1;-3]  |> listToMap; currentPosition = InsideList 4 }
        { jumps = [2;4;0;1;-2]  |> listToMap; currentPosition = InsideList 1 }
    |]
    yield [|
        { jumps = [2;4;0;1;-2]  |> listToMap; currentPosition = InsideList 1 }
        { jumps = [2;5;0;1;-2]  |> listToMap; currentPosition = OutsideList }
    |]
}

[<Theory; MemberData("exampleJumps")>]
let ``Given a jump list can generate the next jump list`` (jumpList:JumpList, expectedJumpList:JumpList) =
    jumpList |> jump alwaysIncreaseByOne |> should equal expectedJumpList

[<Fact>]
let ``Given a jump list, can return how many moves needed to be outside list``() =
    { jumps = [0;3;0;1;-3]  |> listToMap; currentPosition = InsideList 0 } |> jumpsNeeded alwaysIncreaseByOne |> should equal 5

[<Fact>]
let ``Given a jump list, can return how many moves needed to be outside list with alternative strategy``() =
    { jumps = [0;3;0;1;-3]  |> listToMap; currentPosition = InsideList 0 } |> jumpsNeeded increaseOrDecrease |> should equal 10