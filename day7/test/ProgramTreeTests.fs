module ProgramTreeTests

open Xunit
open FsUnit.Xunit
open ProgramParser
open ProgramTree



[<Fact>]
let ``can find initial statement``() =
    let leaf = (Name "pbga", Weight 45, ([]:NameToken list))
    let leafRoot = (Name "jules", Weight 45, [Name "pbga"])
    let root = (Name "root", Weight 45, [Name "jules"])
    findInitialStatement [leafRoot;root;leaf] |> should equal root

[<Fact>]
let ``Can convert list of statements in a tree``() =
    let leaf = (Name "pbga", Weight 3, ([]:NameToken list))
    let leafRoot = (Name "jules", Weight 2, [Name "pbga"])
    let root = (Name "root", Weight 1, [Name "jules"])
    let map =
        Map.empty
            |> Map.add (Name "pbga") leaf
            |> Map.add (Name "jules") leafRoot
            |> Map.add (Name "root") root

    convertToTree map root |> should equal {name = "root"; ownWeight = 1; totalWeight = 6; leaves =
        [
            {name = "jules"; ownWeight = 2; totalWeight = 5; leaves =
            [
                {name = "pbga"; ownWeight = 3; totalWeight = 3; leaves = []}
            ]}
        ]}