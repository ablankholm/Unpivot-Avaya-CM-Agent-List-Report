open FSharp.Data
open System
open System.IO
open FSharp.Data.Runtime.IO
open System.Windows.Forms


let TransposeRow (row : CsvRow) =
    seq { for i in 1 .. 120 do
            let temp = row.Item ("Skl " + string(i))
            if temp.Length > 0 then
                yield String.concat "," [string(row.Item 0); string(row.Item 2); string(i); string(row.Item ("Skl " + string(i))); string(row.Item ("Lvl " + string(i)))]}

let TransposeCSV (csv : seq<CsvRow>) = 
    seq { for row in csv do 
            yield String.concat Environment.NewLine (TransposeRow(row)) }


[<EntryPoint>]
[<STAThreadAttribute>]
let main argv = 
    let ofd = new OpenFileDialog(Filter = "Comma Separated Values files (*.csv)|*.csv", Multiselect = false)

    let ofdResult = ofd.ShowDialog()

    let csvData = CsvFile.Load(ofd.FileName).Cache()

    let sfd = new SaveFileDialog(Filter = "Comma Separated Values files (*.csv)|*.csv")
    let sfdResult = sfd.ShowDialog()
    
    File.WriteAllLines(sfd.FileName, 
        seq { yield "Login ID, Name, Slot, Skl, Lvl"; yield! TransposeCSV(csvData.Rows)})

    0 // return an integer exit code
