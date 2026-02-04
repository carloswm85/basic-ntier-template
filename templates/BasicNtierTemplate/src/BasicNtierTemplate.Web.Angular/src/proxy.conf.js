const { env } = require("process");

const target = env.ASPNETCORE_HTTPS_PORT
    ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
    : env.ASPNETCORE_URLS
      ? env.ASPNETCORE_URLS.split(";")[0]
      : "http://localhost:5001";

const PROXY_CONFIG = [
    {
        context: ["/api/WeatherForecasts/data"],
        proxyTimeout: 10000,
        target: target,
        secure: false,
        headers: {
            Connection: "Keep-Alive",
        },
    },
];

//console.log(`\nenv.ASPNETCORE_HTTPS_PORT: ${env.ASPNETCORE_HTTPS_PORT}\n`);
//console.log(`\nenv.ASPNETCORE_URLS: ${env.ASPNETCORE_URLS}\n\n`);
//console.log(`\nCurrent target: ${target}\n`);
//console.log("\nCurrent PROXY_CONFIG:\n", PROXY_CONFIG, "\n");

module.exports = PROXY_CONFIG;
