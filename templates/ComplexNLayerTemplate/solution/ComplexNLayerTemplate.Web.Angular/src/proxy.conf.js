const { env } = require("process");

const target = env.ASPNETCORE_HTTPS_PORT
    ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
    : env.ASPNETCORE_URLS
      ? env.ASPNETCORE_URLS.split(";")[0]
      : "http://localhost:5001";

const PROXY_CONFIG = [
    {
        context: ["/api/v1/"],
        proxyTimeout: 10000,
        target: target,
        secure: false,
        headers: {
            Connection: "Keep-Alive",
        },
    },
];

console.log(`Current Web API target: ${target}/swagger/index.html`);
console.log(`env.ASPNETCORE_HTTPS_PORT variable: ${env.ASPNETCORE_HTTPS_PORT}`);
console.log(`env.ASPNETCORE_URLS variable: ${env.ASPNETCORE_URLS}`);
console.log("\nCurrent PROXY_CONFIG value:\n", PROXY_CONFIG, "\n");

module.exports = PROXY_CONFIG;
