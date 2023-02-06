const axios = require('axios');
const cratedb_url = 'http://host.docker.internal:4200';
const fiware_url = 'http://host.docker.internal:1026'


const getOrderTimeSeriesDataFromCrateDB = async(id) => {
    const response = await axios.post(`${cratedb_url}/_sql`, {
        "stmt": `SELECT entity_id, entity_type, time_index, fiware_servicepath, __original_ngsi_entity__, i40physicalmodeltype, i40assetname, productid, workstationid, planparts, prodparts, prodpartsio, starttime, finishedtime, deadline, orderstatus, oee, oeeperfomrance, oeequality
        FROM "mtrobot_info"."eti40assetorder" WHERE entity_id = '${id}' ORDER BY time_index DESC
        LIMIT 100;`
    });
    return response;
};

const getWorkingStationTimeSeriesDataFromCrateDB = async(id) => {
    const response = await axios.post(`${cratedb_url}/_sql`, {
        "stmt": `SELECT entity_id, entity_type, time_index, fiware_servicepath, __original_ngsi_entity__, i40physicalmodeltype, i40assetname, orderid, robotrunning, robotspeed, currcycletime, drawer1status, drawer2status, restservicelife
        FROM "mtrobot_info"."eti40assetworkingstation" where entity_id ='${id}' 
        ORDER BY time_index DESC
        LIMIT 100;`
    });
    return response;
}

getProductTimeSeriesDataFromCrateDB = async(id) => {
    const response = await axios.post(`${cratedb_url}/_sql`, {
        "stmt": `SELECT entity_id, entity_type, time_index, fiware_servicepath, __original_ngsi_entity__, i40physicalmodeltype, i40assetname, programname, programversion, versiononrobot, processinglength, plancycletime, pdf
        FROM "mtrobot_info"."eti40assetproduct" WHERE entity_id = '${id}' 
        ORDER BY time_index DESC
        LIMIT 100;`
    });
    return response;
}

const updatingOrder = async(id, oeePerfomrance, oeeQuality, oee) => {
    var body = {
        oeePerfomrance: {
            value: oeePerfomrance,
            type: "Number"
        },
        oeeQuality: {
            value: oeeQuality,
            type: "Number"
        },
        oee: {
            value: oee,
            type: "Number"
        },
    }
    body = JSON.stringify(body);
    const response = await axios.patch(`${fiware_url}/v2/entities/${id}/attrs`,
        body, {
            headers: {
                'content-Type': 'application/json',
                'fiware-Service': ' robot_info',
                'fiware-servicepath': '/demo',
            }
        }
    );

    return response;
}

const calculateOEE = (orderId, prodParts, prodPartsIO, finishedTime, startTime, deadline, planCycleTime, currCycleTime) => {
    let performance = planCycleTime / currCycleTime
    let oeeQuality = prodPartsIO / prodParts;
    console.log(prodPartsIO);
    console.log(prodParts);

    let availability = (finishedTime - startTime) / (deadline - startTime);

    console.log(availability);
    console.log(performance),
        console.log(oeeQuality)
    let oeePerformance = performance * oeeQuality;
    let oee = availability * performance * oeeQuality;


    console.log(oeeQuality, oeePerformance, oee);
    updatingOrder(orderId, oeePerformance * 100, oeeQuality * 100, oee * 100);
}

exports.calculateOEE = (id) => {
    getOrderTimeSeriesDataFromCrateDB(id).then((orderTimeSeries) => {
        const orderId = orderTimeSeries.data.rows[0][0]
        const workingStationId = orderTimeSeries.data.rows[0][8]
        const productId = orderTimeSeries.data.rows[0][7]
        const prodParts = orderTimeSeries.data.rows[0][10];
        const planParts = orderTimeSeries.data.rows[0][8];
        const prodPartsIO = orderTimeSeries.data.rows[0][11];
        const startTime = orderTimeSeries.data.rows[0][11]
        const finishedTime = orderTimeSeries.data.rows[0][12];
        const deadline = orderTimeSeries.data.rows[0][13]
        console.log(orderTimeSeries.data.rows[0]);

        getWorkingStationTimeSeriesDataFromCrateDB('urn:ngsiv2:I40Asset:Workstation00001').then((workingStationTimeSeries) => {
            const currCycleTime = workingStationTimeSeries.data.rows[0][10]

            getProductTimeSeriesDataFromCrateDB('urn:ngsiv2:I40Asset:Product00001').then((productTimeSeries) => {
                const planCycleTime = productTimeSeries.data.rows[0][11]
                calculateOEE(orderId, prodParts, prodPartsIO, finishedTime, startTime, deadline, planCycleTime, currCycleTime);
            })

        });




    });

};