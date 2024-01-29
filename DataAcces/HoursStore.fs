module DataAcces.HourStore

open DataAcces.Store
open Application.Hours
open System
open Model.Hours
open DataAcces.Database


type HoursStore (store: Store) = 
    interface IHours with
       member this.getAllHours(searchName: string) : List<Hours> =
             InMemoryDatabase.all store.hours
                |> Seq.filter (fun (name, _, _) -> name = searchName)
                |> Seq.map (fun (_, date, amount) -> {Date = date; Amount = amount})
                |> Seq.toList
       member this.addHour (name: string) (date: DateTime) (amount: HoursAmount) : Result<unit, string> =
            match InMemoryDatabase.insert (name, date) (name, date, amount) store.hours with
               | Ok _ -> Ok ()
               | Error _ -> Error "Could not add hour"
      

