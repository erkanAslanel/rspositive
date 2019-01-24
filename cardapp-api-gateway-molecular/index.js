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
                "127.0.0.1:6000/node-1",

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
            },
            onAfterCall(ctx, route, req, res, data) {
                res.setHeader("X-Custom-Header", "123456");
            },
        }]
    }
});


broker.start()
    .then(() => broker.waitForServices("address"))
    .then(() => broker.call("address.createAddress"))
    .then(res => console.log(res))
    .catch(err => console.error(`Error occured! ${err.message}`));