module AdventOfCode2024.Day02

open NUnit.Framework
open FParsec
open FSharp.Data.LiteralProviders
open Pinicola.FSharp
open Pinicola.FSharp.FParsec

[<RequireQualifiedAccess>]
module Parser =
    let parse =

        let parseLine = sepBy1 pint32 (pchar ' ')
        String.trim >> parseOrFail ((sepBy1 parseLine newline) .>> eof)

[<RequireQualifiedAccess>]
module Solution =

    type State1 =
        | SafeIncrease
        | SafeDecrease
        | Unsafe

    let solve1 (data: int list list) =

        let checkLine (line: int list) =
            line
            |> List.pairwise
            |> List.map (fun (a, b) ->
                let diff = b - a

                if 1 <= diff && diff <= 3 then State1.SafeIncrease
                elif -3 <= diff && diff <= -1 then State1.SafeDecrease
                else State1.Unsafe
            )
            |> List.reduce (fun a b -> if a = b then a else State1.Unsafe)

        data
        |> List.map checkLine
        |> List.sumBy (
            function
            | State1.SafeIncrease -> 1
            | State1.SafeDecrease -> 1
            | State1.Unsafe -> 0
        )

    let solve2 data =

        let checkLine (line: int list) =
            let rec checkLine' line dampened state prev =
                match line with
                | [] -> Option.get state
                | x::xs ->
                    let diff = x - prev

                    

                    if 1 <= diff && diff <= 3 then
                        checkLine' xs x (Option.map ((+) 1) state)
                    else
                        checkLine' xs x state

            let head, tail = List.head line, List.tail line
            checkLine' line false None head

        data
        |> List.map checkLine
        |> List.sumBy (
            function
            | State1.SafeIncrease -> 1
            | State1.SafeDecrease -> 1
            | State1.Unsafe -> 0
        )

[<Test>]
let Test () =

    Runner.exec {
        Day = 02
        Parser = Parser.parse
        Sample = TextFile.Day02.example.Text |> Some
        UserData = TextFile.Day02.input.Text |> Some
        Part1 =
            Some {
                Solver = Solution.solve1
                Solutions = {
                    ForSample = Some 2
                    ForUserData = Some 236
                }
            }
        Part2 =
            Some {
                Solver = Solution.solve2
                Solutions = {
                    ForSample = Some 4
                    ForUserData = None
                }
            }
    }
