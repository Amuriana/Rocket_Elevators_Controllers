/*****************************************************************************************/
/*                          RESIDENTIAL ELEVATOR CTRL JS 
                                    INCOMPLETE                                      */
/*                                                                                       */
/*****************************************************************************************/

const battery = "on";

function Battery(){
 
};



function elevator_(number_of_floor, number_of_elevator) {
    var controller = new elevatorController(number_of_floor, number_of_elevator);
  
    return controller;
  }
  class Column {
    constructor(number_of_floor, number_of_elevator) {
      this.number_of_floor = number_of_floor;
      this.number_of_elevator = number_of_elevator;
      this.elevator_list = [];
      for (let number_of_elevator of number_of_elevator) {
        let elevator = new Elevator(i, "idle", 1, "up");
        this.elevator_list.push(elevator);
      }
    }
  }
  
  class Elevator {
    constructor(elevator_number, status, elevator_floor, elevator_direction) {
      this.elevator_number = elevator_number;
      this.status = status;
      this.elevator_floor = elevator_floor;
      this.elevator_direction = elevator_direction;
      this.floor_list = [];
    }
    //------------------------------  Counting Requests ----------------------------------
  
    send_request(RequestedFloor) {
      this.floor_list.push(RequestedFloor);
      this.compute_list();
      this.operate_elevator(RequestedFloor)
    }
    //---------------- Sorting Out the Requests List By Asending and Desending Order------------------------------------------
  
    compute_list() {
      if (this.elevator_direction == "up") {
        this.floor_list.sort();
      } else if (this.elevator_direction = "down") {
        this.floor_list.sort();
        this.floor_list.reverse();
      }
      return this.floor_list;
    }
    //---------------------------------  Elevator Actions   -------------------------------------------------------------------------------------------------------
  
    operate_elevator(RequestedFloor) {
      while (this.floor_list > 0) {
        // READ nextfloor FROM floor_list COMMENT???
        if (RequestedFloor === this.elevator_floor) {
          this.Open_door();
          this.status = "moving";
  
          this.floor_list.shift();
        } else if (RequestedFloor < this.elevator_floor) {
          this.status = "moving";
         
          console.log("Elevator" + this.elevator_number, this.status);
          
          this.Direction = "down";
          this.Move_down(RequestedFloor);
          this.status = "stopped";
          console.log("Elevator" + this.elevator_number, this.status);
          
          this.Open_door();
          this.floor_list.shift();
        } else if (RequestedFloor > this.elevator_floor) {
          sleep(1000);
          this.status = "moving";
          console.log("Elevator" + this.elevator_number, this.status);
          
          this.Direction = "up";
          this.Move_up(RequestedFloor);
          this.status = "stopped";
          console.log("Elevator" + this.elevator_number, this.status);
          
          this.Open_door();
  
          this.floor_list.shift();
        }
      }
      if (this.floor_list === 0) {
        this.status = "idle";
      }
    }
    Request_floor_button(RequestedFloor) {
      this.RequestedFloor = RequestedFloor;
      this.floor_light = floor_light;
    }
    Call_floor_button(FloorNumber, Direction) {
      this.FloorNumber = FloorNumber;
      this.Direction = Direction;
    }
    //-----------------------------------Door CTRL--------------------------------------------
  
    Open_door() {
      sleep(3000);
      console.log("Open Door");
      console.log("Button Light Off");
      sleep(3000);
      this.Close_door(); 
    }

    
    
    Close_door() {
      console.log("close door");
      sleep(3000);
    }
  
    // -------------------------Elevator -----------------------------------------------
  
    Move_up(RequestedFloor) {
      console.log("Floor : " + this.elevator_floor);
      sleep(1000);
      while (this.elevator_floor = RequestedFloor) {
        this.elevator_floor += 1;
        console.log("Floor : " + this.elevator_floor);
  
        sleep(1000);
      }
    }
  
    Move_down(RequestedFloor) {
      console.log("Floor : " + this.elevator_floor);
      sleep(1000);
      while (this.elevator_floor = RequestedFloor) {
        this.elevator_floor -= 1;
        console.log("Floor : " + this.elevator_floor);
  
        sleep(1000);
      }
    }
  }
  
  class elevatorController {
    constructor(number_of_floor, number_of_elevator) {
      this.number_of_floor = number_of_floor;
      this.number_of_elevator = number_of_elevator;
      this. column = new Column(number_of_floor, number_of_elevator);
      // console.log(this.column);
  
      console.log("Controller iniatiated");
    }
  
    RequestElevator(FloorNumber, Direction) {
      sleep(1000);

      console.log("Request elevator to floor : ", FloorNumber);
      sleep(1000);
      
      console.log("Call Button Light On");
      sleep(1000);
  
      let elevator = this.find_best_elevator(FloorNumber, Direction);
      elevator.send_request(FloorNumber);
      return elevator;
    }
    //-------------------------------------------------------------------
  
    RequestFloor(elevator, RequestedFloor) {
      sleep(1000);
      console.log("Requested floor : ", RequestedFloor);
      
      
      console.log("Request Button Light On");
      sleep(1000);
      elevator.send_request(RequestedFloor);
      // Elevator.operate_elevator(RequestedFloor);
    }
    //-----------This Function Dispatches the Best Elevator for The Request---------------------------------------
  
    find_best_elevator(FloorNumber, Direction) {
      console.log("find_best_elevator", FloorNumber, Direction);
  
      let bestElevator = null;
      let shortest_distance = 1000;
      for (let column.elevator_list of column.elevator_list) {
        let elevator = this.column.elevator_list[i];
  
        if (
          FloorNumber === elevator.elevator_floor &&
          (elevator.status === "stopped" ||
            elevator.status === "idle" ||
            elevator.status === "moving")
        ) {
          return elevator;
        } else {
          let ref_distance = Math.abs(FloorNumber - elevator.elevator_floor);
          if (shortest_distance > ref_distance) {
            shortest_distance = ref_distance;
            bestElevator = elevator;
  
            if (elevator.Direction === Direction) {
              bestElevator = elevator;
            }
          }
        }
      }
      return bestElevator;
    }
  
  
  //----------------------------------------------------------------------------------
  
  var d = new Date();{
    var hours = d.getHours();
    var mins = d.getMinutes();
    var secs = d.getSeconds();
    document.body.innerHTML =
    hours+":"+mins+":"+secs;
}
         setInterval(printTime, 1000);
      }
    }
  }
  
  //-------------------------------------Test Cases------------------------------------------------

// A request is made from f1 to f6, elevator0 is idle at f10 and elevator 1 is idle at f3( elevator1 will take the request )
  
// 2min later a request is made from F3 to F5, elevator0 is idle at F10 and elevator1 is idle at F6 (elevator1 will take the request)
  
  //5min later a request is made from F9 to F2 and elevator0 F10 and Elevator1 is idle on F5 (elevator0 will take the request)
  
  //controller = ElevatorController(10, 2)
  //controller.column.elevator_list[0].elevator_floor = 10
  //controller.column.elevator_list[0].status = "moving"
  //controller.column.elevator_list[0].elevator_direction = "down"
  //controller.column.elevator_list[1].elevator_floor = 3
  //controller.column.elevator_list[1].status = "moving"
  //controller.column.elevator_list[1].elevator_direction = "down"
  
//elevator = controller.RequestElevator(1, "up")
  //controller.RequestFloor(elevator, 6)
  //elevator = controller.RequestElevator(3, "up")
  //controller.RequestFloor(elevator, 5)
  //elevator = controller.RequestElevator(9, "down")
  //controller.RequestFloor(elevator, 2)






