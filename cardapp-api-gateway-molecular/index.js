const { ServiceBroker } = require("moleculer");
const ApiService = require("moleculer-web");
 
let transporter = process.env.TRANSPORTER || "TCP";

const broker = new ServiceBroker({
    nodeID: "node-2",
    logger: true,
    transporter,
});
 

 

broker.createService({
    mixins: [ApiService],
name:"api-gateway",
    settings: {
        routes: [{
            aliases: {
                
                "GET test": "address.createAddress"
            }
        }]
    }
});
 
 

 

broker.start()
    .then(() => broker.waitForServices("address"))
    .then(() => broker.call("address.createAddress"))
    .then(res => console.log(res))
    .catch(err => console.error(`Error occured! ${err.message}`));