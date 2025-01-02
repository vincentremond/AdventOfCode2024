module AdventOfCode2024.Tests

open AdventOfCode2024.Common
open FluentAssertions
open NUnit.Framework
open FParsec
open FSharp.Data.LiteralProviders

[<RequireQualifiedAccess>]
module Parser =
  let parse =

    let parseLine = (pint32 .>> spaces) .>>. pint32
    runOrFail (sepBy parseLine newline)

[<RequireQualifiedAccess>]
module Solution =

  let solve1 (data: (int * int) list) =
    data
    |> List.map Tuple2.toList
    |> List.transpose
    |> List.map List.sort
    |> List.transpose
    |> List.map ((List.reduce (-)) >> abs)
    |> List.sum

  let solve2 data =
    let (first, second) = List.unzip data
    let countPerValue = List.countBy id (second) |> Map.ofList

    first
    |> List.map (fun v ->
      let count = countPerValue |> Map.tryFindOrDefault v 0
      v * count

    )
    |> List.sum

[<Test>]
let Test1 () =

  {
    Parser = Parser.parse
    Sample = TextFile.example.Text
    UserData = TextFile.input.Text
    Part1 = {
      Solver = Solution.solve1
      ForSample = 11
      ForUserData = Some 2113135
    }
    Part2 =
      Some {
        Solver = Solution.solve2
        ForSample = 31
        ForUserData = Some 19097157
      }
  }
    .run ()
