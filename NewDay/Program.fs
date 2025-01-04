open System
open System.IO
open System.Xml.Linq

printfn "Enter day number:"
let day = Console.ReadLine() |> int |> sprintf "%02i"

let solutionDirectory = Path.Join(__SOURCE_DIRECTORY__, "..") |> Path.GetFullPath

let projectDirectory = Path.Join(solutionDirectory, "AdventOfCode2024")

let mainProject = Path.Join(projectDirectory, "AdventOfCode2024.fsproj")

let dayDirectory = Path.Join(projectDirectory, $"Day{day}")

Directory.CreateDirectory(dayDirectory) |> ignore

let solutionFile = Path.Join(dayDirectory, "Solution.fs")

let solutionSourceTemplate =
    $"""
    module AdventOfCode2024.Day{day}

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
            failwith "Not implemented"

        let solve2 data =
            failwith "Not implemented"

    [<Test>]
    let Test () =

        Runner.exec {{
            Day = {day}
            Parser = Parser.parse
            Sample = TextFile.Day{day}.example.Text |> Some
            UserData = TextFile.Day{day}.input.Text |> Some
            Part1 =
                Some {{
                    Solver = Solution.solve1
                    Solutions = {{
                        ForSample = None
                        ForUserData = None
                    }}
                }}
            Part2 =
                Some {{
                    Solver = Solution.solve2
                    Solutions = {{
                        ForSample = None
                        ForUserData = None
                    }}
                }}
        }}

    """

(solutionFile, solutionSourceTemplate) |> File.WriteAllText

let exampleFile = Path.Join(dayDirectory, "example")
(exampleFile, "TODO") |> File.WriteAllText

let inputFile = Path.Join(dayDirectory, "input")
(inputFile, "TODO") |> File.WriteAllText

let xDoc = XDocument.Load(mainProject).Root

let itemGroup = xDoc.Element "ItemGroup"

itemGroup.Add(XElement("Compile", XAttribute("Include", $"Day{day}/Solution.fs")))
itemGroup.Add(XElement("Content", XAttribute("Include", $"Day{day}/example")))
itemGroup.Add(XElement("Content", XAttribute("Include", $"Day{day}/input")))

xDoc.Save(mainProject)

printfn $"Day%s{day} created"
