module Tests

open System
open Xunit
open Program

[<Fact>]
let ``Given 1111 should produce 4`` () =
    let x = [1;1;1;1]
    Assert.Equal(4, reverseCaptcha(x))

[<Fact>]
let ``Given 1122 should produce 3``() =
    Assert.Equal(3, reverseCaptcha([1;1;2;2]))


[<Fact>]
let ``Given 1234 should produce 0``() =
    Assert.Equal(0, reverseCaptcha([1;2;3;4]))

[<Fact>]
let ``Given 91212129 should produce 9``() =
    Assert.Equal(9, reverseCaptcha([9;1;2;1;2;1;2;9]))

[<Fact>]
let ``Given string of digit returns a list of int``() =
    Assert.Equal<int list>([1;2;3], splitIntoListOfNumbers("123"))

[<Fact>]
let ``Given string of digit and other characters ignore other characters``() =
    Assert.Equal<int list>([1;2;3;5], splitIntoListOfNumbers("1\t2 3z5\n"))