
open System
open System.Collections.Immutable
open System.ComponentModel.DataAnnotations.Schema
open System.IO
open System.Text.RegularExpressions
open System.ComponentModel

type Row = int list
type Spreadsheet = Row list

let differenceForRow (row:Row) : int =
    (List.max row) - (List.min row)


let allCombinations (row:Row): (int*int) list =
    [for x in row do for y in row -> (x,y)]
        |> List.filter (fun (a,b) -> a <> b)

let isEvenlyDivisible ((a,b):(int*int)) : bool =
    a % b = 0

let divisionEvenlyDivisibleValues (row: Row) : int =
    let (dividend, divisor) =
        allCombinations row
        |> List.find isEvenlyDivisible
    dividend / divisor

let checksumForSpreadsheet (spreadsheet:Spreadsheet) : int =
    List.sumBy differenceForRow spreadsheet

let evenlyDivisibleChecksumForSpreadsheet (spreadsheet:Spreadsheet) : int =
    List.sumBy divisionEvenlyDivisibleValues spreadsheet

let stringToRow (input:string) : Row =
    input.Split("\t")
        |> List.ofArray
        |> List.filter (fun s -> Regex.IsMatch(s, "^[0-9]+"))
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
    0 // return an integer exit code
