module Model.Hours

open System


type HoursAmount = private | HoursAmount of int

let (|HoursAmount|) (HoursAmount amount) = amount


//module voor het aanmaken van custom hoursamount
[<RequireQualifiedAccess>]
module HoursAmountModule = 
    let create (amount: int) =
        if amount >= 0 && amount <= 16 then Ok (HoursAmount amount)
        else Error "Amount kan niet negatief zijn en niet hoger dan 16 zijn"
        
    let toInt (amount: HoursAmount) =
        match amount with HoursAmount a -> a
    
    let fromInt (value: int) : HoursAmount =
        match create value with
        | Ok amount -> amount
        | Error errorMessage -> failwith errorMessage


type Hours = { Date: DateTime; Amount: HoursAmount }
