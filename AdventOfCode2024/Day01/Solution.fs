﻿module AdventOfCode2024.Day01

open NUnit.Framework
open FParsec
open FSharp.Data.LiteralProviders
open Pinicola.FSharp
open Pinicola.FSharp.FParsec

[<RequireQualifiedAccess>]
module Parser =
    let parse =

        let parseLine = (pint32 .>> spaces) .>>. pint32
        String.trim >> parseOrFail ((sepBy parseLine newline) .>> eof)

[<RequireQualifiedAccess>]
module Solution =

    let solve1 data =
        data
        |> List.map Tuple2.toList
        |> List.transpose
        |> List.map List.sort
        |> List.transpose
        |> List.map ((List.reduce (-)) >> abs)
        |> List.sum

    let solve2 data =
        let first, second = List.unzip data
        let countPerValue = List.countBy id second |> Map.ofList

        first
        |> List.map (fun v ->
            let count = countPerValue |> Map.tryFindOrDefault v 0
            v * count

        )
        |> List.sum

[<Test>]
let Test () =

    Runner.exec {
        Day = 1
        Parser = Parser.parse
        Sample = TextFile.Day01.example.Text |> Some
        UserData = TextFile.Day01.input.Text |> Some
        Part1 =
            Some {
                Solver = Solution.solve1
                Solutions = {
                    ForSample = Some 11
                    ForUserData = Some 2113135
                }
            }
        Part2 =
            Some {
                Solver = Solution.solve2
                Solutions = {
                    ForSample = Some 31
                    ForUserData = Some 19097157
                }
            }
    }
