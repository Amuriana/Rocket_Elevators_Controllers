using System;
using System.Collections.Generic;
using System.Threading;

namespace Commercial
{
    //Commercial Algo C#
    //Currently 75% functional
    //issues Test case senario # 4(column A/0)sending unexpected elevator to execute request.


    // the controller takes the requests input by the user and dispatches them to the corresponding column 0,1,2,3/A,B,C,D
    public class CommercialController
    {

        //floor that sent the call
        public int floor_number;
        //the amount of elevators in the given column
        public int elevators_per_column;
        //the number of the column that contains the range of floors for the request
        public int column_number;
        //the direction the elevator is travling
        public string direction;
        //the power source for the controller
        public Battery battery;
        //the list of requests
        public List<int> first_list;


        public CommercialController(int floor_number, int column_number, int elevators_per_column, string direction)
        {
            this.floor_number = floor_number;
            this.column_number = column_number;
            this.elevators_per_column = elevators_per_column;
            this.direction = direction;
            this.battery = new Battery(this.column_number);


        }

        // Requests elevator for 
        public Elevator RequestElevator(int FloorNumber, int RequestedFloor)
        {
            Thread.Sleep(200);
            Console.WriteLine("Request elevator to floor : " + FloorNumber);
            Console.WriteLine("********************************");
            Thread.Sleep(200);
            Console.WriteLine("Call Button Light On");
            Console.WriteLine("********************************");
            var column = battery.Coresponding_column(FloorNumber);
            direction = "down";
            var elevator = column.Find_Requested_elevator(FloorNumber, direction);
            if (elevator.elevator_floor > FloorNumber)
            {
                elevator.Send_request(FloorNumber, column.column_number);
                elevator.Send_request(RequestedFloor, column.column_number);
            }

            else if (elevator.elevator_floor < FloorNumber)
            {
                elevator.Move_down(RequestedFloor, column.column_number);
                elevator.Send_request(FloorNumber, column.column_number);
                elevator.Send_request(RequestedFloor, column.column_number);
            }
            Console.WriteLine("Button Light Off");

            return elevator;
        }


        public Elevator AssignElevator(int RequestedFloor)
        {
            Thread.Sleep(200);
            Console.WriteLine("Requested floor : " + RequestedFloor);
            Thread.Sleep(200);
            Console.WriteLine("Call Button Light On");

            Column column = battery.Coresponding_column(RequestedFloor);
            direction = "up";
            var FloorNumber = 1;
            Elevator elevator = column.Find_Assign_elevator(RequestedFloor, FloorNumber, direction);

            elevator.Send_request(FloorNumber, column.column_number);

            elevator.Send_request(RequestedFloor, column.column_number);



            return elevator;
        }

    }

    // Elevator and door status
    public class Elevator
    {
        //the number of the elevator that will be sent
        public int elevator_id;
        //elevators position and actions(moving/stopped/idle)
        public string status;
        //the current floor the elevator is on
        public int elevator_floor;
        //the direction the elevator is travling
        public string elevator_direction;
        //elevators door sensor to detect door obstructions T=clear F=blocked
        public bool Sensor;
        //displays the number of the current floor the elevator is on
        public int FloorDisplay;
        //the list of floors in the elevators queue
        public List<int> floor_list;

        public Elevator(int elevator_id, string status, int elevator_floor, string elevator_direction)
        {
            this.elevator_id = elevator_id;
            this.status = status;
            this.elevator_floor = elevator_floor;
            this.elevator_direction = elevator_direction;
            this.FloorDisplay = elevator_floor;
            this.Sensor = true;
            this.floor_list = new List<int>();
        }

        // here the requests are sorted by,descending and ascending order. 
        public void Send_request(int RequestedFloor, char column_number)
        {

            floor_list.Add(RequestedFloor);
            // if the requested floor is greater than the elevtor, put in "up" requests
            if (RequestedFloor > elevator_floor)
            {
                floor_list.Sort((a, b) => a.CompareTo(b));
            }
            // if the requested floor is less than the elevtor, put in "down" requests    
            else if (RequestedFloor < elevator_floor)
            {
                //sorted list determins the path the elevator is to take to complete it's requests        
                floor_list.Sort((a, b) => -1 * a.CompareTo(b));

            }
            //then send it out to execute the requests  

            Operate_elevator(RequestedFloor, column_number);
        }

