namespace AdventOfCode2024

open FsUnitTyped
open FsToolkit.ErrorHandling
open Pinicola.FSharp.SpectreConsole

type Runner<'parsedData, 'result when 'result: equality> = {
    Day: int
    Parser: string -> 'parsedData
    Sample: string option
    UserData: string option
    Part1: Part<'parsedData, 'result> option
    Part2: Part<'parsedData, 'result> option
} with

    static member run name parser (part: Part<'parsedData, 'result> option) (stringData: string option) (getResult: Solutions<'result> -> 'result option) =
        let x =
            option {
                let! part = part
                let! expected = getResult part.Solutions
                let! stringData = stringData
                let parsedData = parser stringData
                let result = part.Solver parsedData
                return (result, expected)
            }

        match x with
        | Some(result, expected) ->
            result |> shouldEqual expected
            AnsiConsole.markupLineInterpolated $"[green]{name} passed[/]"
        | None -> AnsiConsole.markupLineInterpolated $"[red]{name} not run[/]"

    static member exec (runner: Runner<'parsedData, 'result>) =
        let getSampleSolution = _.ForSample
        let getUserDataSolution = _.ForUserData

        Runner.run "part1 / sample" runner.Parser runner.Part1 runner.Sample getSampleSolution
        Runner.run "part1 / user " runner.Parser runner.Part1 runner.UserData getUserDataSolution
        Runner.run "part2 / sample" runner.Parser runner.Part2 runner.Sample getSampleSolution
        Runner.run "part2 / user" runner.Parser runner.Part2 runner.UserData getUserDataSolution

and Part<'parsedData, 'result> = {
    Solver: 'parsedData -> 'result
    Solutions: Solutions<'result>
}

and Solutions<'result> = {
    ForSample: 'result option
    ForUserData: 'result option
}
