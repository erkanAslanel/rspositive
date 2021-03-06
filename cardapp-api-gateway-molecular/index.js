const { ServiceBroker } = require("moleculer");
const ApiService = require("moleculer-web");

let transporter = process.env.TRANSPORTER || "TCP";

const broker = new ServiceBroker({
    nodeID: "node-2",
    logger: true,
    transporter: {
        type: "TCP",
        options: {
            udpDiscovery: false,
            urls: [
                "127.0.0.1:6001/node-1", 
            ],
        }
    }
});


broker.createService({
    mixins: [ApiService],
    name: "api-gateway",
    settings: {
        routes: [{
            path: "/api/address",
            mappingPolicy: "restrict",
            aliases: {
                "POST create": "address.createAddress"
            } 
        }]
    }
});


broker.start();
