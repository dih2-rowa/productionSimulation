
using System.Globalization;
using System.Net;
using System.Text;
using RestSharp;

namespace Main
    {

        // Robot
        // Changes --> Raw Part --> Execute
        //             Processed --> Complete
        //             Empty --> Idle
        // Order
        //             Finished --> Complete

    public class Program
    {


        // url to fiware/orion broker
        public static string URL { get; set; }  = "http://localhost:1026/";

        // Id of the workingstation
        public static string ID { get; set; } = "urn:ngsiv2:I40Asset:Workstation00001";

        // order NR 
        public static string ON = "";

        // Plan parts nr 
        public static int PlanPart = 1250;

        public static int prodParts = 50;
        public static int prodPartsIO = 0;

        static void Main(string[] args)
        {


            // Reading data: prod parts and plan parts 

            for (int i = 0; i <= PlanPart; i++)
            {
                Schublade1();
                Schublade2();
            }
            
        }

        public static void Schublade1()
        {
           

          
                Console.WriteLine(prodParts);
                int randCyc1 = new Random().Next(16);
                int CurCycTime1 = 69 + randCyc1;
                int randOp1 = new Random().Next(7);
                int intOp1 = 11 + randOp1;
                int opTakeTime1 = (intOp1 / 3);
                int opPutTime1 = intOp1 - opTakeTime1;

            Console.WriteLine(CurCycTime1);


            var client = new RestClient(URL);
            // Create the request to fiware/orion (Updating the workingstation); drawer1Status is set to execute and the robotRunning to true
                var request = new RestRequest("v2/entities/" + ID + "/attrs/", Method.Patch);
                request.AddHeader("fiware-service", "robot_info");
                request.AddHeader("fiware-servicepath", "/demo");
                request.AddHeader("Content-Type", "application/json");


                var body = @"" + "\n" +      @"  {  ""robotRunning"": {""value"": true,""type"": ""Boolean""}," + "\n" + @"  ""drawer1Status"": {""value"": ""Execute"", ""type"": ""String""  }  }";


                string body0 = Convert.ToString(body);

                request.AddJsonBody(body0);
                
                client.Execute(request);
                 

                // wait TZ
                Thread.Sleep(CurCycTime1 * 1000);


//Completed
// Create the request to fiware/orion (Updating the workingstation); drawer1Status is set to completed and the robotRunning to false
                var client1 = new RestClient(URL);
                var request1 = new RestRequest("v2/entities/"+ ID +"/attrs/", Method.Patch);
                request.AddHeader("fiware-service", "robot_info");
                request.AddHeader("fiware-servicepath", "/demo");
                request.AddHeader("Content-Type", "application/json");
                var body1 = @"" + "\n" + @"  {  ""robotRunning"": {""value"": false,""type"": ""Boolean""}," + "\n" + @"  ""drawer1Status"": {""value"": ""Complete"", ""type"": ""String""  }  }";
                string body01 = Convert.ToString(body1);
                request.AddJsonBody(body01);
                client.Execute(request);


                // Wait operator taking part
                Thread.Sleep(opTakeTime1 * 1000); 

// Create the request to fiware/orion (Updating the workingstation); drawer1Status is set to idle and the robotRunning value remains to false
                var client2 = new RestClient(URL);
                var request2 = new RestRequest("v2/entities/"+ ID +"/attrs/", Method.Patch);
                request.AddHeader("fiware-service", "robot_info");
                request.AddHeader("fiware-servicepath", "/demo");
                request.AddHeader("Content-Type", "application/json");
                var body2 = @"" + "\n" + @"  {   ""drawer1Status"": {""value"": ""Idle"", ""type"": ""String""  }  }";
                string body02 = Convert.ToString(body2);
                request.AddJsonBody(body02);
                client.Execute(request);
                

                // Wait operator putting part
                Thread.Sleep(opPutTime1 * 1000);

// Create the request to fiware/orion (Updating the workingstation); drawer1Status is set to execute
                var client3 = new RestClient(URL);
                var request3 = new RestRequest("v2/entities/" + ID +"/attrs/", Method.Patch);
                request.AddHeader("fiware-service", "robot_info");
                request.AddHeader("fiware-servicepath", "/demo");
                request.AddHeader("Content-Type", "application/json");
                var body3 = @"" + "\n" + @"  {   ""drawer1Status"": {""value"": ""Execute"", ""type"": ""String""  }  }";
                string body03 = Convert.ToString(body3);
                request.AddJsonBody(body03);
                client.Execute(request);
            
        }

        public static void Schublade2()
        {
                    int randCyc2 = new Random().Next(16);
                    int CurCycTime2 = 69 + randCyc2;
                    int randOp2 = new Random().Next(7);
                    int intOp2 = 11 + randOp2;
                    int opTakeTime2 = (intOp2 / 3);
                    int opPutTime2 = intOp2 - opTakeTime2;

                    Console.WriteLine(CurCycTime2);

                    // Creating the request for updating the workinstation (drawer2Status). robutRunning set to true and drawer2Status to execute
                    var client = new RestClient(URL);
                    var request = new RestRequest("v2/entities/"+ ID +"/attrs/", Method.Patch);
                    request.AddHeader("fiware-service", "robot_info");
                    request.AddHeader("fiware-servicepath", "/demo");
                    request.AddHeader("Content-Type", "application/json");


                    var body = @"" + "\n" + @"  {  ""robotRunning"": {""value"": true,""type"": ""Boolean""}," + "\n" + @"  ""drawer2Status"": {""value"": ""Execute"", ""type"": ""String""  }  }";


                    string body0 = Convert.ToString(body);

                    request.AddJsonBody(body0);

                    client.Execute(request);


                    // wait TZ
                    Thread.Sleep(CurCycTime2 * 1000);

            // Creating the request for updating the workinstation (drawer2Status). robutRunning set to false and drawer2Status to complete
            var client1 = new RestClient(URL);
            var request1 = new RestRequest("v2/entities/"+ ID +"/attrs/", Method.Patch);
            request.AddHeader("fiware-service", "robot_info");
            request.AddHeader("fiware-servicepath", "/demo");
            request.AddHeader("Content-Type", "application/json");
            var body1 = @"" + "\n" + @"  {  ""robotRunning"": {""value"": false,""type"": ""Boolean""}," + "\n" + @"  ""drawer2Status"": {""value"": ""Complete"", ""type"": ""String""  }  }";
            string body01 = Convert.ToString(body1);
            request.AddJsonBody(body01);
            client.Execute(request);


            // Wait operator taking part
            Thread.Sleep(opTakeTime2 * 1000);

                    // Creating the request for updating the workinstation (drawer2Status).  drawer2Status set to idle (robotRunning stays on false)
                    var client2 = new RestClient(URL);
                    var request2 = new RestRequest("v2/entities/"+ID+"/attrs/", Method.Patch);
                    request.AddHeader("fiware-service", "robot_info");
                    request.AddHeader("fiware-servicepath", "/demo");
                    request.AddHeader("Content-Type", "application/json");
                    var body2 = @"" + "\n" + @"  {   ""drawer2Status"": {""value"": ""Idle"", ""type"": ""String""  }  }";
                    string body02 = Convert.ToString(body2);
                    request.AddJsonBody(body02);
                    client.Execute(request);


                    // Wait operator putting part
                    Thread.Sleep(opPutTime2 * 1000);

                    prodParts = prodParts++;  

                    // set drawer status + CycleTime
                    var client3 = new RestClient(URL);
                    var request3 = new RestRequest("v2/entities/"+ID+"/attrs/", Method.Patch);
                    request.AddHeader("fiware-service", "robot_info");
                    request.AddHeader("fiware-servicepath", "/demo");
                    request.AddHeader("Content-Type", "application/json");
                    var body3 = @"" + "\n" + @"  { ""currCycleTime"": {    ""value"": " + CurCycTime2 + @",    ""type"": ""Integer""  }," + "\n" + @"  ""drawer1Status"": {""value"": ""Execute"", ""type"": ""String""  }  }";
                    string body03 = Convert.ToString(body3);
                    request.AddJsonBody(body03);
                    client.Execute(request);




                    // Updating the order (parts + partsIO)
                    var client4 = new RestClient(URL);
                    var request4 = new RestRequest("v2/entities/" + ON, Method.Patch);
                    request.AddHeader("fiware-service", "robot_info");
                    request.AddHeader("fiware-servicepath", "/demo");
                    request.AddHeader("Content-Type", "application/json");

                    

                    if (prodParts % 25 > 0)
                    {

                       prodPartsIO = prodPartsIO++;
                       var body4 = @"  {     ""prodParts"": {      ""value"": " + prodParts+ @",      ""type"": ""Integer""    },    ""prodPartsIO"": {      ""value"":  " + prodPartsIO + @",      ""type"": ""Integer""    }  }";
                       string body04 = Convert.ToString(body4);
                       request.AddJsonBody(body04);
                        client.Execute(request);
                     }

                    else
                    {

                     var body4 = @"  {     ""prodParts"": {      ""value"": " + prodParts + @",      ""type"": ""Integer""    } }";
                     string body04 = Convert.ToString(body4);
                     request.AddJsonBody(body04);
                     client.Execute(request);


                    }
                    // if all parts are produced set the enddate to the current date
                    if (prodParts == PlanPart )
                    {

                      string EndData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


                      // updating the order, setting the orderstatus to complete and the enddate
                      var client5 = new RestClient(URL);
                      var request5 = new RestRequest("v2/entities/" + ON, Method.Patch);
                      request.AddHeader("fiware-service", "robot_info");
                      request.AddHeader("fiware-servicepath", "/demo");
                      request.AddHeader("Content-Type", "application/json");
                      var body5 = @"{    ""finishedTime"": {     ""value"": """ + EndData + @""",      ""type"": ""Datetime""    },    ""orderStatus"": {    ""value"": ""Complete"",      ""type"": ""String""    }  }";
                      string body05 = Convert.ToString(body5);
                      request.AddJsonBody(body05);
                      client.Execute(request);

                    }
            }
        }
    }

