module GridTests

open System
open Xunit
open Grid
open FsUnit.Xunit
open FsUnit.CustomMatchers


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
let ``Can get the coordinates the outer layer in descending order``() =
    coordinatesOuterLayerFollowingSpiral 3 |> should equal
        [ (2,1); (2,0); (1,0);(0,0); (0,1); (0,2); (1,2); (2,2)]

[<Fact>]
let ``Fill in outer layer``() =
    let grid = (
        array2D [
            [ 0; 0; 0];
            [ 0; 98; 0];
            [ 0; 0; 0];
        ])

    fillInOuterLayer grid |> should equal (
        array2D [
            [ 5; 4;  3];
            [ 6; 98; 2];
            [ 7; 8;  9];
        ])



[<Fact>]
let ``Grid of size 1 is just single element grid``() =
    generateGridForSize 1 |> should equal (array2D [[1]])

[<Fact>]
let ``Grid of size 3``() =
    generateGridForSize 3
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
