// Learn more about F# at http://fsharp.org

open System
open FParsec
open System.IO
open ProgramParser
open ProgramTree
open System.Xml.Serialization


let parseLine (line:string) : Statement option =
    match run statementParser line with
    | Success(result, _, _) -> Some result
    | Failure(_) -> None


let isBalanced (programTree:TreeProgram) =
    let nbOfDifferentWeights = programTree.leaves |> List.distinctBy (fun t -> t.totalWeight) |> List.length
    nbOfDifferentWeights <= 1

let isUnbalanced = isBalanced >> not

let rec find (predicate:TreeProgram->bool)  (treeProgram:TreeProgram) =
    let foundInLeaves = List.tryPick (find predicate) treeProgram.leaves
    match foundInLeaves with
    | (Some _) -> foundInLeaves
    | None when predicate treeProgram -> Some treeProgram
    | _ -> None

[<EntryPoint>]
let main argv =
    let statements =
        File.ReadLines("input.txt")
            |> Seq.map parseLine
            |> Seq.choose id
            |> List.ofSeq
    let statementMap = List.fold (fun map ((name, _,_) as s) -> Map.add name s map) Map.empty statements
    let initialStatment = findInitialStatement statements
    let tree = convertToTree statementMap initialStatment
    let unbalanced = find isUnbalanced tree |> Option.get
    let weights = unbalanced.leaves |> List.map (fun t -> (t.totalWeight, t.ownWeight, t.name))
    printfn "Root: %A" initialStatment
    printfn "Weights of unbalanced item: %A" weights
    0 // return an integer exit code
