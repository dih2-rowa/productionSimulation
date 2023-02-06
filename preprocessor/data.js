const axios = require('axios');
const fs = require('fs')

const getTimeSeriesDataOrder = async(id) => {
    const response = await axios.post(`http://localhost:4200/_sql`, {
        "stmt": `SELECT entity_id, entity_type, time_index, fiware_servicepath, __original_ngsi_entity__, i40physicalmodeltype, i40assetname, productid, workstationid, planparts, prodparts, prodpartsio, starttime, finishedtime, deadline, orderstatus, oee, oeeperfomrance, oeequality
        FROM "mtrobot_info"."eti40assetorder"  ORDER BY time_index ASC
        LIMIT 1000`
    });
    return response;
};

const getWorkingStationTimeSeriesDataFromCrateDB = async() => {
    const response = await axios.post(`http://localhost:4200/_sql`, {
        "stmt": `SELECT entity_id, entity_type, time_index, fiware_servicepath, __original_ngsi_entity__, i40physicalmodeltype, i40assetname, orderid, robotrunning, robotspeed, currcycletime, drawer1status, drawer2status, restservicelife
        FROM "mtrobot_info"."eti40assetworkingstation"
        ORDER BY time_index ASC
        LIMIT 1000;`
    });
    return response;
}


exports.getData = () => {
    getTimeSeriesDataOrder().then((response) => {
        const jsonString = JSON.stringify(response.data)
        fs.writeFile('./orderTimeSeriesData.json', jsonString, err => {
            if (err) {
                console.log('Error writing file', err)
            } else {
                console.log('Successfully wrote file')
            }
        })

    });

    getWorkingStationTimeSeriesDataFromCrateDB().then((response) => {
        const jsonString = JSON.stringify(response.data)
        fs.writeFile('./workingstationTimeSeriesData.json', jsonString, err => {
            if (err) {
                console.log('Error writing file', err)
            } else {
                console.log('Successfully wrote file')
            }
        })
    })
}