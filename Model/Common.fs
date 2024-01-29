module Common

open System.Text.RegularExpressions
open System

type DepartmentID = private | CheckDepartmentID of string

let (|CheckDepartmentID|) (CheckDepartmentID departmentID) = departmentID

//module voor het aanmaken van de DeparmentID
[<RequireQualifiedAccess>]
module CheckDepartmentIDModule = 
    
    // Specifieke regex voor de departmentId en dus niet voor in de validatie
    let private validDepartmentId = Regex(@"^[A-Z]{4}[0-9]{2}$")

    // "makeDepartmentId" is een functie die een ruwe waarde omzet naar het Type DepartmentId.
    // Hierin zit ook de validatie.
    let makeDepartmentId (rawDepartmentId: string) =
        if String.IsNullOrWhiteSpace(rawDepartmentId) then
            Result.Error "DepartmentId mag niet leeg zijn"
        elif validDepartmentId.IsMatch(rawDepartmentId) then
            Result.Ok (CheckDepartmentID rawDepartmentId)
        else
            Result.Error "DepartmentId moet bestaan uit vier hoofdletters gevolgd door twee cijfers"

    // "createDepartmentId" is een functie die een waarde omzet naar het Type DepartmentId met een result type voor error handeling.
    let fromStringToDeparmentID (departmentIdValue: string) : DepartmentID =
        match makeDepartmentId departmentIdValue with
            | Result.Ok departmentId -> departmentId
            | Result.Error errorMessage -> failwith errorMessage

    // "toDeparmentId" 
    let fromDepartmentId (deparmentIdValue: DepartmentID) : string = 
        match deparmentIdValue with
            | CheckDepartmentID departmentId -> departmentId
    
    // "convertToDeparmentID" is een functie die een waarde omzet naar het Type DepartmentId
    let convertToDeparmentID (deparmentIDValue: string) : DepartmentID =
        CheckDepartmentID deparmentIDValue