        // moves the Elevator executing the tasks in it's list, and removes them once the request is fufiled
        public void Operate_elevator(int RequestedFloor, char column_number)
        {
            if (RequestedFloor == elevator_floor)
            {
                Open_door();
                this.status = "moving";

                this.floor_list.Remove(0);
            }
            else if (RequestedFloor < this.elevator_floor)
            {
                status = "moving";
                Console.WriteLine("Button Light Off");
                Console.WriteLine("********************************");
                Console.WriteLine("Column : " + column_number + " Elevator : " + this.elevator_id + " " + status);
                Console.WriteLine("********************************");
                this.elevator_direction = "down";
                Move_down(RequestedFloor, column_number);
                this.status = "stopped";
                Console.WriteLine("Column : " + column_number + " Elevator : " + this.elevator_id + " " + status);

                this.Open_door();
                this.floor_list.Remove(0);
            }
            else if (RequestedFloor > this.elevator_floor)
            {
                Thread.Sleep(1000);
                this.status = "moving";
                Console.WriteLine("Button Light Off");
                Console.WriteLine("********************************");
                Console.WriteLine("Column : " + column_number + " Elevator : " + this.elevator_id + " " + status);
                Console.WriteLine("********************************");
                this.elevator_direction = "up";
                this.Move_up(RequestedFloor, column_number);
                this.status = "stopped";
                Console.WriteLine("********************************");
                Console.WriteLine("Column : " + column_number + " Elevator : " + this.elevator_id + " " + status);


                this.Open_door();

                this.floor_list.Remove(0);
            }

        }
        // Open,Close Door upon reaching the called floor, and Door Sensor detects if the door is obstructed
        public void Open_door()
        {
            Thread.Sleep(3000);

            Console.WriteLine("********************************");
            Console.WriteLine("Door is Opening");

            Thread.Sleep(1000);
            Console.WriteLine("Door is Open");
            Thread.Sleep(3000);

            this.Close_door();
        }
        public void Close_door()
        {
            if (Sensor == true)
            {
                Console.WriteLine("Door is Closing");
                Thread.Sleep(3000);
                Console.WriteLine("Door is Closed");
                Thread.Sleep(3000);


                Console.WriteLine("********************************");
            }
            else if (Sensor == false)
            {
                Open_door();
            }
        }

        // Moves the elevator "up" 
        public void Move_up(int RequestedFloor, char column_number)
        {
            Console.WriteLine("Column : " + column_number + " Elevator : #" + elevator_id + "  Current Floor : " + this.elevator_floor);
            Thread.Sleep(1000);
            Console.WriteLine("********************************");
            while (this.elevator_floor != RequestedFloor)
            {
                this.elevator_floor += 1;
                Console.WriteLine("Column : " + column_number + " Elevator : #" + elevator_id + "  Floor : " + this.elevator_floor);

                Thread.Sleep(1000);
            }
        }
        //Moves elevator "down" 
        public void Move_down(int RequestedFloor, char column_number)
        {
            Console.WriteLine("Column : " + column_number + " Elevator : #" + elevator_id + "  Current Floor : " + this.elevator_floor);
            Thread.Sleep(1000);
            Console.WriteLine("********************************");
            while (this.elevator_floor != RequestedFloor)
            {
                this.elevator_floor -= 1;
                Console.WriteLine("Column : " + column_number + " Elevator : #" + elevator_id + "  Floor : " + this.elevator_floor);

                Thread.Sleep(1000);
            }
            Console.WriteLine("********************************");

        }

    }

    // here is where the column redirects task to the selected elevator list

    public class Column
    {
        public char column_number;
        public int floor_number;
        public int elevators_per_column;
        public List<Elevator> elevator_list;
        public List<int> call_button_list;


