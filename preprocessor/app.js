const runner = require('./preprocessor')
    // const { Client } = require('cratedb');
const { default: axios } = require('axios');

const endpoint = 'http://localhost:1026/v2/entities';
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


setInterval(function() {
    getEntitiesByType(type).then((orders) => {
        // call your function here
        console.log(orders.data);
        for (const order of orders.data) {
            runner.calculateOEE(order.id)
        }
    })
}, 10000);