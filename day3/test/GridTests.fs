module GridTests

open System
open Xunit
open Grid
open FsUnit.Xunit
open FsUnit.CustomMatchers
open FsUnit.Xunit


let exampleSizeGrids : obj array seq =
    seq {
        yield [| 1;  1 |]
        yield [| 9;  3 |]
        yield [| 8;  3 |]
        yield [| 12; 5 |]
    }

[<Theory; MemberData("exampleSizeGrids")>]
let ``Given a number returns the needed size for the grid`` (number:int, expectedSize: int) =
    neededSizeForValue number |> should equal expectedSize

[<Fact>]
let ``Given a small grid can create an extra layer around it``() =
    let smallGrid : Grid = array2D [ [ 98 ] ]
    growGrid smallGrid
    |> should equal (
        array2D [
            [ 0; 0; 0];
            [ 0; 98; 0];
            [ 0; 0; 0];
        ])

[<Fact>]
let ``Can get the coordinates following the outer layer``() =
    coordinatesOuterLayerFollowingSpiral 3 |> should equal
        [ (2,1); (2,0); (1,0);(0,0); (0,1); (0,2); (1,2); (2,2)]

[<Fact>]
let ``Can get the coordinates following spiral``() =
    coordinatesFollowingSpiral 3 |> should equal
        [ (1, 1); (2,1); (2,0); (1,0);(0,0); (0,1); (0,2); (1,2); (2,2)]


[<Fact>]
let ``Grid of with value 1 is just single element grid``() =
    generateGridWithValue 1 |> should equal (array2D [[1]])

[<Fact>]
let ``Grid with value 3 3``() =
    generateGridWithValue 3
    |> should equal (
        array2D [
            [5; 4; 3];
            [6; 1; 2];
            [7; 8; 9]
        ])

[<Fact>]
let ``Can get the coordinates of a value``() =
    let grid = generateGridWithValue 11
    coordinatesForValue 11 grid |> should equal (4, 2)

[<Fact>]
let ``Sum neighbours``() =
    let grid = (
        array2D [
            [0; 4; 2];
            [0; 1; 1];
            [0; 0; 0]
        ])

    sumNeighbours grid (0, 0) |> should equal 5

let exampleDistances : obj array seq =
    seq {
        yield [| 1;     0 |]
        yield [| 12;    3 |]
        yield [| 23;    2 |]
        yield [| 1024; 31 |]
    }

[<Theory; MemberData("exampleDistances")>]
let ``Given a grid and value returns distance from center`` (value:int, expectedDistance: int) =
    distance value  |> should equal expectedDistance


[<Fact>]
let ``Grid with sum neighbours``() =
    generateGridWithSumNeighbours 3 |> should equal (
        array2D [
            [5;   4;  2];
            [10;  1;  1];
            [11; 23; 25]
        ])


let exampleFirstValueLargerThanInput : obj array seq =
    seq {
        yield [| 5;     10 |]
        yield [| 10;    11 |]
        yield [| 20;    23 |]
    }

[<Theory; MemberData("exampleFirstValueLargerThanInput")>]
let ``What is first value larger than input`` (value:int, expected: int) =
    firstValueLargerThanInput value  |> should equal expected