        public Column(char column_number, int floor_number, int elevators_per_column)
        {
            this.column_number = column_number;
            this.floor_number = floor_number;
            this.elevators_per_column = elevators_per_column;
            elevator_list = new List<Elevator>();
            call_button_list = new List<int>();
            for (int i = 0; i < this.elevators_per_column; i++)
            {
                Elevator elevator = new Elevator(i, "idle", 1, "up");
                elevator_list.Add(elevator);
            }
        }

        //Assigns the best elevator by claculating the shortest possible path, using direction, requested floor and elevator status(being idle/least busy or already on route to the floor)

        public Elevator Find_Assign_elevator(int RequestedFloor, int FloorNumber, string direction)
        {

            foreach (var elevator in elevator_list)
                if (elevator.status == "idle")
                {

                    return elevator;
                }

            foreach (var elevator in elevator_list)
                if (elevator.status == "moving")
                {

                    return elevator;
                }

            var bestElevator = 0;
            var shortest_distance = 1000;
            for (var i = 0; i < this.elevator_list.Count; i++)
            {
                var ref_distance = Math.Abs(elevator_list[i].elevator_floor - elevator_list[i].floor_list[0]) + Math.Abs(elevator_list[i].floor_list[0] - 1);
                if (shortest_distance >= ref_distance)
                {
                    shortest_distance = ref_distance;
                    bestElevator = i;
                }


            }
            return elevator_list[bestElevator];
        }



        // selects an elevator depending the shortest possible path for the request using the RequestedFloor, direction

        public Elevator Find_Requested_elevator(int RequestedFloor, string direction)
        {
            var shortest_distance = 999;
            var bestElevator = 0;

            for (var i = 0; i < this.elevator_list.Count; i++)
            {
                var ref_distance = elevator_list[i].elevator_floor - RequestedFloor;

                if (ref_distance > 0 && ref_distance < shortest_distance)
                {
                    shortest_distance = ref_distance;
                    bestElevator = i;
                }
            }
            return elevator_list[bestElevator];
        }

    }
    //The battery powers the CommercialController allowing it to populate the columns list, reciving and dispatching requests made by the user which it then sends to the elevators
    public class Battery
    {
        public string battery_status;
        public int column_number;
        public List<Column> column_list;


        public Battery(int column_number)
        {
            this.column_number = column_number;
            this.battery_status = "on";
            column_list = new List<Column>();



            char cols = 'A';
            for (int i = 0; i < this.column_number; i++, cols++)
            {

                Column column = new Column(cols, 60, 5);

                column.column_number = cols;
                column_list.Add(column);

            }
        }






        // The RequestedFloor is sent to the Cloumn that services that floor;
        //A/0(F-5 to F0 +RC) B/1(F2-F20+RC) C/2(F21-40F+RC) D/4(F41-F60+RC)
        public Column Coresponding_column(int RequestedFloor)
        {
            Column core_column = null;
            foreach (Column column in column_list)
            {
                //sent to list 0 for floors -5 to 0
                if (RequestedFloor > -5 && RequestedFloor <= 0 || RequestedFloor == 1)
                {
                    core_column = column_list[0];
                }
                //sent to list 1 for floors 2 to 20
                else if (RequestedFloor > 2 && RequestedFloor <= 20 || RequestedFloor == 1)
                {

                    core_column = column_list[1];


                }
                //sent to list 2 for floors 21 to 40
                else if (RequestedFloor > 21 && RequestedFloor <= 40 || RequestedFloor == 1)
                {
                    core_column = column_list[2];


                }
                //sent to list 3 for floors 41 to 60
                else if (RequestedFloor > 41 && RequestedFloor <= 60 || RequestedFloor == 1)
                {
                    core_column = column_list[3];


                }

            }
            //then it returns which cloumn list the request was sent
            return core_column;
        }
    }
    // here an alarm will sound if the elevator is to heavy(this achived by inputing max_load into the testing parameters below but has been exclued in the test cases, because the sound is anoying)
    public class LoadLimit
    {
        public int max_load;

