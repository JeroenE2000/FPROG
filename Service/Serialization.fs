module Serialization

open Thoth.Json.Net
open Model.Employee
open Model.Hours
open Model.Department
open Common

let decodeDepartmentId : Decoder<DepartmentID> =
    Decode.string
    |> Decode.andThen (fun departmentId ->
        match CheckDepartmentIDModule.makeDepartmentId departmentId with
        | Result.Ok validDepartmentId -> Decode.succeed validDepartmentId
        | Result.Error errorMessage -> Decode.fail errorMessage
    )

let decodeDeparmentIdFromString (name: string) : DepartmentID =
    CheckDepartmentIDModule.convertToDeparmentID name 

let encodeDepartmentId : Encoder<DepartmentID> =
    fun departmentId ->
        Encode.string (match departmentId with CheckDepartmentID value -> value)

let encodeEmployees : Encoder<Employee> =
    fun employee ->
        Encode.object
            [ "name", Encode.string employee.Name
              "department_id", encodeDepartmentId employee.DepartmentId ]

let decodeHoursAmount: Decoder<HoursAmount> =
    Decode.int |> Decode.andThen (fun amount ->
        match HoursAmountModule.create amount with
        | Ok hoursAmount -> Decode.succeed hoursAmount
        | Error errorMessage -> Decode.fail errorMessage
    )
          
let decodeHours: Decoder<Hours> =
    Decode.object (fun get ->
        { Date = get.Required.Field "date" Decode.datetimeUtc
          Amount = get.Required.Field "amount" decodeHoursAmount 
          })

let decodeDeparmentName : Decoder<DepartmentName> =
    Decode.string
    |> Decode.andThen (fun departmentName ->
        match DepartmentNameModule.createName departmentName with
        | Ok validDepartmentName -> Decode.succeed validDepartmentName
        | Error errorMessage -> Decode.fail errorMessage
    )

let rec decodeDepartment : Decoder<Department> =
    Decode.object (fun get ->
        { Id = get.Required.Field "id" decodeDepartmentId
          Name = get.Required.Field "name" decodeDeparmentName
          Subdepartments = get.Optional.Field "subdepartments" (Decode.list decodeDepartment) |> Option.defaultValue []
         })



    



