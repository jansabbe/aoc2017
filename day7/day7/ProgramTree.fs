module ProgramTree
open ProgramParser

let isStatementBeingReferredFromOtherStatement (allStatements:Statement list) ((name, _, _):Statement)  : bool =
    let statementRefersToName (_, _, references) = List.contains name references
    allStatements |> List.exists statementRefersToName

let isInitial (allStatements: Statement list) = isStatementBeingReferredFromOtherStatement allStatements >> not

let findInitialStatement (allStatements: Statement list) = List.find (isInitial allStatements) allStatements


type TreeProgram = {name:string; ownWeight:int; totalWeight:int; leaves: TreeProgram list}

let rec convertToTree (statements:Map<NameToken, Statement>) (((Name name),(Weight weight),subnames):Statement) =
    let leaves = subnames |> List.map (fun t -> statements.[t] |> convertToTree statements)
    let totalWeightLeaves = List.sumBy (fun t -> t.totalWeight) leaves
    {name=name; ownWeight=weight; totalWeight=(weight + totalWeightLeaves); leaves=leaves}