
using System;
using System.Threading;
using RestSharp;

namespace Main
    {

    public class Program
    {

        // to start the simulation run in the terminal: dotnet watch run


        // url to fiware/orion broker
        public static string URL { get; set; }  = "http://host.docker.internal:1026/";

        // Id of the workingstation
        public static string ID { get; set; } = "urn:ngsiv2:I40Asset:Workstation00001";

        // order NR 
        public static string ON = "urn:ngsiv2:I40Asset:ON000001";

        // the time base for the cycleTime. To this base another time is added which is a random-Time generated in the getRandomTime function
        public static int CycleTimeBase { get; set; } = 2;

        // Plan parts nr 
        public static int PlanPart = 100;
        
        



        public static int prodParts = 0;
        public static int prodPartsIO = 0;

        static void Main(string[] args)
        {
            Simulate();            
        }

    public static void Simulate(){
        while(prodParts < PlanPart){
            int randOp1 = new Random().Next(7);
            int intOp1 = 1 + randOp1;
            int opTakeTime1 = (intOp1 / 3) * 1000;
            int opPutTime1 = intOp1 - opTakeTime1;
            SetDrawer1StatusToExecute();
            Thread.Sleep(opTakeTime1);
            var drawer1CycleTime = getRandomTime(CycleTimeBase) * 1000;
            if(prodParts != 0){
                SetDrawer2StatusToIdle();
            }
            Thread.Sleep(drawer1CycleTime - opTakeTime1);
            SetDrawer1StatusToComplete(drawer1CycleTime);
            SetDrawer2StatusToExecute();
            Thread.Sleep(opTakeTime1);
            SetDrawer1StatusToIdle();
            var drawer2CycleTime = getRandomTime(CycleTimeBase) * 1000;
            Thread.Sleep(drawer2CycleTime - opTakeTime1);
            SetDrawer2StatusToComplete(drawer2CycleTime);
            prodParts = prodParts +1;
            var date = DateTime.Now;

            UpdateOrder(date);
            // DateTime date = DateTime.Now;
            // Console.WriteLine(date);
        
        }
        Thread.Sleep(1000);
        SetDrawer2StatusToIdle();
        


    }

    public static int getRandomTime(int baseTime){
        int randTime = new Random().Next(5);
        return baseTime+ randTime;
    }


        public static void SetDrawer1StatusToExecute(){
                var client = new RestClient(URL);
            // Create the request to fiware/orion (Updating the workingstation); drawer1Status is set to execute and the robotRunning to true
                var request = new RestRequest("v2/entities/" + ID + "/attrs", Method.Patch);
                request.AddHeader("fiware-service", "robot_info");
                request.AddHeader("fiware-servicepath", "/demo");
                request.AddHeader("Content-Type", "application/json");

                var body = @"" + "\n" +      @"  {  ""robotRunning"": {""value"": true,""type"": ""Boolean""}," + "\n" + @"  ""drawer1Status"": {""value"": ""Execute"", ""type"": ""String""  }  }";

                string body0 = Convert.ToString(body);
                request.AddJsonBody(body0);
                var obj = client.Execute(request);
        }

        public static void SetDrawer1StatusToComplete(int cycleTime){
                var client1 = new RestClient(URL);
                var request1 = new RestRequest("v2/entities/"+ ID +"/attrs", Method.Patch);
                request1.AddHeader("fiware-service", "robot_info");
                request1.AddHeader("fiware-servicepath", "/demo");
                request1.AddHeader("Content-Type", "application/json");
                cycleTime = cycleTime/ 1000;
                var body1 = @"" + "\n" + @"  {  ""robotRunning"": {""value"": false,""type"": ""Boolean""}," + "\n" + @"  ""drawer1Status"": {""value"": ""Complete"", ""type"": ""String""  }, "+ "\n" + @"  ""currCycleTime"": {""value"": "+ cycleTime + @", ""type"": ""Integer""  }  }";
                string body01 = Convert.ToString(body1);
                request1.AddJsonBody(body01);
                client1.Execute(request1);
        }

        public static void SetDrawer1StatusToIdle(){
                    var client2 = new RestClient(URL);
                    var request2 = new RestRequest("v2/entities/"+ID+"/attrs", Method.Patch);
                    request2.AddHeader("fiware-service", "robot_info");
                    request2.AddHeader("fiware-servicepath", "/demo");
                    request2.AddHeader("Content-Type", "application/json");
                    var body2 = @"" + "\n" + @"  {   ""drawer1Status"": {""value"": ""Idle"", ""type"": ""String""  }  }";
                    string body02 = Convert.ToString(body2);
                    request2.AddJsonBody(body02);
                    client2.Execute(request2);
        }

        public static void SetDrawer2StatusToExecute(){
                                var client = new RestClient(URL);
                    var request = new RestRequest("v2/entities/"+ ID +"/attrs", Method.Patch);
                    request.AddHeader("fiware-service", "robot_info");
                    request.AddHeader("fiware-servicepath", "/demo");
                    request.AddHeader("Content-Type", "application/json");


                    var body = @"" + "\n" + @"  {  ""robotRunning"": {""value"": true,""type"": ""Boolean""}," + "\n" + @"  ""drawer2Status"": {""value"": ""Execute"", ""type"": ""String""  }  }";


                    string body0 = Convert.ToString(body);

                    request.AddJsonBody(body0);

                    client.Execute(request);
        }

        public static void SetDrawer2StatusToComplete(int cycleTime){
            var client1 = new RestClient(URL);
            var request1 = new RestRequest("v2/entities/"+ ID +"/attrs", Method.Patch);
            request1.AddHeader("fiware-service", "robot_info");
            request1.AddHeader("fiware-servicepath", "/demo");
            request1.AddHeader("Content-Type", "application/json");
            cycleTime = cycleTime/ 1000;
            var body1 = @"" + "\n" + @"  {  ""robotRunning"": {""value"": false,""type"": ""Boolean""}," + "\n" + @"  ""drawer2Status"": {""value"": ""Complete"", ""type"": ""String""  }, "+ "\n" + @"  ""currCycleTime"": {""value"": "+ cycleTime + @", ""type"": ""Integer""  }  }";
            string body01 = Convert.ToString(body1);
            request1.AddJsonBody(body01);
            client1.Execute(request1);

        }

        public static void SetDrawer2StatusToIdle(){
                    var client2 = new RestClient(URL);
                    var request2 = new RestRequest("v2/entities/"+ID+"/attrs", Method.Patch);
                    request2.AddHeader("fiware-service", "robot_info");
                    request2.AddHeader("fiware-servicepath", "/demo");
                    request2.AddHeader("Content-Type", "application/json");
                    var body2 = @"" + "\n" + @"  {   ""drawer2Status"": {""value"": ""Idle"", ""type"": ""String""  }  }";
                    string body02 = Convert.ToString(body2);
                    request2.AddJsonBody(body02);
                    client2.Execute(request2);
        }

        public static void UpdateOrder(DateTime time){

                    // Updating the order (parts + partsIO)
                    var client4 = new RestClient(URL);
                    var request4 = new RestRequest("v2/entities/" + ON +"/attrs", Method.Patch);
                    request4.AddHeader("fiware-service", "robot_info");
                    request4.AddHeader("fiware-servicepath", "/demo");
                    request4.AddHeader("Content-Type", "application/json");

                    

                    if (prodParts % 25 > 0)
                    {
                        // 2023-02-04 15:55:56
                       prodPartsIO = prodPartsIO + 1;
                       var body4 = @"  {     ""prodParts"": {      ""value"": " + prodParts.ToString()+ @",      ""type"": ""Integer""    },    ""prodPartsIO"": {      ""value"":  " + prodPartsIO + @",      ""type"": ""Integer""    },    ""orderStatus"": {    ""value"": ""Execute"",      ""type"": ""String""    },    ""finishedTime"": {    ""value"": """+time.ToString("yyyy-MM-dd HH:mm:ss")+@"""  ,      ""type"": ""Datetime""    }      }";
                       string body04 = Convert.ToString(body4);
                       Console.WriteLine(body04);
                       request4.AddJsonBody(body04);
                        var obj = client4.Execute(request4);
                        Console.WriteLine("Updating order:" + prodParts);
                        Console.WriteLine(obj.ErrorException);
                     }

                    else
                    {

                     var body4 = @"  {     ""prodParts"": {      ""value"": " + prodParts.ToString() + @",      ""type"": ""Integer""    },    ""orderStatus"": {    ""value"": ""Execute"",      ""type"": ""String""    },    ""finishedTime"": {    ""value"": """+time.ToString("yyyy-MM-dd HH:mm:ss")+@"""  ,      ""type"": ""Datetime""    }       }";
                     string body04 = Convert.ToString(body4);
                     request4.AddJsonBody(body04);
                     var obj = client4.Execute(request4);
                     Console.WriteLine(body04);
                     Console.WriteLine("Updating order:" + prodParts);
                     Console.WriteLine(obj.StatusDescription);


                    }
                    // if all parts are produced set the enddate to the current date
                    if (prodParts == PlanPart )
                    {

                      string EndData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


                      // updating the order, setting the orderstatus to complete and the enddate
                      var client5 = new RestClient(URL);
                      var request5 = new RestRequest("v2/entities/" + ON +"/attrs", Method.Patch);
                      request5.AddHeader("fiware-service", "robot_info");
                      request5.AddHeader("fiware-servicepath", "/demo");
                      request5.AddHeader("Content-Type", "application/json");
                      var body5 = @"{    ""finishedTime"": {     ""value"": """ + EndData + @""",      ""type"": ""Datetime""    },    ""orderStatus"": {    ""value"": ""Complete"",      ""type"": ""String""    },    ""finishedTime"": {    ""value"": """+time.ToString("yyyy-MM-dd HH:mm:ss")+@""" ,      ""type"": ""Datetime""    }       }";
                      string body05 = Convert.ToString(body5);
                      request5.AddJsonBody(body05);
                      client5.Execute(request5);

                    }
        }
    }
}
        

