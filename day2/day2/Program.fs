open System.IO
open System.Text.RegularExpressions

type Row = int list
type Spreadsheet = Row list
type Checksummer = Spreadsheet -> int

let differenceForRow (row:Row) : int = (List.max row) - (List.min row)

let allCombinations (row:Row): (int*int) list =
    [for x in row do for y in row -> (x,y)]
        |> List.filter (fun (a,b) -> a <> b)

let isEvenlyDivisible ((a,b):(int*int)) : bool = a % b = 0

let divisionEvenlyDivisibleValues (row: Row) : int =
    let (dividend, divisor) =
        allCombinations row
        |> List.find isEvenlyDivisible
    dividend / divisor

let checksumForSpreadsheet : Checksummer = List.sumBy differenceForRow

let evenlyDivisibleChecksumForSpreadsheet : Checksummer = List.sumBy divisionEvenlyDivisibleValues

let canBeConvertedToInt n = Regex.IsMatch(n, "^[0-9]+")

let stringToRow (input:string) : Row =
    input.Split("\t")
        |> List.ofArray
        |> List.filter canBeConvertedToInt
        |> List.map int

let stringsToSpreadsheet (input: string[]) : Spreadsheet =
    input
        |> List.ofArray
        |> List.map stringToRow

[<EntryPoint>]
let main argv =
    let spreadsheet =
        File.ReadAllLines("input.txt")
        |> stringsToSpreadsheet
    printfn "Checksum for spreadsheet %i" (checksumForSpreadsheet spreadsheet)
    printfn "Evenly divisible checksum for spreadsheet %i" (evenlyDivisibleChecksumForSpreadsheet spreadsheet)
    0
