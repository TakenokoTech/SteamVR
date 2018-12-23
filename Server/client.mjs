import grpc from "grpc";
import protoLoader from "@grpc/proto-loader";

const avaterProto = grpc.loadPackageDefinition(
    protoLoader.loadSync("../Protos/avater.proto", {
        keepCase: true,
        longs: String,
        enums: String,
        defaults: true,
        oneofs: true
    })
).avater;

function main() {
    const client = new avaterProto.Position(
        "localhost:50051",
        grpc.credentials.createInsecure()
    );
    console.debug(client.update);

    let user = "world";
    client.update({ name: user }, (err, response) => {
        err && console.log("Err:", err);
        response && console.log("Greeting:", response.message);
    });
}
main();
