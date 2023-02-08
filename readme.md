# General

The Simulation and the Preprocessor are based on the datatypes in the programmhandler. The main three Types are:
- I40AssetWorkingStation
- I40AssetOrder
- I40AssetProduct

For every type of asset, a subscription is needed to save historical data in the CrateDB

# Simulation
### Description
With the help of the production simulation datase traffic is generated on FIWARE. Thereby the traffic for the data types i40AssetOrder and i40AssetWorkstation is created. The prerequisite is that the corresponding i40AssetOrder and i40AssetWorkstation datasets already exist in FIWARE.
After starting the simulation, the following patches are carried out:

#### i40AssetOrder
![image](https://user-images.githubusercontent.com/102011176/217534949-ec7f159d-d6a7-444d-8042-2cdc53575f3b.png)

With patches, the "produced components" and the "produced components with sufficient quality" are changed in real time (after a specified cycle time has elapsed).
Furthermore, the order status is set to "Completed" after completion of the order.

#### i40AssetWorkstation
![image](https://user-images.githubusercontent.com/102011176/217536109-5b60bae1-8370-4bfc-be36-1192b9d44b98.png)

For this dataset, the following attributes are changed during runtime:
* Current cycle time
* Status drawer 1
* Status drawer 2
* Robot running

The cycle time is determined and set after each cycle (when the "Complete" status is set). 
Robot running is always set to true when the robot is running a process, otherwise it is set to false. 
The drawer statuses are set in a specific order:

1. raw part drawer 1
2. process finished drawer 2
3. process in drawer 1
4. raw part drawer 2
5. process finished drawer 1
6. process in drawer 2

### How to start the simulation

1. Open Program.cs
2. Change the property "ID" to the ID of the workingstation
3. Change the property "ON" to the ID of the order
4. If the simulation is running outside of a docker container change also the URL. This is the URL of fiware
5. The next property which needs to be changed is "PlanParts".
6. Now you can do a docker-compose up. 
7. If you start the program without docker run the following command in the terminal:
`dotnet watch run`
---
# PreProcessor
### How to start the preprocessor

### General

The preprocessor calculates the following oee metrics:
- quality
- performance
- oee

Every 10 seconds data is retrieved from crateDB and these metrics are calculated from the last data of the last entry. Hte calculated data is sent to the PATCH endpoint of the dataset of fiware.


### Prerequisits

At least one entry of every single assettype in the crateDB.
The preprocessor gets all the data from the crateDB, except the types which are specified in the code. If the type changes, a change in the code is needed (app.js)

### How to start the preprocessor

1. if docker is used just do a docker-compose up otherwise run following command in the terminal:

`node app.js`