        public int operational_load;

        public bool lb_sensor;

        public LoadLimit()
        {
            if (lb_sensor == true)
            {
                Console.WriteLine("elevator moving");
            }
            else if (lb_sensor == false)
            {
                Console.WriteLine("elevator full");
                Console.Beep();
            }

        }

        public class Commercial
        {
            public static void Main(string[] args)

            // populates the controller with (#of floors,#of columns,#of elevators per column )
            {
                CommercialController controller = new CommercialController(60, 4, 5, "down");


                //----test cases. Modern Approach. un-comment the senario you'd like to test, alternativley you can input other values by altering these; 
                //.elevator_floor = (the floor# the elevator is on)
                //.elevator_direction = (input "up or down")
                //.status = ("input:moving,stopped,idle")
                //.floor_list.Add(the floor # you want to go to)

                //AssignElevator(the requested floor);
                //RequestElevator(the floor where the request was made, the floor you want to go to);


                //-----------------------Case4-------COLUMN A/0------------------------------------------//  
                //column 0 (or Column A) serving the basements floors (0 to -5), with elevator 0 (idle at -3), elevator1 (idle at 1st floor), elevator2 at (-2 and going to -4), elevator3 at( -4 and going to 1st floor), and elevator4 at (0 going to -4), 
                //someone is at (-2 and requests the 1st) floor. Elevator 3 is expected to be sent.

                //controller.battery.column_list[0].elevator_list[0].elevator_floor = -3;



                //controller.battery.column_list[0].elevator_list[1].elevator_floor = 1;



                //controller.battery.column_list[0].elevator_list[2].elevator_floor = -2;
                // controller.battery.column_list[0].elevator_list[2].elevator_direction = "down";
                //controller.battery.column_list[0].elevator_list[2].status = "moving";
                // controller.battery.column_list[0].elevator_list[2].floor_list.Add(-4);


                // controller.battery.column_list[0].elevator_list[3].elevator_floor = -5;
                //controller.battery.column_list[0].elevator_list[3].elevator_direction = "up";
                // controller.battery.column_list[0].elevator_list[3].status = "moving";
                // controller.battery.column_list[0].elevator_list[3].floor_list.Add(1);


                // controller.battery.column_list[0].elevator_list[4].elevator_floor = 0;
                // controller.battery.column_list[0].elevator_list[4].elevator_direction = "down";
                // controller.battery.column_list[0].elevator_list[4].status = "moving";
                // controller.battery.column_list[0].elevator_list[4].floor_list.Add(-5);

                // controller.AssignElevator(1);
                //  Elevator elevator = controller.RequestElevator(-2, 1);

                //----------------------------------/Case#1 ColumnB/1-----------------------------------------------//           
                //

                // controller.battery.column_list[1].elevator_list[0].elevator_floor = 20;
                //  controller.battery.column_list[1].elevator_list[0].elevator_direction = "down";
                //controller.battery.column_list[1].elevator_list[0].status = "moving";
                // controller.battery.column_list[1].elevator_list[0].floor_list.Add(5);


                // controller.battery.column_list[1].elevator_list[1].elevator_floor = 3;
                // controller.battery.column_list[1].elevator_list[1].elevator_direction = "up";
                //  controller.battery.column_list[1].elevator_list[1].status = "moving";
                //  controller.battery.column_list[1].elevator_list[1].floor_list.Add(5);


                // controller.battery.column_list[1].elevator_list[2].elevator_floor = 13;
                //  controller.battery.column_list[1].elevator_list[2].elevator_direction = "down";
                //  controller.battery.column_list[1].elevator_list[2].status = "moving";
                //  controller.battery.column_list[1].elevator_list[2].floor_list.Add(1);

                //  controller.battery.column_list[1].elevator_list[3].elevator_floor = 15;
                //  controller.battery.column_list[1].elevator_list[3].elevator_direction = "down";
                //  controller.battery.column_list[1].elevator_list[3].status = "moving";
                //  controller.battery.column_list[1].elevator_list[3].floor_list.Add(2);


                // controller.battery.column_list[1].elevator_list[4].elevator_floor = 6;
                // controller.battery.column_list[1].elevator_list[4].elevator_direction = "down";
                // controller.battery.column_list[1].elevator_list[4].status = "moving";
                // controller.battery.column_list[1].elevator_list[4].floor_list.Add(1);




                // controller.AssignElevator(20);
                //   Elevator elevator = controller.RequestElevator(1, 20);

                //------------------------------------/Case#2-ColumnC/2-----------------------------------------/  

                //controller.battery.column_list[2].elevator_list[0].elevator_floor = 21;
                //controller.battery.column_list[2].elevator_list[0].elevator_direction = "down";
                //controller.battery.column_list[2].elevator_list[0].status = "moving";
                //controller.battery.column_list[2].elevator_list[0].floor_list.Add(1);


                //controller.battery.column_list[2].elevator_list[1].elevator_floor = 23;
                // controller.battery.column_list[2].elevator_list[1].elevator_direction = "up";
                //controller.battery.column_list[2].elevator_list[1].status = "moving";
                // controller.battery.column_list[2].elevator_list[1].floor_list.Add(28);


                // controller.battery.column_list[2].elevator_list[2].elevator_floor = 33;
                // controller.battery.column_list[2].elevator_list[2].elevator_direction = "down";
                // controller.battery.column_list[2].elevator_list[2].status = "moving";
                //controller.battery.column_list[2].elevator_list[2].floor_list.Add(1);


                //controller.battery.column_list[2].elevator_list[3].elevator_floor = 40;
                // controller.battery.column_list[2].elevator_list[3].elevator_direction = "down";
                // controller.battery.column_list[2].elevator_list[3].status = "moving";
                // controller.battery.column_list[2].elevator_list[3].floor_list.Add(24);


                // controller.battery.column_list[2].elevator_list[4].elevator_floor = 39;
                // controller.battery.column_list[2].elevator_list[4].elevator_direction = "down";
                // controller.battery.column_list[2].elevator_list[4].status = "moving";
                //controller.battery.column_list[2].elevator_list[4].floor_list.Add(1);


                // controller.AssignElevator(36);
                // Elevator elevator = controller.RequestElevator(1, 36);


                //------------------------------////Case#3 ColumnD/3///------------------------------------//


                controller.battery.column_list[3].elevator_list[0].elevator_floor = 58;
                controller.battery.column_list[3].elevator_list[0].elevator_direction = "down";
                controller.battery.column_list[3].elevator_list[0].status = "moving";
                controller.battery.column_list[3].elevator_list[0].floor_list.Add(1);


                controller.battery.column_list[3].elevator_list[1].elevator_floor = 50;
                controller.battery.column_list[3].elevator_list[1].elevator_direction = "up";
                controller.battery.column_list[3].elevator_list[1].status = "moving";
                controller.battery.column_list[3].elevator_list[1].floor_list.Add(60);


                controller.battery.column_list[3].elevator_list[2].elevator_floor = 46;
                controller.battery.column_list[3].elevator_list[2].elevator_direction = "up";
                controller.battery.column_list[3].elevator_list[2].status = "moving";
                controller.battery.column_list[3].elevator_list[2].floor_list.Add(58);


                controller.battery.column_list[3].elevator_list[3].elevator_floor = 54;
                controller.battery.column_list[3].elevator_list[3].elevator_direction = "down";
                controller.battery.column_list[3].elevator_list[3].status = "moving";
                controller.battery.column_list[3].elevator_list[3].floor_list.Add(1);


                controller.battery.column_list[3].elevator_list[4].elevator_floor = 60;
                controller.battery.column_list[3].elevator_list[4].elevator_direction = "down";
                controller.battery.column_list[3].elevator_list[4].status = "moving";
                controller.battery.column_list[3].elevator_list[4].floor_list.Add(1);

                controller.AssignElevator(1);
                Elevator elevator = controller.RequestElevator(54, 1);

            }



        }

    }

}