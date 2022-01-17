open System.IO
open type System.Environment
open FSharp.Data

let puts (str: string) = printfn $"{str}"

let logDir =
  [| GetFolderPath(SpecialFolder.Personal)
     "Library/Application Support/com.wizards.mtga/Logs/Logs" |]
  |> Path.Combine

let logFiles = Directory.GetFiles logDir
let mostRecentLogFile = (logFiles |> Array.head)

let parseLine (line: string) =
  let parts = line.Split "==> "

  if parts.Length = 1 then
    let otherParts = line.Split "<== "

    if otherParts.Length = 1 then
      None
    else
      Some otherParts.[1]
  else
    Some parts.[1]

let parseLineIntoEntry (line: string) =
  //[id] [UnityCrossThreadLogger]==> MessageName JSONString
  //[id] [UnityCrossThreadLogger]<== MessageName JSONString
  let parts = parseLine line

  if parts.IsNone then
    None
  else
    let subParts = parts.Value.Split(" ")

    Some
      {| MessageName = subParts.[0]
         Payload = subParts.[1] |}

let testReadFile () =
  File.ReadLines mostRecentLogFile
  |> Seq.filter (fun line ->
    line.Split(" ")
    |> Array.exists (fun part -> part.IndexOf("UnityCrossThreadLogger") > -1))
  |> Seq.map parseLineIntoEntry
  |> Seq.filter (fun e -> e.IsSome)
  |> Seq.iter (fun e -> puts e.Value.Payload)

// testReadFile()

[<Literal>]
let ResolutionFolder = __SOURCE_DIRECTORY__

type DraftStats = CsvProvider<"data/stx-quick-draft.tsv", ResolutionFolder=ResolutionFolder>
let draftStats = new DraftStats()

type CardData = CsvProvider<"data/card_list.csv", ResolutionFolder=ResolutionFolder>
let cards = new CardData()

for row in draftStats.Rows do
  let filtered =
    (cards.Filter(fun card -> card.Name = row.Name))
      .Rows
    |> Seq.head

  puts $"{row.Name} {filtered.Id}"
