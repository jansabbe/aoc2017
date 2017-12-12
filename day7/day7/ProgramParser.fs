module ProgramParser
open FParsec

type NameToken = Name of string
type WeightToken = Weight of int
type Statement = NameToken * WeightToken * NameToken list

let nameParser : Parser<NameToken, unit> = identifier (IdentifierOptions()) |>> Name

let weightParser: Parser<WeightToken, unit> =
    let openParen = spaces .>> pstring "(" .>> spaces
    let closedParen = spaces .>> pstring ")" .>> spaces
    between openParen closedParen pint32 |>> Weight

let listNameParser: Parser<NameToken list, unit> =
    sepBy1 nameParser (spaces .>> pstring "," .>> spaces)

let statementParser: Parser<Statement, unit> =
    let arrowParser = spaces >>. pstring "->" >>. spaces
    tuple3 nameParser weightParser ((arrowParser >>. listNameParser <|>% []))