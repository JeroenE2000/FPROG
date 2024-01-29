module Model.Department

open Common
type DepartmentName = private NameChecker of string

let (|NameChecker|) (NameChecker name) = name


//module voor het aanmaken van deparmentname 
[<RequireQualifiedAccess>]
module DepartmentNameModule = 
   let validateName (name: string) =
        let isValid = 
            name.Length > 0 && System.Char.IsUpper name.[0] &&
            name |> Seq.forall (fun c -> System.Char.IsLetter c || c = ' ')
        let hasConsecutiveSpaces =
            name |> Seq.windowed 2 |> Seq.exists (fun window -> window.[0] = ' ' && window.[1] = ' ')
        isValid && not hasConsecutiveSpaces

   let toString (name: DepartmentName) =
        match name with
        | NameChecker name -> name

   let fromString (name: string)  =
        if validateName name then
            NameChecker name
        else
            failwith "Ongeldige afdelingsnaam"
    
   let createName (name: string) = 
        if validateName name then
            NameChecker name |> Result.Ok
        else
            Result.Error "Ongeldige afdelingsnaam"


type Department = { Id: DepartmentID;
                    Name: DepartmentName;
                    Subdepartments: List<Department> }