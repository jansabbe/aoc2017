open System
open FParsec
open System.IO

type Register = Register of string
type Condition =
    GreaterThan of (Register * int)
    | GreaterOrEquals of (Register * int)
    | Equals of (Register * int)
    | NotEquals of (Register * int)
    | LessThan of (Register * int)
    | LessOrEquals of (Register * int)
type Action =
    | Increment of (Register * int)
    | Decrement of (Register * int)
type Statement = (Action * Condition)


let registerParser = identifier (IdentifierOptions()) |>> Register

let infixParser (expectedOperator:string) ctor =
    let operator = between spaces spaces (pstring expectedOperator)
    attempt <| pipe2 (registerParser .>> operator) pint32 (fun a (b:int) -> ctor (a, b))

let conditionParser : Parser<Condition,unit> =
    let ifParser = spaces >>. pstring "if" >>. spaces
    ifParser >>. ((infixParser "<" LessThan)
                <|> (infixParser "<=" LessOrEquals)
                <|> (infixParser "==" Equals)
                <|> (infixParser "!=" NotEquals)
                <|> (infixParser ">" GreaterThan)
                <|> (infixParser ">=" GreaterOrEquals))

let actionParser : Parser<Action,unit> =
    (infixParser "inc" Increment) <|> (infixParser "dec" Decrement)

let statementParser =
    tuple2 actionParser conditionParser

let parseStatement (input:string) : Statement option =
    match run statementParser input with
    | Success(result, _, _) -> Some result
    | Failure(_) -> None


let runCondition (condition:Condition) (registerMap:Map<Register,int>) : bool =
    let valueInRegister register = Map.tryFind register registerMap |> Option.defaultValue 0
    match condition with
    | GreaterThan (register,value) -> (valueInRegister register) > value
    | GreaterOrEquals (register,value) -> (valueInRegister register) >= value
    | Equals (register,value) -> (valueInRegister register) = value
    | NotEquals (register,value) -> (valueInRegister register) <> value
    | LessThan (register,value) -> (valueInRegister register) < value
    | LessOrEquals (register,value) -> (valueInRegister register) <= value

let runAction (action:Action) (registerMap:Map<Register,int>) : Map<Register,int> =
    let valueInRegister register = Map.tryFind register registerMap |> Option.defaultValue 0
    match action with
    | Increment (register,value) ->
        let newValue = (valueInRegister register) + value
        Map.add register newValue registerMap
    | Decrement (register,value) ->
        let newValue = (valueInRegister register) - value
        Map.add register newValue registerMap


let runStatement (registerMap:Map<Register,int>) ((action,condition):Statement) =
    if runCondition condition registerMap
    then runAction action registerMap
    else registerMap

let runStatements = List.fold runStatement Map.empty

type State = {map:Map<Register,int>; max:int}
let runStatementsAndKeepTrackOfMax =
    let initialState = {map=Map.empty; max=0}
    let runit {map=map;max=max} statement =
        let newMap = (runStatement map statement)
        let maxInMap = newMap |> Map.toList |> List.maxBy snd |> snd
        {map= newMap; max = if maxInMap > max then maxInMap else max }
    List.fold runit initialState

[<EntryPoint>]
let main argv =
    let statements =
        File.ReadLines("input.txt")
            |> Seq.map parseStatement
            |> Seq.choose id
            |> List.ofSeq
    let {map=registerMap; max=max} = runStatementsAndKeepTrackOfMax statements
    let maxTuple = registerMap |> Map.toList |> List.maxBy snd
    printfn "Largest register after last statement %A" maxTuple
    printfn "The very biggest value: %A" max
    0 // return an integer exit code
