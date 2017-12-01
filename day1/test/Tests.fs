module Tests

open System
open Xunit
open Program


let examplesWithTheNextElement : obj array seq =
    seq {
        yield [| [1;1;1;1]; 4 |]
        yield [| [1;1;2;2]; 3 |]
        yield [| [1;2;3;4]; 0 |]
        yield [| [9;1;2;1;2;1;2;9]; 9 |]
    }

[<Theory; MemberData("examplesWithTheNextElement")>]
let ``If a digit matches the element next in the list, we sum``(input : int list, expected : int) =
    let actual = reverseCaptchaWithNextElement input
    Assert.Equal(expected, actual)



let examplesWithTheHalfwayElement : obj array seq =
    seq {
        yield [| [1; 2; 1; 2]; 6 |]
        yield [| [1; 2; 2; 1]; 0 |]
        yield [| [1; 2;3;4;2;5]; 4 |]
        yield [| [1;2;3;1;2;3]; 12 |]
        yield [| [1;2;1;3;1;4;1;5]; 4 |]
    }

[<Theory; MemberData("examplesWithTheHalfwayElement")>]
let ``If a digit matches the element halfway further in the list, we sum``(input : int list, expected : int) =
    let actual = reverseCaptchaWithHalfwayElement input
    Assert.Equal(expected, actual)


[<Fact>]
let ``Given string of digit returns a list of int``() =
    Assert.Equal<int list>([1;2;3], splitIntoListOfNumbers "123")

[<Fact>]
let ``Given string of digit and other characters ignore other characters``() =
    Assert.Equal<int list>([1;2;3;5], splitIntoListOfNumbers "1\t2 3z5\n")


[<Fact>]
let ``Given a list of digits, return list of tuples with next element``() =
    Assert.Equal<(int*int) list>([(1,2);(2,3);(3,1)], pairedWithNextElement [1;2;3])

[<Fact>]
let ``Given a list of digits, return list of tuples with halfway element``() =
    let actual = pairedWithTheHalfwayElement [1;2;1;2]
    let expected = [(1,1); (2,2); (1,1); (2,2)]
    Assert.Equal<(int*int) list>(expected, actual)


