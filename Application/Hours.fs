module Application.Hours

open System
open Model.Hours

type IHours =
    abstract member addHour : string -> DateTime -> HoursAmount -> Result<unit, string>
    abstract member getAllHours : string -> List<Hours>

let registerHours (hours: IHours) (name: string, date: DateTime, amount: HoursAmount) : Result<unit, string> = hours.addHour name date amount

let getAllHours (hours: IHours) (name: string) : List<Hours> = hours.getAllHours name

let totalHoursFor (hours: IHours) (name: string) : int = hours.getAllHours name |> List.sumBy (fun h -> HoursAmountModule.toInt h.Amount)

let overTimeFor (hours: IHours) (name: string) : int = hours.getAllHours name |> List.sumBy (fun h -> max 0 (HoursAmountModule.toInt h.Amount - 8))

let totalHoursForEmployees (hours: IHours) (employees: List<string>) : int = employees |> List.sumBy (fun e -> totalHoursFor hours e)

let totalOverTimeHoursForEmployees (hours: IHours) (employees: List<string>) : int = employees |> List.sumBy (fun e -> overTimeFor hours e)