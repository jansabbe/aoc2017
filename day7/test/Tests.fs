module Tests

open System
open Xunit
open Program
open FsUnit.Xunit
open ProgramTree

[<Fact>]
let ``No leaves means always balanced``() =
    isBalanced {name="name";ownWeight=33;totalWeight=33;leaves=[]} |> should equal true

[<Fact>]
let ``Different weights in leaves means unbalanced``() =
    isBalanced {name="name";ownWeight=33;totalWeight=33;leaves=
    [
        {name="name";ownWeight=33;totalWeight=33;leaves=[]};
        {name="name";ownWeight=33;totalWeight=22;leaves=[]}
    ]} |> should equal false

[<Fact>]
let ``One single kinda weight in leaves means balanced``() =
    isBalanced {name="name";ownWeight=33;totalWeight=33;leaves=
    [
        {name="name";ownWeight=33;totalWeight=33;leaves=[]};
        {name="name";ownWeight=33;totalWeight=33;leaves=[]}
    ]} |> should equal true

[<Fact>]
let ``Can find something in tree depth first``() =
    let tree : TreeProgram = {name="root";ownWeight=33;totalWeight=33;leaves=
    [
        {name="leaf";ownWeight=7;totalWeight=33;leaves=[]};
        {name="name";ownWeight=33;totalWeight=33;leaves=[]}
    ]}
    find (fun t -> t.ownWeight = 7) tree |> should equal (Some {name="leaf";ownWeight=7;totalWeight=33;leaves=[]})