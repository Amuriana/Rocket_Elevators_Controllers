import time


def batteryStatus():
    return True


if batteryStatus():
    print(" Batttery Activated")
else:
    print("Back up power activated")


class ElevatorController:
    def __init__(self, number_of_floor, number_of_elevator):
        self.number_of_floor = number_of_floor
        self.number_of_elevator = number_of_elevator
        self.column = Column(number_of_floor, number_of_elevator)

        print("Controller activated")

    # ---------------- This Function Sends a Request From Floor Number and Direction Called  ----------------------
    def RequestElevator(self, FloorNumber, Direction):
        time.sleep(1)
        print("***************************************************")
        print("Call made from floor # : ", FloorNumber)
        time.sleep(1)
        print("***************************************************")
        print("Call Button Light On")
        time.sleep(1)
        elevator = self.find_best_elevator(FloorNumber, Direction)
        elevator.send_request(FloorNumber)
        return elevator

    # --------------------------Sends the Request for Floor Buttons Inside the Elevator -----------------------------------

    def RequestFloor(self, elevator, RequestedFloor):
        time.sleep(1)
        print("***************************************************")
        print("Requested floor : ", RequestedFloor)
        time.sleep(1)
        print("***************************************************")
        print("Request Button Light On")
        time.sleep(1)
        elevator.send_request(RequestedFloor)

    # --------------------This Function Dispatches the Best Elevator for The Request----------------------------------

    def find_best_elevator(self, FloorNumber, Direction):
        bestElevator = None
        shortest_distance = 1000
        for elevator in (self.column.elevator_list):
            if (FloorNumber == elevator.elevator_floor and (
                    elevator.status == "stopped" or elevator.status == "idle" or elevator.status == "moving")):
                return elevator
            else:
                ref_distance = abs(FloorNumber - elevator.elevator_floor)
                if shortest_distance > ref_distance:
                    shortest_distance = ref_distance
                    bestElevator = elevator

                elif elevator.Direction == Direction:
                    bestElevator = elevator

        return bestElevator


class Elevator:
    def __init__(self, elevator_number, status, elevator_floor, elevator_direction):
        self.elevator_number = elevator_number
        self.status = status
        self.elevator_floor = elevator_floor
        self.elevator_direction = elevator_direction
        self.floor_list = []

    #  ------------------------------------  Counting Requests --------------------------------

    def send_request(self, RequestedFloor):
        self.floor_list.append(RequestedFloor)
        self.compute_list()
        self.operate_elevator(RequestedFloor)

    # ---------------------- Sorting Out the Requests List By Asending and Desending Order----------------------------------

    def compute_list(self):
        if self.elevator_direction == "up":
            self.floor_list.sort()
        elif self.elevator_direction == "down":
            self.floor_list.sort()
            self.floor_list.reverse()
        return self.floor_list

    # ---------------------------------  Elevator Actions   --------------------------------

    def operate_elevator(self, RequestedFloor):
        while (len(self.floor_list) > 0):
            if ((RequestedFloor == self.elevator_floor)):
                self.Open_door()
                self.status = "moving"

                self.floor_list.pop()
            elif (RequestedFloor < self.elevator_floor):

                self.status = "moving"
                print("***************************************************")
                print("Elevator", self.elevator_number, self.status)
                print("***************************************************")
                self.Direction = "down"
                self.Move_down(RequestedFloor)
                self.status = "stopped"
                print("***************************************************")
                print("Elevator", self.elevator_number, self.status)
                print("***************************************************")
                self.Open_door()
                self.floor_list.pop()
            elif (RequestedFloor > self.elevator_floor):

                time.sleep(1)
                self.status = "moving"
                print("***************************************************")
                print("Elevator", self.elevator_number, self.status)
                print("***************************************************")
                self.Direction = "up"
                self.Move_up(RequestedFloor)
                self.status = "stopped"
                print("***************************************************")
                print("Elevator", self.elevator_number, self.status)
                print("***************************************************")

                self.Open_door()

                self.floor_list.pop()

        if self.floor_list == 0:
            self.status = "idle"

    # ---------------------------------Door  Ctrl----------------------------------------

    def Open_door(self):
        time.sleep(3)
        print("Open Door")
        print("***************************************************")
        print("Button Light Off")
        time.sleep(1)
        print("***************************************************")
        time.sleep(1)
        self.Close_door()

    def openButtonPressed(self):
        if self.status != "In-Service":
            self.openDoors()

    def Close_door(self):
        print("Close Door")
        time.sleep(3)

    def openButtonPressed(self):
        if self.status != "In-Service":
            self.openDoors()

    # -------------------------Elevator Actions---------------------------------------------------------------------

    def Move_up(self, RequestedFloor):
        print("Floor : ", self.elevator_floor)
        time.sleep(1)
        while (self.elevator_floor != RequestedFloor):
            self.elevator_floor += 1
            print("Floor : ", self.elevator_floor)
            time.sleep(1)

    def Move_down(self, RequestedFloor):
        print("Floor : ", self.elevator_floor)
        time.sleep(1)
        while (self.elevator_floor != RequestedFloor):
            self.elevator_floor -= 1
            print("Floor : ", self.elevator_floor)
            time.sleep(1)


class Call_button:
    def __init__(self, FloorNumber, Direction):
        self.FloorNumber = FloorNumber
        self.Direction = Direction
        self.light = False


# ------------------Elevator Cage Floor Buttons & Misc-----------------------------------------

class Floor_button:
    def __init__(self, RequestedFloor):
        self.RequestedFloor = RequestedFloor


# ------------------------------------------------------------

class Column:
    def __init__(self, number_of_floor, number_of_elevator):
        self.number_of_floor = number_of_floor
        self.number_of_elevator = number_of_elevator
        self.elevator_list = []
        for i in range(number_of_elevator):
            elevator = Elevator(i, "idle", 1, "up")
            self.elevator_list.append(elevator)


# -------------------------------------Test Cases------------------------------------------------

# A request is made from f1 to f6, elevator0 is idle at f10 and elevator 1 is idle at f3( elevator1 will take the request )

# 2min later a request is made from F3 to F5, elevator0 is idle at F10 and elevator1 is idle at F6 (elevator1 will take the request)

# 5min later a request is made from F9 to F2 and elevator0 F10 and Elevator1 is idle on F5 (elevator0 will take the request)

#controller = ElevatorController(10, 2)
#controller.column.elevator_list[0].elevator_floor = 10
#controller.column.elevator_list[0].status = "moving"
#controller.column.elevator_list[0].elevator_direction = "down"
#controller.column.elevator_list[1].elevator_floor = 3
#controller.column.elevator_list[1].status = "moving"
#controller.column.elevator_list[1].elevator_direction = "down"

#elevator = controller.RequestElevator(1, "up")
#controller.RequestFloor(elevator, 6)
#elevator = controller.RequestElevator(3, "up")
#controller.RequestFloor(elevator, 5)
#elevator = controller.RequestElevator(9, "down")
#controller.RequestFloor(elevator, 2)
