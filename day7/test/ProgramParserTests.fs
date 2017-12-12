module ProgramParserTests

open Xunit
open ProgramParser
open FsUnit.Xunit
open NHamcrest.Core
open FParsec

let parseTo parsed =
    CustomMatcher<obj>((sprintf "parseable into %A" parsed),
        fun x ->
        match x with
        | :? ParserResult<_,unit> as t ->
            match t with
            | Success(value,_,_) -> value = parsed
            | Failure(_) -> false
        | _ -> false)

let parseWithoutError<'T> =
    CustomMatcher<obj>("parseable",
        fun x ->
        match x with
        | :? ParserResult<'T,unit> as t ->
            match t with
            | Success(_) -> true
            | Failure(_) -> false
        | _ -> false)


[<Theory>]
[<InlineData("pbga")>]
[<InlineData("pbga ")>]
[<InlineData("pbga,")>]
[<InlineData("pbga(4)")>]
let ``Can parse a string into a name ignoring any special characters`` (input:string) =
    input |> run nameParser |> should parseTo (Name "pbga")

[<Theory>]
[<InlineData("1pbga")>]
[<InlineData("?pbga ")>]
[<InlineData("*pbga,")>]
[<InlineData(" pbga")>]
let ``Cannot parse a string into a name starting with weird stuff`` (input:string) =
    input |> run nameParser |> should not' parseWithoutError


[<Theory>]
[<InlineData("(456)", 456)>]
[<InlineData("( 456 )", 456)>]
[<InlineData("(4)", 4)>]
let ``Can parse a number between parentheses into a weight`` (input:string, expected:int) =
    input |> run weightParser |> should parseTo (Weight expected)

[<Theory>]
[<InlineData("()")>]
[<InlineData("(abc)")>]
[<InlineData("(12,32)")>]
[<InlineData("(12.32)")>]
let ``Cannot parse any weird stuff into weight`` (input:string) =
    input |> run weightParser |> should not' parseWithoutError

[<Theory>]
[<InlineData("abc,def")>]
[<InlineData("abc, def")>]
[<InlineData("abc ,def")>]
[<InlineData("abc , def")>]
let ``Can parse a string into a list of names`` (input:string) =
    input |> run listNameParser |> should parseTo [Name "abc"; Name "def"]

[<Fact>]
let ``Can parse a string into a statement without holding programs``() =
    "abc (45)" |> run statementParser |> should parseTo (Name "abc", Weight 45, ([]:NameToken list))

[<Fact>]
let ``Can parse a string into a statement while holding programs``() =
    "abc (45) -> def, ghi" |> run statementParser |> should parseTo (Name "abc", Weight 45, [Name "def"; Name "ghi"])

