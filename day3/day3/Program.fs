// Learn more about F# at http://fsharp.org

open System
open Grid

[<EntryPoint>]
let main argv =
    let distance = distance 265149
    printfn "Distance part1 %i" distance
    0 // return an integer exit code
