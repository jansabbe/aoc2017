module Tests

open System
open Xunit
open FsUnit.Xunit
open Program



[<Fact>]
let ``Can remove blocks at index`` () =
    removeBlocksAtIndex 1 [1;2;3;4] |> should equal [1;0;3;4]

let spreadingOutExamples : obj array seq = seq {
    // spreading out zero does nothing
    yield [|
        0; 0; [0;2;7;0]; [0;2;7;0]
    |]
    yield [|
        0; 1; [0;2;7;0]; [0;2;7;0]
    |]
    yield [|
        0; 3; [0;2;7;0]; [0;2;7;0]
    |]
    // spreading out 1 adds one to the next value. When index at end of string, cycle back to beginning
    yield [|
        1; 1; [0;2;7;0]; [0;2;8;0]
    |]
    yield [|
        1; 3; [0;2;7;0]; [1;2;7;0]
    |]
    // spreading out n adds one to the next n values
    yield [|
        2; 1; [0;2;7;0]; [0;2;8;1]
    |]
    yield [|
        3; 3; [0;2;7;0]; [1;3;8;0]
    |]
}

[<Theory; MemberData("spreadingOutExamples")>]
let ``Can spread out values`` (value: int, index: int, memoryBank : int list, expectedBank : int list) =
    memoryBank |> spreadOutValue value index |> should equal expectedBank


let memoryBankExamples : obj array seq = seq {
    yield [|
        [0;2;7;0];
        [2;4;1;2]
    |]

    yield [|
        [2;4;1;2];
        [3;1;2;3]
    |]

    yield [|
        [3;1;2;3];
        [0;2;3;4]
    |]

    yield [|
        [1;3;4;1];
        [2;4;1;2]
    |]
}

[<Theory; MemberData("memoryBankExamples")>]
let ``Can rebalance memory banks`` (memoryBank : int list, expectedBank : int list) =
    memoryBank |> rebalance |> should equal expectedBank


[<Fact>]
let ``Can find step and memorybank for which rebalancing will loop`` () =
    [0;2;7;0] |> findMemoryBankWhichWillLoop |> should equal (5, [2;4;1;2])

[<Fact>]
let ``Looping once`` () =
    [2;4;1;2] |> findMemoryBankWhichWillLoop |> should equal (4, [2;4;1;2])