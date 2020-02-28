package main

import (
	"fmt"
) 

//Commercial Algo Go, currently incomplete.



// Battery class/struct and variable placeholder for SystemOnline Function
type Battery struct {
	number_of_columns int
	column_list []Column
}

// SystemOnline is the function that powers the controller 
//and allows the cloumn lists to be populated with requests from users in the building

func SystemOnline(number_of_columns int) *Battery {
	battery := new(Battery)
	battery.number_of_columns = 4
	for index := 0; index < battery.number_of_columns; index++ {
		column := CloumnsABCD(index)
		battery.column_list = append(battery.column_list, *column)
	}
	return battery
}

// CommercialController class/struct and variable placeholder for CoreController function
type CommercialController struct {
	Power_Source int
	Power  []Battery
	number_of_columns int
	direction  string
}

func CoreController(Power_Source int) CommercialController {
	controller := new(CommercialController)
	controller.Power_Source = 1
	for index := 0; index < Power_Source; index++ {
		battery := SystemOnline(index)
		controller.Power = append(controller.Power, *battery)
	}
	return *controller
}


// Column class/struct and variable placeholder for ColumnsABCD function
type Column struct {
	column_id int
	elevators_per_column int
	elevator_list []Elevator
}

// CloumnsABCD contains a set of 5 elevators for each of the 4 columns

func CloumnsABCD(elevators_per_column int) *Column {
	column := new(Column)
	column.elevators_per_column = 5
	for index := 0; index < column.elevators_per_column; index++ {
		elevator := Cage()
		column.elevator_list = append(column.elevator_list, *elevator)
	}
	return column
}

 // The RequestedFloor is sent to the Cloumn that services that floor;
        //A/0(F-5 to F0 +RC) B/1(F2-F20+RC) C/2(F21-40F+RC) D/4(F41-F60+RC)
func (b *Battery) CorespondingColumn(RequestedFloor int) Column { 
	if RequestedFloor > -5 && RequestedFloor <= 0 {
		return b.column_list[0]
	} else if RequestedFloor > 2 && RequestedFloor <= 20 {
		return b.column_list[1]
	} else if RequestedFloor > 21 && RequestedFloor <= 40 {
		return b.column_list[2]
	} else if RequestedFloor > 41 && RequestedFloor <= 60 {
		return b.column_list[3]
	}
	return b.column_list[3]
}


type Elevator struct {
	elevator_id        int
	elevator_position  int
	Floor_list        []int
	elevator_status    string
	elevator_direction string
	door_sensor        bool
	column             Column
}



func Cage() *Elevator {
	elevator := new(Elevator)
	elevator.elevator_position = 1
	elevator.Floor_list = []int{}
	elevator.elevator_status = "idle"
	elevator.elevator_direction = "up"
	elevator.door_sensor = true
	return elevator
}


//  Door CTRL functions
func (cage *Elevator) OpenDoor() {
	fmt.Println("*****************************")
	fmt.Println("Opening Doors")
	fmt.Println("Doors are Open")
	fmt.Println("Button Light Off")
	cage.CloseDoor()
}
func (cage *Elevator) CloseDoor() {
	if cage.door_sensor == true {
		fmt.Println("Closing Doors")
		fmt.Println("Doors are Closed")
		fmt.Println("*****************************")
	} else if cage.door_sensor {
		cage.OpenDoor()
		fmt.Println("Doors obstructed, clear doorway")
	}
}



func main() {
	fmt.Println("Out of service, use stairs")
}
