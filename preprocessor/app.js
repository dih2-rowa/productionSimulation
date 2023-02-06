const runner = require('./preprocessor')
    // const { Client } = require('cratedb');
const { default: axios } = require('axios');
const getData = require('./data');

const endpoint = 'http://host.docker.internal:1026/v2/entities';
const type = 'I40AssetOrder';

const getEntitiesByType = async(type) => {

    const result = await axios.get(`${endpoint}?type=${type}`, {
        headers: {
            'fiware-service': 'robot_info',
            'fiware-servicepath': '/demo'
        }
    });
    console.log(result.data)
    return result;
}

// id you want timeseries data from crate db uncomment the next line
// getData.getData();


setInterval(function() {
    getEntitiesByType(type).then((orders) => {
        // call your function here
        console.log(orders.data);
        for (const order of orders.data) {
            runner.calculateOEE(order.id)
        }
    })
}, 10000);