const { ServiceBroker } = require("moleculer");
const ApiService = require("moleculer-web");

let transporter = process.env.TRANSPORTER || "TCP";

const broker = new ServiceBroker({
    nodeID: "node-1",
    logger: true,
    transporter: {
        type: "TCP",
        options: {
            udpDiscovery: false,
            urls: [
                "127.0.0.1:6000/node-1", 
            ],
        }
    }
});

broker.createService({
    mixins: [ApiService],
    name: "address",
    settings: {
        port: 3001
    },
    actions: {
        createAddress(ctx) {
            return "addressCreated";
        }
    }
});

 

broker.start();

