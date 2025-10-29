const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:5001';


const PROXY_CONFIG = [
  {
    context: [
      "/WeatherForecast",
    ],
    target,
    secure: false
  }
]

console.log(`\nCurrent target: ${target}\n`);
//console.log("\nCurrent PROXY_CONFIG:\n", PROXY_CONFIG, "\n");

module.exports = PROXY_CONFIG;
