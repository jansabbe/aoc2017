
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


let checksumForSpreadsheet (spreadsheet:Spreadsheet) : int =
    List.sumBy differenceForRow spreadsheet

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
    let checksum =
        File.ReadAllLines("input.txt")
        |> stringsToSpreadsheet
        |> checksumForSpreadsheet
    printfn "Checksum for spreadsheet %i" checksum
    0 // return an integer exit code
