module Tests

open Xunit
open FsUnit.Xunit
open Program

let examplesForRowDifferences : obj array seq =
    seq {
        yield [| [1; 1];         0 |]
        yield [| [5; 1; 9; 5];   8 |]
        yield [| [7; 5; 3];      4 |]
        yield [| [2; 4; 6; 8];   6 |]
    }

[<Theory; MemberData("examplesForRowDifferences")>]
let ``Can calculate the difference between largest and smallest value in a row`` (row, expected) =
    differenceForRow row |> should equal expected


let examplesForEvenlyDivisibleValues : obj array seq =
    seq {
        yield [| [8;2];         4 |]
        yield [| [2;8];         4 |]
        yield [| [5;9;2;8];     4 |]
        yield [| [9;4;7;3];     3 |]
        yield [| [3;8;6;5];     2 |]
    }

[<Theory; MemberData("examplesForEvenlyDivisibleValues")>]
let ``Can calculate the division of evenly divisible values`` (row, expected) =
    divisionEvenlyDivisibleValues row |> should equal expected


[<Fact>]
let ``Can calculate the checksum by summing the differences of the rows`` () =
    let spreadsheet = [
        [5; 1; 9; 5];
        [7; 5; 3];
        [2; 4; 6; 8];
    ]
    checksumForSpreadsheet spreadsheet |> should equal (8 + 4 + 6)


let examplesForTabDelimitedStrings : obj array seq =
    seq {
        yield [| "15";                  [15]            |]
        yield [| "15\t23\t34\n";        [15; 23; 34]    |]
        yield [| "";                    ([]:int list)   |]
    }

[<Theory; MemberData("examplesForTabDelimitedStrings")>]
let ``Can convert a tab delimited string in a Row``(stringInput: string, expected: Row) =
    stringInput
        |> stringToRow
        |> should equal expected


[<Fact>]
let ``Can convert an array of strings in a spreadsheet``() =
    [| "15\t23\t34\n"; "3\t3";|]
        |> stringsToSpreadsheet
        |> should equal [
            [15; 23; 34];
            [3; 3]
        ]